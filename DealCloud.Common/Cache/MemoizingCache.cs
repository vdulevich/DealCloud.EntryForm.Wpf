using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using DealCloud.Common.Interfaces;
using System.Threading.Tasks;
using NLog;

namespace DealCloud.Common.Cache
{
    /// <summary>
    ///     Cache method calls. Cache limited by capacity and timeout. Particular items can be invalidated
    /// </summary>
    [Serializable]
    public class MemoizingCache : IDeserializationCallback
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private const string CACHE_DELIM = "^|^";

        private static bool _usePerfCounters = true;

        private readonly Dictionary<int, ICache<int>> _caches;

        private readonly int _capacity;

        private readonly ICacheInfoProvider _contextProvider;

        private readonly ConcurrentDictionary<int, int> _keysCurrentlyUpdating = new ConcurrentDictionary<int, int>();

        private readonly TimeSpan _timeout;

        [NonSerialized]
        private readonly PerformanceCounter _pcHits;

        [NonSerialized]
        private readonly PerformanceCounter _pcMisses;

        [NonSerialized] 
        private readonly PerformanceCounter _pcRequests;

        [NonSerialized]
        private ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        /// <summary>
        ///     Returns version resolver which was set during cache contruction
        /// </summary>
        public ICacheVersionHelper<int> VersionResolver { get; }

        public MemoizingCache(string name, int capacity, TimeSpan timeout, ICacheVersionHelper<int> versionResolver = null, ICacheInfoProvider contextProvider = null)
        {
            if (capacity <= 0) throw new ArgumentException($"{nameof(capacity)} should be positive integer.");

            _capacity = capacity;
            _timeout = timeout;
            VersionResolver = versionResolver;
            _contextProvider = contextProvider;
            _caches = new Dictionary<int, ICache<int>>();

            try
            {
                var instanceName = $"{Environment.UserName} - {name}";

                _pcHits = new PerformanceCounter(Constants.PERF_CATEGORY_CACHE, Constants.PERF_COUNTER_HITS, instanceName, false) {RawValue = 0};
                _pcMisses = new PerformanceCounter(Constants.PERF_CATEGORY_CACHE, Constants.PERF_COUNTER_MISSES, instanceName, false) { RawValue = 0 };
                _pcRequests = new PerformanceCounter(Constants.PERF_CATEGORY_CACHE, Constants.PERF_COUNTER_REQUESTS, instanceName, false) { RawValue = 0 };

                _pcHits.NextSample();
                _pcMisses.NextSample();
                _pcRequests.NextSample();
            }
            catch
            {
                _usePerfCounters = false; //NOTE: can't initialize perf counters, so won't use them
            }
        }

        public void OnDeserialization(object sender)
        {
            _cacheLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        ///     Returns value from the cache, calls supplied method if needed
        /// </summary>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue>(Func<TValue> method, params string[] tags)
        {
            return GetValue(method, _timeout, false, tags);
        }

        public TValue GetValue<TValue>(Func<TValue> method, bool useCurrentValueIfUpdateInProgress, params string[] tags)
        {
            return GetValue(method, _timeout, useCurrentValueIfUpdateInProgress, tags);
        }

        /// <summary>
        ///     Returns value from the cache, calls supplied method if needed. Method may do a delta changes from CacheBuildDate supplied to the method
        /// </summary>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue>(Func<DateTime, TValue, TValue> method, params string[] tags)
        {
            return GetValue(method, _timeout, false, tags);
        }

        /// <summary>
        ///     Returns value from the cache, calls supplied method if needed. Method may do a delta changes from CacheBuildDate supplied to the method
        /// </summary>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue>(Func<DateTime, TValue, TValue> method, TimeSpan timeout, params string[] tags)
        {
            return GetValue(method, _timeout, false, tags);
        }

        /// <summary>
        ///     Returns value from the cache, calls supplied method if needed. Method may do a delta changes from CacheBuildDate supplied to the method
        /// </summary>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue>(Func<DateTime, TValue, TValue> method, bool useCurrentValueIfUpdateInProgress, params string[] tags)
        {
            return GetValue(method, _timeout, useCurrentValueIfUpdateInProgress, tags);
        }

        /// <summary>
        ///     Returns value from the cache, calls supplied method if needed.
        /// </summary>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue>(Func<DateTime, TValue, TValue> method, TimeSpan timeout, bool useCurrentValueIfUpdateInProgress, params string[] tags)
        {
            if (_usePerfCounters) _pcRequests.Increment();

            var clientId = _contextProvider?.GetClientId() ?? 0;
            var key = CreateKey(clientId, method.Method.Name, tags);
            var cache = GetCache(clientId);

            try
            {
                object cachedValue;
                DateTime created;
                bool startUpdate;
                if (!NeedCall(key, cache, out created, out cachedValue, out startUpdate, useCurrentValueIfUpdateInProgress))
                {
                    if (useCurrentValueIfUpdateInProgress && startUpdate)
                    {
                        var identity = _contextProvider?.GetIdentity();
                        var items = _contextProvider?.Items;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                _contextProvider?.BeginAsyncCallProcessing(identity, items);
                                GetValue(method, timeout, false, tags);
                            }
                            catch (Exception ex)
                            {
                                _log.Error(ex);
                            }
                            finally
                            {
                                _contextProvider?.EndAsyncCallProcessing();
                            }
                        });
                    }
                    return (TValue)cachedValue;
                }

                long version = 0;
                var tref = new VersionedTimeoutReference<object>(null, timeout, true, version); //NOTE: create versioned reference
                var value = method(created != DateTime.MinValue ? created.AddMinutes(-5) : created, (TValue) cachedValue); //NOTE: call method, allow 5 minutes difference between Server and DBServer, in the worst case we will reload more records...

                if (VersionResolver != null) version = VersionResolver.GetVersion(key);

                tref.SetTarget(value, version); //NOTE: set value to versioned reference

                if (_usePerfCounters) _pcMisses.Increment();

                cache.Add(key, tref, tags);

                VersionResolver?.SetVersion(key, version, tags);

                return value;
            }
            finally
            {
                int threadId;

                if (_keysCurrentlyUpdating.TryGetValue(key, out threadId) && threadId == Thread.CurrentThread.ManagedThreadId)
                {
                    _keysCurrentlyUpdating.TryRemove(key, out threadId);
                }
            }
        }


        /// <summary>
        ///     Returns value from the cache, calls supplied method if needed
        /// </summary>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue>(Func<TValue> method, TimeSpan timeout, bool useCurrentValueIfUpdateInProgress, params string[] tags)
        {
            if (_usePerfCounters) _pcRequests.Increment();

            var clientId = _contextProvider?.GetClientId() ?? 0;
            var key = CreateKey(clientId, method.Method.Name, tags);
            var cache = GetCache(clientId);

            try
            {
                object cachedValue;
                DateTime dummy;
                bool startUpdate;
                if (!NeedCall(key, cache, out dummy, out cachedValue, out startUpdate, useCurrentValueIfUpdateInProgress))
                {
                    if (useCurrentValueIfUpdateInProgress && startUpdate)
                    {
                        var identity = _contextProvider?.GetIdentity();
                        var items = _contextProvider?.Items;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                _contextProvider?.BeginAsyncCallProcessing(identity, items);
                                GetValue(method, timeout, false, tags);
                            }
                            catch (Exception ex)
                            {
                                _log.Error(ex);
                            }
                            finally
                            {
                                _contextProvider?.EndAsyncCallProcessing();
                            }
                        });
                    }
                    return (TValue)cachedValue;
                }

                long version = 0;
                var value = method();

                if (VersionResolver != null) version = VersionResolver.GetVersion(key);

                if (_usePerfCounters) _pcMisses.Increment();

                cache.Add(key, new VersionedTimeoutReference<object>(value, timeout, true, version), tags);

                VersionResolver?.SetVersion(key, version, tags);

                return value;
            }
            finally
            {
                int threadId;

                if (_keysCurrentlyUpdating.TryGetValue(key, out threadId) && threadId == Thread.CurrentThread.ManagedThreadId)
                {
                    _keysCurrentlyUpdating.TryRemove(key, out threadId);
                }
            }
        }
            
        private bool NeedCall(int key, ICache<int> cache, out DateTime createdTime, out object result, out bool startUpdate, bool useCurrentValueIfUpdateInProgress = false)
        {
            //TODO: review and possibly redo with while(true) statement
            DO_IT_AGAIN:

            result = null;
            createdTime = DateTime.MinValue;
            startUpdate = false;
            var tref = cache[key] as VersionedTimeoutReference<object>;

            if (tref != null)
            {
                result = tref.Target;
                var needreset = false;

                if (tref.IsAlive)
                {
                    var version = tref.Version;

                    if ((VersionResolver == null) || (!VersionResolver.RequireUpdate(key, version, out needreset)))
                    {
                        if (_usePerfCounters) _pcHits.Increment();

                        return false;
                    }
                }
                if (needreset)
                {
                    result = null;
                    tref = null;
                }
                else
                {
                    createdTime = tref.Created; //NOTE: need to return DateTime from reference
                }
            }

            var threadId = Thread.CurrentThread.ManagedThreadId;

            if (tref != null && useCurrentValueIfUpdateInProgress) //NOTE: something in the cache and we can use this value
            {
                startUpdate = useCurrentValueIfUpdateInProgress && _keysCurrentlyUpdating.TryAdd(key, threadId);
                return false; //NOTE: current thread need to be added as later we will be removing it, and we can remove the key which is in progress of update
            }
            if (!_keysCurrentlyUpdating.TryAdd(key, threadId)) //NOTE: some other thread in progress of update
            {
                //TODO: review and possibly redo with ManualResetEvent
                Thread.Sleep(1); //NOTE: wait and check again

                goto DO_IT_AGAIN;
            }

            //NOTE: key was added now need to perform actual method call (outside)
            startUpdate = useCurrentValueIfUpdateInProgress;
            return true;
        }

        /// <summary>
        ///     Get results of the method call with 1 parameter from cache, if not there calls method
        /// </summary>
        /// <returns>Cached value (result of function call)</returns>
        public TValue GetValue<TValue, TParam>(TParam param, Func<TParam, TValue> method, params string[] tags)
        {
            return GetValue(param, method, _timeout, tags);
        }

        /// <summary>
        ///     Returns value from the cache, if values not in the cache call supplied method. It creates key for a cache like string.Join(parameters) + tags
        ///     so in order to work params should have ToString() method well defined (not just return class name)
        /// </summary>
        /// <param name="method">
        ///     Method to execute when value not in cache, method should Accept DateTiem - this is when values was last placed into cache and current cache value
        /// </param>
        /// <param name="tags">Tags assigned to cache value</param>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue, TParam>(TParam param, Func<TParam, DateTime, TValue, TValue> method, params string[] tags)
        {
            return GetValue(param, method, _timeout, false, tags);
        }

        /// <summary>
        ///     Returns value from the cache, if values not in the cache call supplied method. It creates key for a cache like string.Join(parameters) + tags
        ///     so in order to work params should have ToString() method well defined (not just return class name)
        /// </summary>
        /// <param name="method">
        ///     Method to execute when value not in cache, method should Accept DateTiem - this is when values was last placed into cache and current cache value
        /// </param>
        /// <param name="tags">Tags assigned to cache value</param>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue, TParam>(TParam param, Func<TParam, DateTime, TValue, TValue> method, bool useCurrentValueIfUpdateInProgress, params string[] tags)
        {
            return GetValue(param, method, _timeout, useCurrentValueIfUpdateInProgress, tags);
        }

        /// <summary>
        ///     Returns value from the cache, if values not in the cache call supplied method. It creates key for a cache like string.Join(parameters) + tags
        ///     so in order to work params should have ToString() method well defined (not just return class name)
        /// </summary>
        /// <param name="method">
        ///     Method to execute when value not in cache, method should Accept DateTiem - this is when values was last placed into cache and current cache value
        /// </param>
        /// <param name="tags">Tags assigned to cache value</param>
        /// <returns>Cached value (result of fucntion call)</returns>
        public TValue GetValue<TValue, TParam>(TParam param, Func<TParam, DateTime, TValue, TValue> method, TimeSpan timeout, bool useCurrentValueIfUpdateInProgress, params string[] tags)
        {
            if (_usePerfCounters) _pcRequests.Increment();

            var clientId = _contextProvider?.GetClientId() ?? 0;
            var key = CreateKey(clientId, method.Method.Name, tags, param);
            var cache = GetCache(clientId);

            try
            {
                object cachedValue;
                DateTime created;
                bool startUpdate;
                if (!NeedCall(key, cache, out created, out cachedValue, out startUpdate, useCurrentValueIfUpdateInProgress))
                {
                    if (useCurrentValueIfUpdateInProgress && startUpdate)
                    {
                        var identity = _contextProvider?.GetIdentity();
                        var items = _contextProvider?.Items;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                _contextProvider?.BeginAsyncCallProcessing(identity, items);
                                GetValue(param, method, timeout, false, tags);
                            }
                            catch (Exception ex)
                            {
                                _log.Error(ex);
                            }
                            finally
                            {
                                _contextProvider?.EndAsyncCallProcessing();
                            }
                        });
                    }
                    return (TValue)cachedValue;
                }

                long version = 0;
                var tref = new VersionedTimeoutReference<object>(null, timeout, true, version); //NOTE: create versioned reference
                var value = method(param, created != DateTime.MinValue ? created.AddMinutes(-5) : created, (TValue) cachedValue); //NOTE: call method

                if (VersionResolver != null) version = VersionResolver.GetVersion(key);

                tref.SetTarget(value, version); //NOTE: set value to versioned reference

                if (_usePerfCounters) _pcMisses.Increment();

                cache.Add(key, tref, tags);

                VersionResolver?.SetVersion(key, version, tags);

                return value;
            }
            finally
            {
                int threadId;

                if (_keysCurrentlyUpdating.TryGetValue(key, out threadId) && threadId == Thread.CurrentThread.ManagedThreadId)
                {
                    _keysCurrentlyUpdating.TryRemove(key, out threadId);
                }
            }
        }

        /// <summary>
        ///     Get results of the method call with 1 parameter from cache, if not there calls method
        /// </summary>
        /// <returns>Cached value (result of function call)</returns>
        public TValue GetValue<TValue, TParam>(TParam param, Func<TParam, TValue> method, TimeSpan timeout, params string[] tags)
        {
            return GetValue(param, method, timeout, false, tags);
        }


        /// <summary>
        ///     Get results of the method call with 1 parameter from cache, if not there calls method
        /// </summary>
        /// <returns>Cached value (result of function call)</returns>
        public TValue GetValue<TValue, TParam>(TParam param, Func<TParam, TValue> method, TimeSpan timeout, bool useCurrentValueIfUpdateInProgress, params string[] tags)
        {
            if (_usePerfCounters) _pcRequests.Increment();

            var clientId = _contextProvider?.GetClientId() ?? 0;
            var key = CreateKey(clientId, method.Method.Name, tags, param);
            var cache = GetCache(clientId);

            try
            {
                object cachedValue;
                DateTime dummy;
                bool startUpdate;
                if (!NeedCall(key, cache, out dummy, out cachedValue, out startUpdate, useCurrentValueIfUpdateInProgress))
                {
                    if (useCurrentValueIfUpdateInProgress && startUpdate)
                    {
                        var identity = _contextProvider?.GetIdentity();
                        var items = _contextProvider?.Items;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                _contextProvider?.BeginAsyncCallProcessing(identity, items);
                                GetValue(param, method, timeout, false, tags);
                            }
                            catch (Exception ex)
                            {
                                _log.Error(ex);
                            }
                            finally
                            {
                                _contextProvider?.EndAsyncCallProcessing();
                            }
                        });
                    }
                    return (TValue)cachedValue;
                }

                long version = 0;
                var value = method(param);

                if (VersionResolver != null) version = VersionResolver.GetVersion(key);

                if (_usePerfCounters) _pcMisses.Increment();

                cache.Add(key, new VersionedTimeoutReference<object>(value, timeout, true, version), tags);

                VersionResolver?.SetVersion(key, version, tags);

                return value;
            }
            finally
            {
                int threadId;

                if (_keysCurrentlyUpdating.TryGetValue(key, out threadId) && threadId == Thread.CurrentThread.ManagedThreadId)
                {
                    _keysCurrentlyUpdating.TryRemove(key, out threadId);
                }
            }
        }

        /// <summary>
        ///     Get results of the method call with 2 parameters from cache, if not there calls method
        /// </summary>
        /// <returns>Cached value (result of function call)</returns>
        public TValue GetValue<TValue, TParam1, TParam2>(TParam1 param1, TParam2 param2, Func<TParam1, TParam2, TValue> method, params string[] tags)
        {
            return GetValue(param1, param2, method, _timeout, tags);
        }

        /// <summary>
        ///     Get results of the method call with 2 parameters from cache, if not there calls method
        /// </summary>
        /// <returns>Cached value (result of function call)</returns>
        public TValue GetValue<TValue, TParam1, TParam2>(TParam1 param1, TParam2 param2, Func<TParam1, TParam2, TValue> method, TimeSpan timeout, params string[] tags)
        {
            if (_usePerfCounters) _pcRequests.Increment();

            var clientId = _contextProvider?.GetClientId() ?? 0;
            var key = CreateKey(clientId, method.Method.Name, tags, param1, param2);
            var cache = GetCache(clientId);

            try
            {
                object cachedValue;
                DateTime dummy;
                bool ignore;
                if (!NeedCall(key, cache, out dummy, out cachedValue, out ignore)) return (TValue) cachedValue;

                long version = 0;
                var value = method(param1, param2);

                if (VersionResolver != null) version = VersionResolver.GetVersion(key);

                if (_usePerfCounters) _pcMisses.Increment();

                cache.Add(key, new VersionedTimeoutReference<object>(value, timeout, true, version), tags);

                VersionResolver?.SetVersion(key, version, tags);

                return value;
            }
            finally
            {
                int threadId;

                if (_keysCurrentlyUpdating.TryGetValue(key, out threadId) && threadId == Thread.CurrentThread.ManagedThreadId)
                {
                    _keysCurrentlyUpdating.TryRemove(key, out threadId);
                }
            }
        }

        /// <summary>
        ///     Invalidates multimple cache entries by any of the provided tags, tags must be specified when creating cache entries
        /// </summary>
        public void Invalidate(params string[] tags)
        {
            if (tags == null || tags.Length == 0) throw new ArgumentException("At least 1 tag must be specified");

            var cache = GetCache(_contextProvider?.GetClientId() ?? 0);

            if (VersionResolver != null)
            {
                VersionResolver.IncrementVersion(tags);
            }
            else
            {
                cache.RemoveByTags(tags);
            }
        }

        /// <summary>
        ///     Clears all values in the cache
        /// </summary>
        public void Clear()
        {
            var cache = GetCache(_contextProvider?.GetClientId() ?? 0);

            VersionResolver?.IncrementVersion(null);

            cache.Clear();
        }

        private ICache<int> GetCache(int clientId)
        {
            _cacheLock.EnterReadLock();

            try
            {
                ICache<int> ret;

                if (_caches.TryGetValue(clientId, out ret)) return ret;
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }

            _cacheLock.EnterUpgradeableReadLock();

            try
            {
                ICache<int> ret;

                if (_caches.TryGetValue(clientId, out ret)) return ret; //NOTE: One more try to get cache, may be some one just created it

                _cacheLock.EnterWriteLock();

                try
                {
                    var cache = new LruCache<int>(_capacity);

                    _caches[clientId] = cache;

                    return cache;
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
            }
            finally
            {
                _cacheLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        ///     Creates key by supplied parameters. it uses string.GetHashCode() for key creation, this may end up with some collisions
        /// </summary>
        private static int CreateKey(int clientId, string methodName, string[] tags, params object[] keys)
        {
            var sb = new StringBuilder(100)
                //commented out this 2 lines so we will have more predictable cache keys on the QA time (we should not use use dynamic parameters which will appear in Prod time but may not be tested in QA)
                //.Append(clientId)
                //.Append(CACHE_DELIM)
                .Append(methodName)
                .Append(CACHE_DELIM)
                .Append(string.Join(CACHE_DELIM, keys));

            if (tags != null && tags.Length > 0) sb.Append(CACHE_DELIM).Append(string.Join(CACHE_DELIM, tags));

            //TODO: review because .GetHashCode() may end up with some collisions
            return sb.ToString().GetHashCode();
        }
    }
}
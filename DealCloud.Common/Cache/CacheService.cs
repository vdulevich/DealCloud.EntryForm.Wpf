using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading;
using DealCloud.Common.Interfaces;
using NLog;
using DealCloud.Common.Extensions;
using Unity.Resolution;

namespace DealCloud.Common.Cache
{
    public static class CacheService
    {
        public const int DEFAULT_TIMEOUT = 3600*12;

        private const int DEFAULT_CAPACITY = 1024;

        private const string SIZE_STRING = "_Size";

        private const string TIMEOUT_STRING = "_Timeout";

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<string, MemoizingCache> Caches = new Dictionary<string, MemoizingCache>();

        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public static MemoizingCache GetDictionariesCache()
        {
            return GetMemoizingCache(Constants.DICTIONARIES_CACHE);
        }

        public static MemoizingCache GetCommonCache()
        {
            return GetMemoizingCache(Constants.COMMON_CACHE, true);
        }

        public static MemoizingCache GetDataCache()
        {
            return GetMemoizingCache(Constants.DATA_CACHE);
        }

        public static MemoizingCache GetDataProvidersCache()
        {
            return GetMemoizingCache(Constants.DATA_PROVIDERS_CACHE, true);
        }

        public static MemoizingCache GetAddinCacheClient(bool forceCreate = false)
        {
            // on a client Addin cache should be like Common cache (without the knowledge of clientId)
            return GetMemoizingCache(Constants.ADDIN_CACHE, true, forceCreate: forceCreate);
        }

        public static MemoizingCache GetAddinCacheServer()
        {
            return GetMemoizingCache(Constants.ADDIN_CACHE);
        }

        internal static MemoizingCache GetMemoizingCache(string name, bool isCommon = false, bool isVersioned = true, bool forceCreate = false)
        {
            var config = ConfigurationManager.GetSection("cacheSettings") as NameValueCollection ?? new NameValueCollection();

            var tmp = config[$"{name}{SIZE_STRING}"];
            int capacity;
            if (!int.TryParse(tmp, out capacity)) capacity = DEFAULT_CAPACITY;

            tmp = config[$"{name}{TIMEOUT_STRING}"];
            int timeout;
            if (!int.TryParse(tmp, out timeout)) timeout = DEFAULT_TIMEOUT;

            return GetMemoizingCache(name, isCommon, isVersioned, capacity, timeout, forceCreate);
        }

        internal static MemoizingCache GetMemoizingCache(string name, bool isCommon, bool isVersioned, int capacity, int timeout = DEFAULT_TIMEOUT, bool forceCreate = false)
        {
            if (name.IsNullOrEmpty()) throw new ArgumentException($"{nameof(name)} could not be null or empty.");
            if (capacity <= 0) throw new ArgumentException($"{nameof(capacity)} should be greater than 0.");
            if (timeout < 0) throw new ArgumentException($"{nameof(timeout)} should be positive integer or 0.");

            Lock.EnterUpgradeableReadLock();

            try
            {
                MemoizingCache result;

                if (!forceCreate && Caches.TryGetValue(name, out result)) return result;

                Lock.EnterWriteLock();

                try
                {
                    var cp = GetCacheInfoProvider(isCommon);

                    if (timeout <= 0) timeout = 3600*24*10*365; //10 years, if not specified

                    var cache = new MemoizingCache(name, capacity, TimeSpan.FromSeconds(timeout), (cp == null || !isVersioned) ? null : new VersionHelper(name, cp), cp);

                    Caches[name] = cache;

                    return cache;
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
            finally
            {
                Lock.ExitUpgradeableReadLock();
            }
        }

        public static void ClearCacheVersions(params string[] names)
        {
            Lock.EnterReadLock();

            try
            {
                if (names == null || names.Length == 0) names = Caches.Keys.ToArray();

                foreach (var name in names)
                {
                    MemoizingCache result;

                    if (!Caches.TryGetValue(name, out result)) continue;

                    result.VersionResolver?.Clear();
                }
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public static void InvalidateByAny(string[] tags, params string[] names)
        {
            if (tags == null || tags.Length == 0) throw new ArgumentException($"{nameof(tags)} must have at least 1 tag.");

            Lock.EnterReadLock();

            try
            {
                if (names == null || names.Length == 0) names = Caches.Keys.ToArray();

                foreach (var name in names)
                {
                    MemoizingCache result;

                    if (!Caches.TryGetValue(name, out result)) continue;

                    result.Invalidate(tags);
                }
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public static void ClearCaches(params string[] names)
        {
            Lock.EnterReadLock();

            try
            {
                if (names == null || names.Length == 0) names = Caches.Keys.ToArray();

                foreach (var name in names)
                {
                    MemoizingCache result;

                    if (!Caches.TryGetValue(name, out result)) continue;

                    result.Clear();
                }
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        private static ICacheInfoProvider GetCacheInfoProvider(bool isCommonDb = false)
        {
            ICacheInfoProvider result = null;

            try
            {
                result = IoContainer.ResolveWithParamsOverride<ICacheInfoProvider>(new ParameterOverride(nameof(isCommonDb), isCommonDb));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error resolving ICacheInfoProvider");
            }

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using DealCloud.Common.Interfaces;

namespace DealCloud.Common.Cache
{
    /// <summary>
    ///     Relolves cache versions using persistent storage.
    /// </summary>
    [Serializable]
    public class VersionHelper : ICacheVersionHelper<int>
    {
        private readonly string _cacheName;

        /// <summary>
        ///     Info Provider - DB, WebServices, etc
        /// </summary>
        private readonly ICacheInfoProvider _infoProvider;

        /// <summary>
        ///     VersionPolicy from config
        /// </summary>
        private readonly VersionPolicy _retrievalPolicy;

        /// <summary>
        ///     Default timeout for versions
        /// </summary>
        private readonly TimeSpan _timeout;

        /// <summary>
        ///     TimeoutReference for versions expitarion if mode is VersionPolicy.Timeout
        /// </summary>
        private TimeoutReference<int> _timeoutRef;

        /// <summary>
        ///     Relolves cache versions using persistent storage
        /// </summary>
        /// <param name="name">Name of the cache (region)</param>
        /// <param name="infoProvider"> </param>
        public VersionHelper(string name, ICacheInfoProvider infoProvider)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name) + " should not be null or empty");
            if (infoProvider == null) throw new ArgumentException(nameof(infoProvider) + " should not be null");

            var config = ConfigurationManager.GetSection("cacheSettings") as NameValueCollection ?? new NameValueCollection();

            if (!Enum.TryParse(config["VersionPolicy"], out _retrievalPolicy)) _retrievalPolicy = VersionPolicy.Once; //NOTE: default to Once like in running job in a thread

            if (_retrievalPolicy == VersionPolicy.Timeout) _timeout = TimeSpan.FromSeconds(Convert.ToInt32(config["VersionTimeout"]));

            _cacheName = name;
            _infoProvider = infoProvider;
        }

        public long GetVersion(int key)
        {
            var keyCode = key.GetHashCode();
            var cacheVersions = LoadVersions(keyCode);
            long version;

            version = cacheVersions != null && cacheVersions.TryGetValue(keyCode, out version) ? version : 0;

            return version;
        }

        public void SetVersion(int key, long version, string[] tags)
        {
            var keyCode = key.GetHashCode();
            var cacheVersions = LoadVersions(keyCode);
            long currVersion;

            if (cacheVersions != null && cacheVersions.TryGetValue(keyCode, out currVersion))
            {
                if (currVersion >= version) return;
            }

            _infoProvider.SaveVersion(_cacheName, keyCode, version, tags);
        }

        public bool RequireUpdate(int key, long version, out bool needreset)
        {
            needreset = false;
            var keyCode = key.GetHashCode();
            var cacheVersions = LoadVersions(keyCode);
            long currentVersion;
            bool result = true;

            if (cacheVersions != null && cacheVersions.TryGetValue(keyCode, out currentVersion))
            {
                needreset = currentVersion - version > Constants.DIFFERNCE_FOR_CACHE_RESET;

                result = currentVersion != version;
            }

            return result;
        }

        public void IncrementVersion(IEnumerable<string> tags)
        {
            _infoProvider.InvalidateVersions(_cacheName, tags);
        }

        public void Clear(bool increment = false)
        {
            if (increment)
            {
                _infoProvider.InvalidateVersions(_cacheName, null);
            }
            else
            {
                _infoProvider.Clear(_cacheName);
            }
        }

        /// <summary>
        ///     Returns all cache versions
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, long> GetVersions()
        {
            var cacheVersions = LoadVersions(-1);

            return cacheVersions != null ? new Dictionary<int, long>(cacheVersions) : null;
        }

        private Dictionary<int, long> LoadVersions(int key)
        {
            var loadPerCall = _retrievalPolicy == VersionPolicy.PerCall;
            var isTimedout = (_retrievalPolicy == VersionPolicy.Timeout) && ((_timeoutRef == null) || (!_timeoutRef.IsAlive));
            var cacheVersions = _infoProvider.LoadVersions(_cacheName, loadPerCall ? (int?) (key) : null, loadPerCall || isTimedout);

            if (isTimedout) _timeoutRef = new TimeoutReference<int>(1, _timeout, false);

            return cacheVersions;
        }

        [Serializable]
        internal enum VersionPolicy
        {
            /// <summary>
            ///     Load from persistent storage once and forget
            /// </summary>
            Once,

            /// <summary>
            ///     Refresh versions by timeout
            /// </summary>
            Timeout,

            /// <summary>
            ///     Refresh versions for each call to cache
            /// </summary>
            PerCall,

            /// <summary>
            ///     Refresh per http request or wcf call, etc
            /// </summary>
            PerTransaction
        }
    }
}
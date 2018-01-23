using DealCloud.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using NLog;
using System.Security.Claims;
using System.Threading;

namespace DealCloud.AddIn.Common.Utils
{
    public class AddinCacheInfoProvider : ICacheInfoProvider
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<object, object> _items = new ConcurrentDictionary<object, object>();

        public AddinCacheInfoProvider(bool isCommonDb = false)
        {
        }

        public IDictionary Items
        {
            get { return _items; }
        }


        public void Clear(string name)
        {
            _items[name] = null;
        }

        public int GetClientId()
        {
            return 0;
        }

        public void InvalidateVersions(string name, IEnumerable<string> tags)
        {
            _items[name] = null;
        }

        public Dictionary<int, long> LoadVersions(string name, int? key, bool forceReload)
        {
            var _cacheVersions = GetLocalCacheVersions(name);
            if (_cacheVersions == null || forceReload)
            {
                var dp = GetDataProvider();
                if (dp != null)
                {
                    try
                    {
                        _cacheVersions = dp.GetCacheVersionsCall();
                        SetLocalCacheVersions(name, _cacheVersions);
                    }
                    catch (Exception ex)
                    {
                        if (_log.IsDebugEnabled) _log.Debug(ex, "can't get cache versions");
                    }
                }
            }
            return _cacheVersions;
        }

        private DataProviderBase GetDataProvider()
        {
            return DataProviderBase.GetInstance();
        }

        protected virtual void SetLocalCacheVersions(string name, Dictionary<int, long> _cacheVersions)
        {
            _items[name] = _cacheVersions;
        }


        protected virtual Dictionary<int, long> GetLocalCacheVersions(string name)
        {
            object obj;
            if (_items.TryGetValue(name, out obj))
                return obj as Dictionary<int, long>;

            return null;
        }

        public void SaveVersion(string name, int keyCode, long version, string[] tags)
        {
            var dataProvider = GetDataProvider();
            if (dataProvider != null)
            {
                try
                {
                    dataProvider.SetCacheVersionCall(keyCode, version, tags);
                }
                catch (Exception ex)
                {
                    if (_log.IsDebugEnabled) _log.Debug(ex, "can't set cache version");
                }
            }
            var _cacheVersions = GetLocalCacheVersions(name);
            if (_cacheVersions != null) _cacheVersions[keyCode] = version;
        }

        public ClaimsIdentity GetIdentity()
        {
            return Thread.CurrentPrincipal.Identity as ClaimsIdentity;
        }

        public void BeginAsyncCallProcessing(ClaimsIdentity identity, IDictionary items)
        {
        }

        public void EndAsyncCallProcessing()
        {
        }
    }
}

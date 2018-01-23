using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace DealCloud.Common.Interfaces
{
    /// <summary>
    ///     Client and version provider for cache
    /// </summary>
    public interface ICacheInfoProvider
    {
        IDictionary Items { get; }

        int GetClientId();

        Dictionary<int, long> LoadVersions(string name, int? key, bool forceReload);

        void SaveVersion(string name, int keyCode, long version, string[] tags);

        void InvalidateVersions(string name, IEnumerable<string> tags);

        void Clear(string name);

        ClaimsIdentity GetIdentity();

        void BeginAsyncCallProcessing(ClaimsIdentity identity, IDictionary items);

        void EndAsyncCallProcessing();
    }
}
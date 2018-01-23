using System.Collections.Generic;

namespace DealCloud.Common.Interfaces
{
    /// <summary>
    ///     Resolve versions in the cache
    /// </summary>
    public interface ICacheVersionHelper<TKey>
    {
        long GetVersion(TKey key);

        void SetVersion(TKey key, long version, string[] tags);

        bool RequireUpdate(TKey key, long version, out bool needreset);

        void IncrementVersion(IEnumerable<string> tags);

        void Clear(bool increment = false);

        Dictionary<TKey, long> GetVersions();
    }
}
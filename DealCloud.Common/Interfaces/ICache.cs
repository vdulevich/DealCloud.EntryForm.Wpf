namespace DealCloud.Common.Interfaces
{
    /// <summary>
    ///     Interface for local cache provider
    /// </summary>
    public interface ICache<TKey>
    {
        /// <summary>
        ///     Returns object by Key
        /// </summary>
        object this[TKey key] { get; set; }

        /// <summary>
        ///     Get all keys in local cache
        /// </summary>
        TKey[] Keys { get; }

        /// <summary>
        ///     Add object to local cache
        /// </summary>
        void Add(TKey key, object value);

        /// <summary>
        ///     Add object to local cache with tags
        /// </summary>
        void Add(TKey key, object value, string[] tags);

        /// <summary>
        ///     Remove object from local cache
        /// </summary>
        /// <typeparam name="TRemove">Type of object to remove</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Removed object</returns>
        TRemove Remove<TRemove>(TKey key);

        /// <summary>
        ///     Removes all objects by tags
        /// </summary>
        /// <param name="tags">Tags by which objects should be removed</param>
        void RemoveByTags(string[] tags);

        /// <summary>
        ///     Clear local cache
        /// </summary>
        void Clear();

        /// <summary>
        ///     Return object by Key and cast it to specified type
        /// </summary>
        /// <typeparam name="TCastTo">Type that retrieved object will be casted to</typeparam>
        /// <param name="Key">Key</param>
        /// <returns></returns>
        TCastTo Get<TCastTo>(TKey Key);
    }
}
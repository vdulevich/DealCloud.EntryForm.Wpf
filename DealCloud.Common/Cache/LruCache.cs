using System;
using System.Collections.Generic;
using System.Linq;
using DealCloud.Common.Interfaces;

namespace DealCloud.Common.Cache
{
    /// <summary>
    ///     Least-recently-used cache
    /// </summary>
    [Serializable]
    public class LruCache<TKey> : ICache<TKey>
    {
        /// <summary>
        ///     Empty number in containers list, terminator for head and tail
        /// </summary>
        private const int EMPTY = -1;

        /// <summary>
        ///     Cache capacity
        /// </summary>
        private readonly int _capacity;

        private Container[] _containers;

        private int _head = EMPTY;

        private Dictionary<TKey, Container> _keyContainerTable;

        private Dictionary<string, HashSet<TKey>> _tagsToKeys;

        private int _tail = EMPTY;

        /// <summary>
        ///     Creates cache for specified capacity
        /// </summary>
        /// <param name="capacity">
        ///     Cache capacity
        /// </param>
        public LruCache(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Cache size should be positive.");

            _keyContainerTable = new Dictionary<TKey, Container>(capacity);
            _tagsToKeys = new Dictionary<string, HashSet<TKey>>();
            _capacity = capacity;

            InitContainers(capacity);
        }

        /// <summary>
        ///     Adds Key, Value to the cache
        /// </summary>
        public void Add(TKey key, object value)
        {
            Add(key, value, null);
        }

        /// <summary>
        ///     Adds value to the cache and associate tags to it
        /// </summary>
        public void Add(TKey key, object value, string[] tags)
        {
            if (_capacity == 0) return;
            if (value == null) return; //TODO: may be should throw exception

            string[] oldTags;
            TKey oldKey;

            lock (_keyContainerTable)
            {
                Container container;

                if (_keyContainerTable.ContainsKey(key))
                {
                    container = MoveToHead(_keyContainerTable[key]);
                }
                else
                {
                    container = MoveToHead(_containers[_tail]);

                    if (container.Key != null)
                    {
                        _keyContainerTable.Remove(container.Key);
                    }
                }

                oldTags = container.Tags;
                oldKey = container.Key;

                container.Key = key;
                container.Value = value;
                container.Tags = tags;

                _keyContainerTable[key] = container;
            }

            lock (_tagsToKeys)
            {
                ClearTagKeys(oldKey, oldTags); //NOTE: remove tags-keys accociation

                AddTagKeys(key, tags);
            }
        }

        /// <summary>
        ///     Removes cache entries by tags
        /// </summary>
        /// <param name="tags">Array of tags by which cche entries will be removed</param>
        public void RemoveByTags(string[] tags)
        {
            if (tags == null || tags.Length == 0) throw new ArgumentException("At least 1 tag should be provided.");

            var keys = new List<TKey>();

            foreach (var tag in tags)
            {
                lock (_tagsToKeys)
                {
                    if (_tagsToKeys.ContainsKey(tag))
                    {
                        keys.AddRange(_tagsToKeys[tag]);
                    }

                    _tagsToKeys.Remove(tag); //NOTE: remove tag-keys association
                }
            }

            lock (_keyContainerTable)
            {
                foreach (var key in keys)
                {
                    if (_keyContainerTable.ContainsKey(key))
                    {
                        var container = MoveToTail(_keyContainerTable[key]);

                        _keyContainerTable.Remove(key);

                        container.Key = default(TKey);
                        container.Value = null;
                        container.Tags = null;
                    }
                }
            }
        }

        /// <summary>
        ///     Removes value for the Key
        /// </summary>
        public TValue Remove<TValue>(TKey key)
        {
            if (_capacity == 0) return default(TValue);

            var result = default(TValue);

            lock (_keyContainerTable)
            {
                if (_keyContainerTable.ContainsKey(key))
                {
                    var container = MoveToTail(_keyContainerTable[key]);

                    _keyContainerTable.Remove(key);

                    container.Key = default(TKey);

                    result = (TValue) container.Value;

                    container.Value = default(TValue);
                }
            }

            return result;
        }

        /// <summary>
        ///     Read/Write operations on cache. If any tags was assigned to cache they won't change
        /// </summary>
        public object this[TKey key]
        {
            get
            {
                lock (_keyContainerTable)
                {
                    if (_capacity == 0) throw new ArgumentOutOfRangeException(nameof(key), key, "MsgCacheEmpty");
                    if (!_keyContainerTable.ContainsKey(key)) return null;

                    var container = MoveToHead(_keyContainerTable[key]);

                    return container.Value;
                }
            }
            set
            {
                if (_capacity == 0) return;

                Container container;
                var clear = false;

                lock (_keyContainerTable)
                {
                    if (_keyContainerTable.ContainsKey(key))
                    {
                        container = MoveToHead(_keyContainerTable[key]);
                    }
                    else
                    {
                        container = MoveToHead(_containers[_tail]);

                        if (container.Key != null)
                        {
                            if (_keyContainerTable.ContainsKey(container.Key))
                            {
                                _keyContainerTable.Remove(container.Key);

                                clear = true;
                            }
                        }

                        _keyContainerTable.Add(key, container);
                    }

                    container.Key = key;
                    container.Value = value;
                }

                if (clear)
                {
                    lock (_tagsToKeys)
                    {
                        ClearTagKeys(container.Key, container.Tags); //NOTE: remove tags-key accociation
                    }
                }
            }
        }

        /// <summary>
        ///     Gets Value from the cache converted to type
        /// </summary>
        public TValue Get<TValue>(TKey Key)
        {
            var item = this[Key];

            var result = (item == null) ? default(TValue) : (TValue)item;

            return result;
        }

        public TKey[] Keys
        {
            get
            {
                lock (_keyContainerTable)
                {
                    return _keyContainerTable.Keys.ToArray();
                }
            }
        }

        /// <summary>
        ///     Clears internal caches
        /// </summary>
        public void Clear()
        {
            lock (_keyContainerTable)
            {
                _keyContainerTable = new Dictionary<TKey, Container>(_capacity);

                InitContainers(_capacity);
            }

            lock (_tagsToKeys)
            {
                _tagsToKeys = new Dictionary<string, HashSet<TKey>>();
            }
        }

        private void AddTagKeys(TKey key, IEnumerable<string> tags)
        {
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    if (_tagsToKeys.ContainsKey(tag))
                    {
                        _tagsToKeys[tag].Add(key);
                    }
                    else
                    {
                        var hs = new HashSet<TKey>();

                        _tagsToKeys[tag] = hs;

                        hs.Add(key);
                    }
                }
            }
        }

        private void ClearTagKeys(TKey key, IEnumerable<string> tags)
        {
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    if (_tagsToKeys.ContainsKey(tag))
                    {
                        var hs = _tagsToKeys[tag];

                        if (hs.Count > 1)
                        {
                            hs.Remove(key);
                        }
                        else
                        {
                            _tagsToKeys.Remove(tag);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Initialize containers for future use
        /// </summary>
        /// <param name="capacity">
        ///     Capacity of cache
        /// </param>
        private void InitContainers(int capacity)
        {
            if (capacity == 0) return;

            _containers = new Container[capacity];

            for (var i = 0; i < capacity; i++)
            {
                _containers[i] = new Container(i - 1, i, i + 1);
            }

            _head = 0;
            _tail = capacity - 1;

            _containers[_head].Prev = EMPTY;
            _containers[_tail].Next = EMPTY;
        }

        private Container MoveToHead(Container container)
        {
            if (container.Index == _head)
            {
                return container;
            }

            if (container.Index == _tail)
            {
                _containers[container.Prev].Next = EMPTY;

                _tail = container.Prev;

                container.Prev = EMPTY;
                container.Next = _head;

                _containers[_head].Prev = container.Index;

                _head = container.Index;

                return container;
            }

            _containers[container.Prev].Next = container.Next;
            _containers[container.Next].Prev = container.Prev;

            container.Prev = EMPTY;
            container.Next = _head;

            _containers[_head].Prev = container.Index;

            _head = container.Index;

            return container;
        }

        private Container MoveToTail(Container container)
        {
            if (container.Index == _tail)
            {
                return container;
            }

            if (container.Index == _head)
            {
                _containers[container.Next].Prev = EMPTY;

                _head = container.Next;

                container.Next = EMPTY;
                container.Prev = _tail;

                _containers[_tail].Next = container.Index;

                _tail = container.Index;

                return container;
            }

            _containers[container.Prev].Next = container.Next;
            _containers[container.Next].Prev = container.Prev;

            container.Next = EMPTY;
            container.Prev = _tail;

            _containers[_tail].Next = container.Index;

            _tail = container.Index;

            return container;
        }

        #region private class Container;

        /// <summary>
        ///     Key, Value pair in double linked list
        /// </summary>
        [Serializable]
        private class Container
        {
            /// <summary>
            ///     Index of this container in array of containers
            /// </summary>
            public readonly int Index;

            /// <summary>
            ///     Cache key
            /// </summary>
            public TKey Key;

            /// <summary>
            ///     Index of Next container in the array
            /// </summary>
            public int Next;

            /// <summary>
            ///     Index of previous container in the array
            /// </summary>
            public int Prev;

            /// <summary>
            ///     Cache value
            /// </summary>
            public string[] Tags;

            /// <summary>
            ///     Cache value
            /// </summary>
            public object Value;

            /// <summary>
            ///     Creates container and initializes it
            /// </summary>
            /// <param name="prev">
            ///     Previous container
            /// </param>
            /// <param name="index">
            ///     Index of this container
            /// </param>
            /// <param name="next">
            ///     Index of next container
            /// </param>
            public Container(int prev, int index, int next)
            {
                Key = default(TKey);
                Value = null;
                Prev = prev;
                Index = index;
                Next = next;
            }
        }

        #endregion
    }
}
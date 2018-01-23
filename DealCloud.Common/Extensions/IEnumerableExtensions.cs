using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Paged<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            return source
              .Skip((page - 1) * pageSize)
              .Take(pageSize);
        }

        public static IEnumerable<List<T>> Partition<T>(this IEnumerable<T> sequence, int size)
        {
            List<T> partition = new List<T>(size);
            foreach (var item in sequence)
            {
                partition.Add(item);
                if (partition.Count == size)
                {
                    yield return partition;
                    partition = new List<T>(size);
                }
            }
            if (partition.Count > 0)
                yield return partition;
        }

        public static IEnumerable<object> AsEnumerable(this IList src)
        {
            foreach (var x in src)
                yield return x; 
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return null;
            var ret = new HashSet<T>();
            foreach (var x in source)
                ret.Add(x);
            return ret;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var ret = new HashSet<T>(comparer);
            foreach (var x in source)
                ret.Add(x);
            return ret;
        }

        public static Dictionary<K, T> ToDictionarySafe<K, T>(this IEnumerable<T> src, Func<T,K> keySelector)
        {
            return ToDictionarySafe(src, keySelector, null as IEqualityComparer<K>);
        }

        public static Dictionary<K, T> ToDictionarySafe<K, T>(this IEnumerable<T> src, Func<T, K> keySelector, IEqualityComparer<K> comparer)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (src == null) return null;
            var ret = comparer != null ? new Dictionary<K, T>(comparer) : new Dictionary<K, T>();
            foreach (var t in src)
            {
                ret[keySelector(t)] = t;
            }
            return ret;
        }

        public static Dictionary<K, T> ToDictionarySafe<S, K, T>(this IEnumerable<S> src, Func<S, K> keySelector, Func<S, T> valueSelector)
        {
            return ToDictionarySafe(src, keySelector, valueSelector, null as IEqualityComparer<K>);
        }
        public static Dictionary<K,T> ToDictionarySafe<S,K,T>(this IEnumerable<S> src, Func<S, K> keySelector, Func<S,T> valueSelector, IEqualityComparer<K> comparer)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));
            if (src == null) return null;
            var ret = comparer != null ? new Dictionary<K, T>(comparer) : new Dictionary<K, T>();
            foreach (var t in src)
            {
                ret[keySelector(t)] = valueSelector(t);
            }
            return ret;
        }

        public static IEnumerable<object>ToEnumerable(this IList list)
        {
            foreach (var o in list)
                yield return o;
        }


        public static int UncheckedSum(this IEnumerable<int> source)
        {
            return source.Aggregate((sum, i) => unchecked(sum + i));
        }

        public static bool ContainsAll<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            if (items == null || !items.Any()) return false;

            foreach (var item in items)
            {
                if (!set.Contains(item)) return false;
            }
            return true;
        }

        public static bool Equals<T>(IList<T> x, IList<T> y)
        {
            if ((x != null && y == null) || (x == null && y != null)) return false;

            if (y != null)
            {
                if (x.Count != y.Count) return false;
                for (int i = 0; i < y.Count; i++)
                {
                    var l = x[i];
                    var r = y[i];
                    if (l is IList || r is IList) return Equals2(l as IList, r as IList);
                    if (!object.Equals(l, r)) return false;
                }
            }
            return true;
        }

        public static bool Equals2(IList x, IList y)
        {
            if ((x != null && y == null) || (x == null && y != null)) return false;

            if (y != null)
            {
                if (x.Count != y.Count) return false;
                for (int i = 0; i < y.Count; i++)
                {
                    var l = x[i];
                    var r = y[i];
                    if (l is IList || r is IList) return Equals2(l as IList, r as IList);
                    if (!object.Equals(l, r)) return false;
                }
            }
            return true;
        }

        public static bool Equals<T>(T[] x, T[] y)
        {
            if ((x != null && y == null) || (x == null && y != null)) return false;

            if (y != null)
            {
                if (x.Length != y.Length) return false;
                for (int i = 0; i < y.Length; i++)
                {
                    var l = x[i];
                    var r = y[i];
                    if (l is IList || r is IList) return Equals2(l as IList, r as IList);
                    if (!object.Equals(l, r)) return false;
                }
            }
            return true;
        }

        public static int GetHashCodeForList<T>(this IEnumerable<T> items)
        {
            int result = 17;
            unchecked
            {
                if (items != null)
                {
                    foreach (var row in items)
                    {
                        var hc = row is IList ? (GetHashCodeForList(row as IList)) : (row != null ? row.GetHashCode() : 0);
                        result = 37 * result + hc;
                    }
                }
            }
            return result;
        }

        public static int GetHashCodeForList(this IList items)
        {
            int result = 17;
            unchecked
            {
                if (items != null)
                {
                    foreach (var row in items)
                    {
                        var hc = row is IList ? (GetHashCodeForList(row as IList)) : (row != null ? row.GetHashCode() : 0);
                        result = 37 * result + hc;
                    }
                }
            }
            return result;
        }

        public static void AddRange<K, T>(this IDictionary<K, T> dst, IEnumerable<T> src, Func<T, K> keySelector)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (src == null || !src.Any()) return;

            foreach (var item in src)
                dst[keySelector(item)] = item;
        }

    }
}

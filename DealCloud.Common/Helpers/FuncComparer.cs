using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Helpers
{

    public class FuncIComparer<T> : IComparer<T>
    {
        private Func<T, T, int> _compareFunction;
        public FuncIComparer(Func<T, T, int> compareFunction)
        {
            if (compareFunction == null) throw new ArgumentNullException("compareFunction");
            _compareFunction = compareFunction;
        }

        public int Compare(T x, T y)
        {
            return _compareFunction(x, y);
        }
    }

    public class FuncEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _compareFunction;
        private readonly Func<T, int> _hashFunction;

        public FuncEqualityComparer(Func<T, T, bool> compareFunction)
        {
            _compareFunction = compareFunction;
            _hashFunction = t => t.GetHashCode();
        }
        public FuncEqualityComparer(Func<T, T, bool> compareFunction, Func<T, int> hashFunction)
        {
            _compareFunction = compareFunction;
            _hashFunction = hashFunction;
        }

        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return _compareFunction(x, y);
        }

        public int GetHashCode(T obj)
        {
            if (ReferenceEquals(obj, null))
                return 0;

            return _hashFunction(obj);
        }
    }



}

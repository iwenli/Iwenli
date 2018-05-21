using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 方法1
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;
        private IEqualityComparer<V> comparer;

        public CommonEqualityComparer(Func<T, V> keySelector, IEqualityComparer<V> comparer = null)
        {
            this.keySelector = keySelector;
            this.comparer = comparer ?? EqualityComparer<V>.Default;
        }

        public bool Equals(T x, T y)
        {
            return comparer.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return comparer.GetHashCode(keySelector(obj));
        }

    }
    /// <summary>
    /// 方法2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PredicateEqualityComparer<T> : EqualityComparer<T>
    {
        private Func<T, T, bool> predicate;

        public PredicateEqualityComparer(Func<T, T, bool> predicate) : base()
        {
            this.predicate = predicate;
        }

        public override bool Equals(T x, T y)
        {
            if (x != null)
            {
                return ((y != null) && this.predicate(x, y));
            }

            if (y != null)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode(T obj)
        {
            //始终返回相同的值以强制调用IEqualityComparer<T>.Equals
            return 0;
        }
    }
}

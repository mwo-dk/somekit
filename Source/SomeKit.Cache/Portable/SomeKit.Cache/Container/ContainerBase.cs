using System.Collections;
using System.Collections.Generic;

namespace SomeKit.Cache.Container
{
    public abstract class ContainerBase<T> :
        IContainer<T>
    {
        public abstract int Size { get; }

        public abstract bool TryGet(int position, out T value);

        public abstract void Set(int position, T value);

        public abstract bool TryRemove(int position);
        public abstract void Clear();

        public IEnumerator<T> GetEnumerator()
        {
            for (var n = 0; n < Size; ++n)
            {
                T value;
                TryGet(n, out value);
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

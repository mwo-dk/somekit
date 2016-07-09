using System.Collections.Generic;

namespace SomeKit.Cache
{
    public interface IContainer<T> :
        IEnumerable<T>
    {
        int Size { get; }
        bool TryGet(int position, out T value);
        void Set(int position, T value);
        bool TryRemove(int position);
        void Clear();
    }
}

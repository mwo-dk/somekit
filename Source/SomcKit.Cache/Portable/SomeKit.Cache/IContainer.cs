using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

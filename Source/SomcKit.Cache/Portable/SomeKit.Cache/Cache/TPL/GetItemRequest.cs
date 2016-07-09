using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class GetItemRequest<T> : RequestBase<T>
    {
        internal GetItemRequest(Record<T>[] source, int key)
        {
            Source = source;
            Key = key;
        }
        internal Record<T>[] Source { get; }
        internal int Key { get; }
    }
}

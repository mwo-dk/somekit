using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class DeleteItemRequest<T> : RequestBase<T>
    {
        internal DeleteItemRequest(Record<T>[] source, int key)
        {
            Source = source;
            Key = key;
        }
        internal Record<T>[] Source { get; }
        internal int Key { get; }
    }
}

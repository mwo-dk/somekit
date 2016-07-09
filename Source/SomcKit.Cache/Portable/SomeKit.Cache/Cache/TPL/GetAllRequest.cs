using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class GetAllRequest<T> : RequestBase<T>
    {
        internal GetAllRequest(Record<T>[] source)
        {
            Source = source;
        }
        internal Record<T>[] Source { get; }
    }
}

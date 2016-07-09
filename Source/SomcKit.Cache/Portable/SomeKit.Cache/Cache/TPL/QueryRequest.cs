using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class QueryRequest<T> : RequestBase<T>
    {
        internal QueryRequest(Record<T>[] source, Predicate<T> filter)
        {
            Source = source;
            Filter = filter;
        }
        internal Record<T>[] Source { get; }
        internal Predicate<T> Filter { get; }
    }
}

using System;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class QueryRequest<T> : RequestBase
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

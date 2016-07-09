using System;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class ModifyRequest<T> : RequestBase
    {
        internal ModifyRequest(Record<T>[] source, Action<T> handler, Predicate<T> filter)
        {
            Source = source;
            Handler = handler;
            Filter = filter;
        }

        internal Record<T>[] Source { get; }
        internal Action<T> Handler { get; }
        internal Predicate<T> Filter { get; }
    }
}

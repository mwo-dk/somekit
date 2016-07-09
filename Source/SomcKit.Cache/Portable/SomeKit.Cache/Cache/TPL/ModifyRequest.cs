using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class ModifyRequest<T> : RequestBase<T>
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

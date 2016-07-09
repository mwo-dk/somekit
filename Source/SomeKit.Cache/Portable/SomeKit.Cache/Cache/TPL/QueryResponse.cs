using System;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class QueryResponse<T> : ResponseBase
    {
        internal QueryResponse(T[] result)
        {
            Result = result;
            Error = null;
        }

        internal QueryResponse(Exception error)
        {
            Result = null;
            Error = error;
        }
        internal T[] Result { get; }
    }
}

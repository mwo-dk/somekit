using System;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class GetItemResponse<T> : ResponseBase
    {
        internal GetItemResponse(T result)
        {
            Result = result;
            Error = null;
        }

        internal GetItemResponse(Exception error)
        {
            Result = default(T);
            Error = error;
        }
        internal T Result { get; }
    }
}

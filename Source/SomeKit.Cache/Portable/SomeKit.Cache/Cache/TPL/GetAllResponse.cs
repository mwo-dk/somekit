using System;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class GetAllResponse<T> : ResponseBase
    {
        internal GetAllResponse(T[] result)
        {
            Result = result;
            Error = null;
        }

        internal GetAllResponse(Exception error)
        {
            Result = null;
            Error = error;
        }
        internal T[] Result { get; }
    }
}

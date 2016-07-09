using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class GetItemResponse<T> : ResponseBase<T>
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

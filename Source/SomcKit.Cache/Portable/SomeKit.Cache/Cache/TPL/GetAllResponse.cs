using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class GetAllResponse<T> : ResponseBase<T>
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

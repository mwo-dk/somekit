using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class QueryResponse<T> : ResponseBase<T>
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

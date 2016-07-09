using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class ModifyResponse<T> : ResponseBase<T>
    {
        internal ModifyResponse(Exception error = null)
        {
            Error = error;
        }
    }
}

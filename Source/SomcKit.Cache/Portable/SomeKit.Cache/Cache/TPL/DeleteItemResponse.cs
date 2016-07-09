using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class DeleteItemResponse<T> : ResponseBase<T>
    {
        internal DeleteItemResponse(Exception error = null)
        {
            Error = error;
        }
    }
}

using System;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class DeleteItemResponse : ResponseBase
    {
        internal DeleteItemResponse(Exception error = null)
        {
            Error = error;
        }
    }
}

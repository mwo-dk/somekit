using System;

namespace SomeKit.Cache.Cache.TPL
{
    internal sealed class ModifyResponse : ResponseBase
    {
        internal ModifyResponse(Exception error = null)
        {
            Error = error;
        }
    }
}

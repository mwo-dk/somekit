using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache.Cache.TPL
{
    internal abstract class ResponseBase<T>
    {
        internal Exception Error { get; set; }
    }
}

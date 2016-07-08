using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeKit
{
    /// <summary>
    /// Represents a collection of <see cref="IHttpRequestHeader"/>s
    /// </summary>
    public interface IHttpRequestHeaderCollection : ICollection<IHttpRequestHeader>
    {
    }
}

using System.Collections.Generic;

namespace SomeKit.REST
{
    /// <summary>
    ///     Represents a collection of <see cref="IHttpRequestHeader" />s
    /// </summary>
    public interface IHttpRequestHeaderCollection : ICollection<IHttpRequestHeader>
    {
    }
}
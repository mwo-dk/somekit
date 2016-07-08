using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeKit
{
    /// <summary>
    /// Represents an HTTP request header.
    /// </summary>
    public interface IHttpRequestHeader
    {
        /// <summary>
        /// The name of the header
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// The value of the header
        /// </summary>
        string Value { get; set; }
    }
}

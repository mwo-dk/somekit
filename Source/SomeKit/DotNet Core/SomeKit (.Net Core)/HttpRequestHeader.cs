using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeKit
{
    /// <inheritdoc/>
    public sealed class HttpRequestHeader : IHttpRequestHeader
    {
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string Value { get; set; }
    }
}

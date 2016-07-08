using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SomeKit.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="HttpRequestMessage"/>
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Extension method that adds an HTTP request header of name <paramref name="name"/> and name <paramref name="value"/>
        /// to <paramref name="request"/>
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/> to which the HTTP request header should be added</param>
        /// <param name="name">The name of the HTTP request header</param>
        /// <param name="value">The value of the HTTP request header</param>
        /// <returns></returns>
        public static HttpRequestMessage WithHeader(this HttpRequestMessage request,
            string name,
            string value)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            request.Headers.Add(name, value);

            return request;
        }
    }
}

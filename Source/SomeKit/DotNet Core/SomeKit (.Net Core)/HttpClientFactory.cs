using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SomeKit
{
    /// <summary>
    /// Implements <see cref="IHttpClientFactory"/>
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        ///<inheritdoc/>
        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SomeKit
{
    /// <summary>
    /// Implements <see cref="IHttpClientFactory"/>. Creates <see cref="HttpClient"/>s, that use Windows Authentication
    /// </summary>
    public class HttpWindowsClientFactory
    {
        ///<inheritdoc/>
        public HttpClient Create()
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.UseDefaultCredentials = true;
            return new HttpClient(clientHandler);
        }
    }
}

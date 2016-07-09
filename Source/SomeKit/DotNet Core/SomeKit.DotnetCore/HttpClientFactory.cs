using System.Net.Http;

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

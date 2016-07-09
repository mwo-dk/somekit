using System.Net.Http;

namespace SomeKit.REST
{
    /// <summary>
    ///     Implements <see cref="IHttpClientFactory" />
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        /// <inheritdoc />
        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}
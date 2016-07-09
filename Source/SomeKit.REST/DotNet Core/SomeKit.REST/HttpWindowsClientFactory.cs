using System.Net.Http;

namespace SomeKit.REST
{
    /// <summary>
    /// Implements <see cref="IHttpClientFactory"/>. Creates <see cref="HttpClient"/>s, that use Windows Authentication
    /// </summary>
    public class HttpWindowsClientFactory : IHttpClientFactory
    {
        ///<inheritdoc/>
        public HttpClient Create()
        {
            var clientHandler = new HttpClientHandler {UseDefaultCredentials = true};
            return new HttpClient(clientHandler);
        }
    }
}

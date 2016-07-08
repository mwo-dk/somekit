using System.Net.Http;

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
            var clientHandler = new HttpClientHandler {UseDefaultCredentials = true};
            return new HttpClient(clientHandler);
        }
    }
}

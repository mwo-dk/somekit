using System.Net.Http;
using System.Net.Http.Headers;

namespace SomeKit.REST
{
    /// <summary>
    ///     Implements <see cref="IHttpClientFactory" />
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly AuthenticationHeaderValue _authenticationHeader;
        private readonly MediaTypeWithQualityHeaderValue _mediaTypeHeaderValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientFactory"/> class.
        /// </summary>
        /// <param name="authenticationHeader">The authentication header.</param>
        /// <param name="mediaTypeHeaderValue">The media type header value.</param>
        public HttpClientFactory(AuthenticationHeaderValue authenticationHeader=null,
            MediaTypeWithQualityHeaderValue mediaTypeHeaderValue =null)
        {
            _authenticationHeader = authenticationHeader;
            _mediaTypeHeaderValue = mediaTypeHeaderValue;
        }

        /// <inheritdoc />
        public HttpClient Create()
        {
            var client = new HttpClient();
            if (_authenticationHeader != null)
            {
                client.DefaultRequestHeaders.Authorization = _authenticationHeader;
            }
            if (_mediaTypeHeaderValue != null)
            {
                client.DefaultRequestHeaders.Accept.Add(_mediaTypeHeaderValue);
            }
            return client;
        }
    }
}
using System.Net.Http;

namespace SomeKit.REST
{
    /// <summary>
    ///     Factory for creating <see cref="HttpClient" />s.
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        ///     Creates a new instance of a <see cref="HttpClient" />
        /// </summary>
        /// <returns>An <see cref="HttpClient" /></returns>
        HttpClient Create();
    }
}
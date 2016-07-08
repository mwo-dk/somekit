using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SomeKit.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="HttpClient"/>
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Invokes correct version of SendAsync on <paramref name="client"/> depending on the combination of parameters
        /// <paramref name="cancellationToken"/> and <paramref name="httpCompletionOption"/>
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> to utilize</param>
        /// <param name="requestMessage">The <see cref="HttpRequestMessage"/> to send</param>
        /// <param name="httpCompletionOption">The optional <see cref="HttpCompletionOption"/></param>
        /// <param name="cancellationToken">The optional <see cref="CancellationToken"/></param>
        /// <returns>The resulting <see cref="HttpResponseMessage"/></returns>
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient client,
            HttpRequestMessage requestMessage,
            HttpCompletionOption? httpCompletionOption,
            CancellationToken? cancellationToken)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            if (httpCompletionOption.HasValue)
            {
                if (cancellationToken.HasValue)
                    return await client.SendAsync(requestMessage, httpCompletionOption.Value, cancellationToken.Value)
                        .ConfigureAwait(false);
                else
                    return await client.SendAsync(requestMessage, httpCompletionOption.Value)
                        .ConfigureAwait(false);
            }
            else
            {
                if (cancellationToken.HasValue)
                    return await client.SendAsync(requestMessage, cancellationToken.Value)
                        .ConfigureAwait(false);
                else
                    return await client.SendAsync(requestMessage)
                        .ConfigureAwait(false);
            }
        }
    }
}

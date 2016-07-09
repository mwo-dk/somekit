using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SomeKit.REST.Extensions;
using SomeKit.REST.Serialization.Json;

namespace SomeKit.REST
{
    /// <summary>
    ///     Provides implementatin of <see cref="IRestClient" />
    /// </summary>
    public class RestClient : IRestClient
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory" /> utilized to create <see cref="HttpClient" />s</param>
        /// <param name="payloadSerializer">The <see cref="IPayloadSerializer" /> used to serialize requests</param>
        /// <param name="payloadDeserializer">The <see cref="IPayloadDeserializer" /> used to deserialize requests</param>
        public RestClient(IHttpClientFactory httpClientFactory,
            IPayloadSerializer payloadSerializer,
            IPayloadDeserializer payloadDeserializer)
        {
            HttpClientFactory = httpClientFactory ?? new HttpClientFactory();
            PayloadSerializer = payloadSerializer ?? new JsonPayloadSerializer();
            PayloadDeserializer = payloadDeserializer ?? new JsonPayloadDeserializer();
            RequestHeaders = new HttpRequestHeaderCollection();
        }

        /// <inheritdoc />
        public Uri BaseAddress { get; set; }

        /// <inheritdoc />
        public IHttpRequestHeaderCollection RequestHeaders { get; }

        #region Get

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return await InvokeAsync<T>(relativeAddress, HttpMethod.Get, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public async Task DeleteAsync(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            await InvokeAsync(relativeAddress, HttpMethod.Delete, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }

        #endregion

        #region Support services

        /// <inheritdoc />
        public IHttpClientFactory HttpClientFactory { get; set; }

        /// <inheritdoc />
        public IPayloadSerializer PayloadSerializer { get; set; }

        /// <inheritdoc />
        public IPayloadDeserializer PayloadDeserializer { get; set; }

        #endregion

        #region Post

        /// <inheritdoc />
        public async Task<TResponse> PostAsync<TRequest, TResponse>(Uri relativeAddress,
            TRequest request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return
                await
                    InvokeAsync<TRequest, TResponse>(relativeAddress, HttpMethod.Post, request, errorHandler,
                        httpCompletionOption,
                        cancellationToken, cancellationHandler).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResponse> PostAsync<TResponse>(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return await InvokeAsync<TResponse>(relativeAddress, HttpMethod.Post, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task PostAsync<TRequest>(Uri relativeAddress,
            TRequest request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            await InvokeAsync(relativeAddress, HttpMethod.Post, request, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }

        #endregion

        #region Put

        /// <inheritdoc />
        public async Task<TResponse> PutAsync<TRequest, TResponse>(Uri relativeAddress,
            TRequest request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return
                await
                    InvokeAsync<TRequest, TResponse>(relativeAddress, HttpMethod.Put, request, errorHandler,
                        httpCompletionOption,
                        cancellationToken, cancellationHandler).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResponse> PutAsync<TResponse>(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return await InvokeAsync<TResponse>(relativeAddress, HttpMethod.Put, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task PutAsync<TRequest>(Uri relativeAddress,
            TRequest request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            await InvokeAsync(relativeAddress, HttpMethod.Put, request, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }

        #endregion

        #region Implementation

        private async Task<TResponse> InvokeAsync<TRequest, TResponse>(Uri relativeAddress,
            HttpMethod method,
            TRequest request,
            Action<Exception> errorHandler = null,
            HttpCompletionOption? httpCompletionOption = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null)
        {
            using (var client = HttpClientFactory.Create())
            {
                string serlializedPayload = PayloadSerializer.Serialize(request);
                using (var content = new StringContent(serlializedPayload, Encoding.UTF8, PayloadSerializer.ContentType)
                    )
                {
                    using (
                        var httpRequestMessage = new HttpRequestMessage(method, new Uri(BaseAddress, relativeAddress))
                        {
                            Content = content
                        })
                    {
                        if (RequestHeaders.Count > 0)
                            foreach (var header in RequestHeaders)
                                httpRequestMessage.WithHeader(header.Name, header.Value);
                        try
                        {
                            using (
                                var response =
                                    await client.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken)
                                        .ConfigureAwait(false))
                            {
                                response.EnsureSuccessStatusCode();
                                string returnPayload = await response.Content.ReadAsStringAsync()
                                    .ConfigureAwait(false);
                                return PayloadDeserializer.Deserialize<TResponse>(returnPayload);
                            }
                        }
                        catch (OperationCanceledException canceledException)
                        {
                            if (cancellationHandler != null)
                                cancellationHandler();
                            else if (errorHandler != null)
                                errorHandler(canceledException);
                            else throw;
                        }
                        catch (Exception error)
                        {
                            if (errorHandler != null)
                                errorHandler(error);
                            else throw;
                        }
                        return default(TResponse); // UGLY. In order to avoid compiler error CS0161
                    }
                }
            }
        }

        private async Task<TResponse> InvokeAsync<TResponse>(Uri relativeAddress,
            HttpMethod method,
            Action<Exception> errorHandler = null,
            HttpCompletionOption? httpCompletionOption = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null)
        {
            using (var client = HttpClientFactory.Create())
            {
                using (var content = new StringContent(string.Empty, Encoding.UTF8, PayloadSerializer.ContentType))
                {
                    using (
                        var httpRequestMessage = new HttpRequestMessage(method, new Uri(BaseAddress, relativeAddress))
                        {
                            Content = content
                        })
                    {
                        if (RequestHeaders.Count > 0)
                            foreach (var header in RequestHeaders)
                                httpRequestMessage.WithHeader(header.Name, header.Value);
                        try
                        {
                            using (
                                var response =
                                    await client.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken)
                                        .ConfigureAwait(false))
                            {
                                response.EnsureSuccessStatusCode();
                                string returnPayload = await response.Content.ReadAsStringAsync()
                                    .ConfigureAwait(false);
                                return PayloadDeserializer.Deserialize<TResponse>(returnPayload);
                            }
                        }
                        catch (OperationCanceledException canceledException)
                        {
                            if (cancellationHandler != null)
                                cancellationHandler();
                            else if (errorHandler != null)
                                errorHandler(canceledException);
                            else throw;
                        }
                        catch (Exception error)
                        {
                            if (errorHandler != null)
                                errorHandler(error);
                            else throw;
                        }
                        return default(TResponse); // UGLY. In order to avoid compiler error CS0161
                    }
                }
            }
        }

        private async Task InvokeAsync<TRequest>(Uri relativeAddress,
            HttpMethod method,
            TRequest request,
            Action<Exception> errorHandler = null,
            HttpCompletionOption? httpCompletionOption = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null)
        {
            using (var client = HttpClientFactory.Create())
            {
                string serlializedPayload = PayloadSerializer.Serialize(request);
                using (var content = new StringContent(serlializedPayload, Encoding.UTF8, PayloadSerializer.ContentType)
                    )
                {
                    using (
                        var httpRequestMessage = new HttpRequestMessage(method, new Uri(BaseAddress, relativeAddress))
                        {
                            Content = content
                        })
                    {
                        if (RequestHeaders.Count > 0)
                            foreach (var header in RequestHeaders)
                                httpRequestMessage.WithHeader(header.Name, header.Value);
                        try
                        {
                            using (
                                var response =
                                    await client.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken)
                                        .ConfigureAwait(false))
                                response.EnsureSuccessStatusCode();
                        }
                        catch (OperationCanceledException canceledException)
                        {
                            if (cancellationHandler != null)
                                cancellationHandler();
                            else if (errorHandler != null)
                                errorHandler(canceledException);
                            else throw;
                        }
                        catch (Exception error)
                        {
                            if (errorHandler != null)
                                errorHandler(error);
                            else throw;
                        }
                    }
                }
            }
        }

        private async Task InvokeAsync(Uri relativeAddress,
            HttpMethod method,
            Action<Exception> errorHandler = null,
            HttpCompletionOption? httpCompletionOption = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null)
        {
            using (var client = HttpClientFactory.Create())
            {
                using (
                    var httpRequestMessage = new HttpRequestMessage(method, new Uri(BaseAddress, relativeAddress)))
                {
                    if (RequestHeaders.Count > 0)
                        foreach (var header in RequestHeaders)
                            httpRequestMessage.WithHeader(header.Name, header.Value);
                    try
                    {
                        using (
                            var response =
                                await client.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken)
                                    .ConfigureAwait(false))
                            response.EnsureSuccessStatusCode();
                    }
                    catch (OperationCanceledException canceledException)
                    {
                        if (cancellationHandler != null)
                            cancellationHandler();
                        else if (errorHandler != null)
                            errorHandler(canceledException);
                        else throw;
                    }
                    catch (Exception error)
                    {
                        if (errorHandler != null)
                            errorHandler(error);
                        else throw;
                    }
                }
            }
        }

        #endregion
    }
}
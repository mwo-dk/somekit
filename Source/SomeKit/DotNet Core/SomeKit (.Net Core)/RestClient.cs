using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SomeKit.Extensions;
using SomeKit.Serialization.Json;

namespace SomeKit
{
    /// <summary>
    /// Provides implementatin of <see cref="IRestClient"/>
    /// </summary>
    public sealed class RestClient : IRestClient
    {
        public RestClient(IHttpClientFactory httpClientFactory,
            IPayloadSerializer payloadSerializer,
            IPayloadDeserializer payloadDeserializer)
        {
            HttpClientFactory = httpClientFactory ?? new HttpClientFactory();
            PayloadSerializer = payloadSerializer ?? new JsonPayloadSerializer();
            PayloadDeserializer = payloadDeserializer ?? new JsonPayloadDeserializer();
            RequestHeaders = new HttpRequestHeaderCollection();
        }

        /// <inheritdoc/>
        public Uri BaseAddress { get; set; }
        /// <inheritdoc/>
        public IHttpRequestHeaderCollection RequestHeaders { get; }

        #region Support services
        /// <inheritdoc/>
        public IHttpClientFactory HttpClientFactory { get; set; }
        /// <inheritdoc/>
        public IPayloadSerializer PayloadSerializer { get; set; }
        /// <inheritdoc/>
        public IPayloadDeserializer PayloadDeserializer { get; set; }

        #endregion

        #region Get
        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(Uri relativeAddrress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddrress == null)
                throw new ArgumentNullException(nameof(relativeAddrress));

            return await InvokeAsync<T>(relativeAddrress, HttpMethod.Get, errorHandler, httpCompletionOption, 
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }
        #endregion

        #region Post
        /// <inheritdoc/>
        public async Task<RESPONSE> PostAsync<REQUEST, RESPONSE>(Uri relativeAddress, 
            REQUEST request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return await InvokeAsync<REQUEST, RESPONSE>(relativeAddress, HttpMethod.Post, request, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<RESPONSE> PostAsync<RESPONSE>(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return await InvokeAsync<RESPONSE>(relativeAddress, HttpMethod.Post, errorHandler, httpCompletionOption, 
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }
        /// <inheritdoc/>
        public async Task PostAsync<REQUEST>(Uri relativeAddress, 
            REQUEST request,
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
        /// <inheritdoc/>
        public async Task<RESPONSE> PutAsync<REQUEST, RESPONSE>(Uri relativeAddress,
            REQUEST request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return await InvokeAsync<REQUEST, RESPONSE>(relativeAddress, HttpMethod.Put, request, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<RESPONSE> PutAsync<RESPONSE>(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null)
        {
            if (relativeAddress == null)
                throw new ArgumentNullException(nameof(relativeAddress));

            return await InvokeAsync<RESPONSE>(relativeAddress, HttpMethod.Put, errorHandler, httpCompletionOption,
                cancellationToken, cancellationHandler)
                .ConfigureAwait(false);
        }
        /// <inheritdoc/>
        public async Task PutAsync<REQUEST>(Uri relativeAddress,
            REQUEST request,
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

        #region Delete
        /// <inheritdoc/>
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

        #region Implementation

        private async Task<RESPONSE> InvokeAsync<REQUEST, RESPONSE>(Uri relativeAddress, 
            HttpMethod method, 
            REQUEST request,
            Action<Exception> errorHandler = null,
            HttpCompletionOption? httpCompletionOption = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null)
        {
            using (var client = HttpClientFactory.Create())
            {

                var serlializedPayload = PayloadSerializer.Serialize(request);
                using (var content = new StringContent(serlializedPayload, Encoding.UTF8, PayloadSerializer.ContentType))
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
                                var returnPayload = await response.Content.ReadAsStringAsync()
                                    .ConfigureAwait(false);
                                return PayloadDeserializer.Deserialize<RESPONSE>(returnPayload);
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
                        return default(RESPONSE); // UGLY. In order to avoid compiler error CS0161
                    }
                }
            }
        }

        private async Task<RESPONSE> InvokeAsync<RESPONSE>(Uri relativeAddress, 
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
                                var returnPayload = await response.Content.ReadAsStringAsync()
                                    .ConfigureAwait(false);
                                return PayloadDeserializer.Deserialize<RESPONSE>(returnPayload);
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
                        return default(RESPONSE); // UGLY. In order to avoid compiler error CS0161
                    }
                }
            }
        }

        private async Task InvokeAsync<REQUEST>(Uri relativeAddress, 
            HttpMethod method, 
            REQUEST request,
            Action<Exception> errorHandler = null,
            HttpCompletionOption? httpCompletionOption = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null)
        {
            using (var client = HttpClientFactory.Create())
            {
                var serlializedPayload = PayloadSerializer.Serialize(request);
                using (var content = new StringContent(serlializedPayload, Encoding.UTF8, PayloadSerializer.ContentType))
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

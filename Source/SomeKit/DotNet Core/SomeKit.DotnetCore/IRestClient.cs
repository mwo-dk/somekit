using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace SomeKit
{
    /// <summary>
    /// REST client
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// The base address of the REST service
        /// </summary>
        Uri BaseAddress { get; set; }

        /// <summary>
        /// The additional request headers <see cref="IHttpRequestHeaderCollection"/>
        /// </summary>
        IHttpRequestHeaderCollection RequestHeaders { get; }

        #region Support services
        /// <summary>
        /// <see cref="IHttpClientFactory"/> utilized to create <see cref="HttpClient"/>s
        /// </summary>
        IHttpClientFactory HttpClientFactory { get; set; }
        /// <summary>
        /// <see cref="IPayloadSerializer"/> utilized to serialize payload
        /// </summary>
        IPayloadSerializer PayloadSerializer { get; set; }
        /// <summary>
        /// <see cref="IPayloadDeserializer"/> utilized to deserialize return payload
        /// </summary>
        IPayloadDeserializer PayloadDeserializer { get; set; }
        #endregion

        #region Get

        /// <summary>
        /// Invokes a GET request
        /// </summary>
        /// <typeparam name="T">The type of payload to which the return payload should be deserialized</typeparam>
        /// <param name="relativeAddrress">The relative <see cref="Uri"/> of the resource</param>
        /// <param name="errorHandler">Optional error handler. If this is provided, errors will not be propagated</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/></param>
        /// <param name="cancellationHandler">Optional cancellation handler. If this is provided, cancelletion exceptions will not be propagated</param>
        /// <param name="httpCompletionOption">Optional <see cref="HttpCompletionOption"/></param>
        /// <returns>The resource</returns>
        Task<T> GetAsync<T>(Uri relativeAddrress, 
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null);

        #endregion

        #region Post
        /// <summary>
        /// Invokes a POST request
        /// </summary>
        /// <typeparam name="REQUEST">The type of the request</typeparam>
        /// <typeparam name="RESPONSE">The type of the response</typeparam>
        /// <param name="relativeAddress">The relative address of the resource</param>
        /// <param name="request">The request payload</param>
        /// <param name="errorHandler">Optional error handler. If this is provided, errors will not be propagated</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/></param>
        /// <param name="cancellationHandler">Optional cancellation handler. If this is provided, cancelletion exceptions will not be propagated</param>
        /// <param name="httpCompletionOption">Optional <see cref="HttpCompletionOption"/></param>
        /// <returns>The newly created resource</returns>
        Task<RESPONSE> PostAsync<REQUEST, RESPONSE>(Uri relativeAddress, 
            REQUEST request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null);
        /// <summary>
        /// Invokes a POST request
        /// </summary>
        /// <typeparam name="RESPONSE">The type of the response</typeparam>
        /// <param name="relativeAddress">The relative address of the resource</param>
        /// <param name="errorHandler">Optional error handler. If this is provided, errors will not be propagated</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/></param>
        /// <param name="cancellationHandler">Optional cancellation handler. If this is provided, cancelletion exceptions will not be propagated</param>
        /// <param name="httpCompletionOption">Optional <see cref="HttpCompletionOption"/></param>
        /// <returns>The newly created resource</returns>
        Task<RESPONSE> PostAsync<RESPONSE>(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null);
        /// <summary>
        /// Invokes a POST request
        /// </summary>
        /// <typeparam name="REQUEST">The type of the request</typeparam>
        /// <param name="relativeAddress">The relative address of the resource</param>
        /// <param name="request">The request payload</param>
        /// <param name="errorHandler">Optional error handler. If this is provided, errors will not be propagated</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/></param>
        /// <param name="cancellationHandler">Optional cancellation handler. If this is provided, cancelletion exceptions will not be propagated</param>
        /// <param name="httpCompletionOption">Optional <see cref="HttpCompletionOption"/></param>
        /// <returns>An awaitable task</returns>
        Task PostAsync<REQUEST>(Uri relativeAddress, 
            REQUEST request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null);

        #endregion

        #region Put
        /// <summary>
        /// Invokes a PUT request
        /// </summary>
        /// <typeparam name="REQUEST">The type of the request</typeparam>
        /// <typeparam name="RESPONSE">The type of the response</typeparam>
        /// <param name="relativeAddress">The relative address of the resource</param>
        /// <param name="request">The request payload</param>
        /// <param name="errorHandler">Optional error handler. If this is provided, errors will not be propagated</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/></param>
        /// <param name="cancellationHandler">Optional cancellation handler. If this is provided, cancelletion exceptions will not be propagated</param>
        /// <param name="httpCompletionOption">Optional <see cref="HttpCompletionOption"/></param>
        /// <returns>The replaced resource</returns>
        Task<RESPONSE> PutAsync<REQUEST, RESPONSE>(Uri relativeAddress, 
            REQUEST request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null);
        /// <summary>
        /// Invokes a POST request
        /// </summary>
        /// <typeparam name="RESPONSE">The type of the response</typeparam>
        /// <param name="relativeAddress">The relative address of the resource</param>
        /// <param name="errorHandler">Optional error handler. If this is provided, errors will not be propagated</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/></param>
        /// <param name="cancellationHandler">Optional cancellation handler. If this is provided, cancelletion exceptions will not be propagated</param>
        /// <param name="httpCompletionOption">Optional <see cref="HttpCompletionOption"/></param>
        /// <returns>The replaced resource</returns>
        Task<RESPONSE> PutAsync<RESPONSE>(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null);
        /// <summary>
        /// Invokes a POST request
        /// </summary>
        /// <typeparam name="REQUEST">The type of the request</typeparam>
        /// <param name="relativeAddress">The relative address of the resource</param>
        /// <param name="request">The request payload</param>
        /// <param name="errorHandler">Optional error handler. If this is provided, errors will not be propagated</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/></param>
        /// <param name="cancellationHandler">Optional cancellation handler. If this is provided, cancelletion exceptions will not be propagated</param>
        /// <param name="httpCompletionOption">Optional <see cref="HttpCompletionOption"/></param>
        /// <returns>An awaitable task</returns>
        Task PutAsync<REQUEST>(Uri relativeAddress, 
            REQUEST request,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null);
        #endregion

        #region Delete
        /// <summary>
        /// Invokes a DELETE request
        /// </summary>
        /// <param name="relativeAddress">The relative address of the resource</param>
        /// <param name="errorHandler">Optional error handler. If this is provided, errors will not be propagated</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/></param>
        /// <param name="cancellationHandler">Optional cancellation handler. If this is provided, cancelletion exceptions will not be propagated</param>
        /// <param name="httpCompletionOption">Optional <see cref="HttpCompletionOption"/></param>
        /// <returns>An awaitable task</returns>
        Task DeleteAsync(Uri relativeAddress,
            Action<Exception> errorHandler = null,
            CancellationToken? cancellationToken = null,
            Action cancellationHandler = null,
            HttpCompletionOption? httpCompletionOption = null);

        #endregion
    }
}

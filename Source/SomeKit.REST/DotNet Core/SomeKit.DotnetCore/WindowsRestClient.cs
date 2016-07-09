namespace SomeKit.REST
{
    /// <summary>
    /// Extends <see cref="RestClient"/> to utilize Windows Authentication
    /// </summary>
    public class WindowsRestClient : RestClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="payloadSerializer">The <see cref="IPayloadSerializer"/> used to serialize requests</param>
        /// <param name="payloadDeserializer">The <see cref="IPayloadDeserializer"/> used to deserialize requests</param>
        public WindowsRestClient(IPayloadSerializer payloadSerializer,
            IPayloadDeserializer payloadDeserializer)
            : base(new HttpWindowsClientFactory(), payloadSerializer, payloadDeserializer)
        {
            
        }
    }
}

using Newtonsoft.Json;

namespace SomeKit.REST.Serialization.Json
{
    /// <summary>
    ///     Implements <see cref="IPayloadSerializer" /> for Json
    /// </summary>
    public sealed class JsonPayloadSerializer : IPayloadSerializer
    {
        /// <inheritdoc />
        public string ContentType => "application/json";

        /// <inheritdoc />
        public string Serialize<T>(T @object)
        {
            return JsonConvert.SerializeObject(@object);
        }
    }
}
using Newtonsoft.Json;

namespace SomeKit.REST.Serialization.Json
{
    /// <summary>
    /// Implements <see cref="IPayloadDeserializer"/> for Json.
    /// </summary>
    public sealed class JsonPayloadDeserializer : IPayloadDeserializer
    {
        ///<inheritdoc/>
        public T Deserialize<T>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }
}

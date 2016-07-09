namespace SomeKit.REST
{
    /// <summary>
    /// Last in chain serializer. 
    /// </summary>
    public interface IPayloadSerializer
    {
        /// <summary>
        /// The content type to which the serializer serializes
        /// </summary>
        string ContentType { get; }
        /// <summary>
        /// Serializes a given object <paramref name="payload"/> to a string
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="payload">The payload to serialize</param>
        /// <returns>The payload <paramref name="payload"/> serialized as a string</returns>
        string Serialize<T>(T payload);
    }
}

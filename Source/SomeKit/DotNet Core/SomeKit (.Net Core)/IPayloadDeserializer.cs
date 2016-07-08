using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeKit
{
    /// <summary>
    /// Last in chain deserializer. 
    /// </summary>
    public interface IPayloadDeserializer
    {
        /// <summary>
        /// Desializes a given payload to a strongly typed object
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to</typeparam>
        /// <param name="serializedObject">The string serialized representation of the object</param>
        /// <returns>The <paramref name="serializedObject"/> deserialized</returns>
        T Deserialize<T>(string serializedObject);
    }
}

namespace SomeKit.Cryptography
{
    /// <summary>
    /// Interface supporting signing of payloads
    /// </summary>
    public interface ISigningService
    {
        /// <summary>
        /// Hashes and signs a given payload
        /// </summary>
        /// <param name="data">The payload to hash and sign</param>
        /// <returns><paramref name="data"/> hashed and signed</returns>
        byte[] HashAndSign(byte[] data);
    }
}

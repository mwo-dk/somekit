namespace SomeKit.Cryptography
{
    /// <summary>
    ///     Interface describing the ability to encrypt and decrypt payloads
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        ///     Encrypts a given payload
        /// </summary>
        /// <param name="payload">The payload to encrypt</param>
        /// <returns><paramref name="payload" /> encrypted</returns>
        byte[] Encrypt(byte[] payload);

        /// <summary>
        ///     Decrypts a given payload
        /// </summary>
        /// <param name="payload">The payload to decrypt</param>
        /// <returns><paramref name="payload" /> decrypted</returns>
        byte[] Decrypt(byte[] payload);
    }
}
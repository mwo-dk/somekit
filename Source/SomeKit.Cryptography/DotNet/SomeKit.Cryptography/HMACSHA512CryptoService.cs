using System;
using System.Linq;
using System.Security.Cryptography;

namespace SomeKit.Cryptography
{
    /// <summary>
    ///     Impleemnts <see cref="ICryptoService" />
    /// </summary>
    public sealed class Hmacsha512CryptoService :
        ISigningService
    {
        /// <inheritdoc />
        public byte[] SecretKey { get; set; }

        /// <inheritdoc />
        public byte[] HashAndSign(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length == 0)
                return data;

            using (var hmac = new HMACSHA512(SecretKey))
            {
                // Compute the hash of the input data. 
                var hashValue = hmac.ComputeHash(data);
                return hashValue.Concat(data).ToArray();
            }
        }

        /// <inheritdoc />
        public void GenerateSecretKey()
        {
            SecretKey = new byte[64];
            using (var rng = new RNGCryptoServiceProvider())
            {
                // The array is now filled with cryptographically strong random bytes.
                rng.GetBytes(SecretKey);
            }
        }

        /// <summary>
        ///     Verifies whether a given payload (<paramref name="signedData" />) is has been duely signed
        /// </summary>
        /// <param name="signedData">The signed data to verify</param>
        public void VerifySignedData(byte[] signedData)
        {
            // Initialize the keyed hash object.  
            using (var hmac = new HMACSHA512(SecretKey))
            {
                // Create an array to hold the keyed hash value read from the signed data.
                var storedHash = signedData.Take(hmac.HashSize/8).ToArray();
                var originalData = signedData.Skip(hmac.HashSize/8).ToArray();
                var computedHash = hmac.ComputeHash(originalData);
                bool ok = storedHash.SequenceEqual(computedHash);
                if (!ok)
                    throw new Exception("Signature validity could not be verified.");
            }
        }
    }
}
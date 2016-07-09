using System;
using System.Linq;
using System.Security.Cryptography;

namespace SomeKit.Cryptography
{
    /// <summary>
    /// Impleemnts <see cref="ICryptoService"/>
    /// </summary>
    public sealed class HMACSHA512CryptoService : 
        ISigningService
    {
        ///<inheritdoc/>
        public void GenerateSecretKey()
        {
            SecretKey = new byte[64];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // The array is now filled with cryptographically strong random bytes.
                rng.GetBytes(SecretKey);
            }
        }
        ///<inheritdoc/>
        public byte[] SecretKey { get; set; }
        ///<inheritdoc/>
        public byte[] HashAndSign(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length == 0)
                return data;

            using (HMACSHA512 hmac = new HMACSHA512(SecretKey))
            {
                // Compute the hash of the input data. 
                byte[] hashValue = hmac.ComputeHash(data);
                return hashValue.Concat(data).ToArray();
            }
        }
        /// <summary>
        /// Verifies whether a given payload (<paramref name="signedData"/>) is has been duely signed
        /// </summary>
        /// <param name="signedData">The signed data to verify</param>
        public void VerifySignedData(byte[] signedData)
        {
            // Initialize the keyed hash object.  
            using (HMACSHA512 hmac = new HMACSHA512(SecretKey))
            {
                // Create an array to hold the keyed hash value read from the signed data.
                var storedHash = signedData.Take(hmac.HashSize / 8).ToArray();
                var originalData = signedData.Skip(hmac.HashSize / 8).ToArray();
                var computedHash = hmac.ComputeHash(originalData);
                var ok = storedHash.SequenceEqual(computedHash);
                if (!ok)
                    throw new Exception("Signature validity could not be verified.");
            }
        }
    }
}

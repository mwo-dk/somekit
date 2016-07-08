using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cryptography
{
    public sealed class HMACSHA512CryptoService : IHMACSHA512CryptoService
    {
        public void GenerateSecretKey()
        {
            this.SecretKey = new byte[64];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // The array is now filled with cryptographically strong random bytes.
                rng.GetBytes(this.SecretKey);
            }
        }

        public byte[] SecretKey { get; set; }

        public byte[] HashAndSign(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                return data;

            using (HMACSHA512 hmac = new HMACSHA512(this.SecretKey))
            {
                // Compute the hash of the input data. 
                byte[] hashValue = hmac.ComputeHash(data);
                return hashValue.Concat(data).ToArray();
            }
        }

        public void VerifyData(byte[] signedData)
        {
            // Initialize the keyed hash object.  
            using (HMACSHA512 hmac = new HMACSHA512(this.SecretKey))
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

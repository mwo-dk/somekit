using System;
using System.IO;
using System.Security.Cryptography;

namespace SomeKit.Cryptography
{
    /// <summary>
    ///     Implements <see cref="ICryptoService" />
    /// </summary>
    public sealed class AesCryptoService : ICryptoService
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="password">The password to utilize</param>
        /// <param name="salt">The salt to utilize</param>
        /// <param name="iterations">The number of iterations for the hash generator - defaults to one</param>
        public AesCryptoService(byte[] password,
            byte[] salt,
            int iterations)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (salt == null)
                throw new ArgumentNullException(nameof(salt));
            if (iterations < 1)
                throw new ArgumentOutOfRangeException(nameof(iterations));

            Password = password;
            Salt = salt;
            Iterations = iterations;
        }

        private byte[] Password { get; }
        private byte[] Salt { get; }
        private int Iterations { get; }

        /// <summary>
        ///     Encrypts a given payload <paramref name="data" /> using the symmetric AES algorithm
        /// </summary>
        /// <param name="data">The data to encrypt</param>
        /// <returns><paramref name="data" /> AES encrypted</returns>
        public byte[] Encrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
            using (var derivedBytes = new Rfc2898DeriveBytes(Password, Salt, Iterations))
            {
                // Create AES algorithm with 256 bit key and 128-bit block size 
                using (var aes = new AesManaged())
                {
                    aes.Key = derivedBytes.GetBytes(aes.KeySize/8);
                    aes.IV = derivedBytes.GetBytes(aes.BlockSize/8);
                    // Create Memory and Crypto Streams 
                    using (var memoryStream = new MemoryStream())
                    using (var transform = aes.CreateEncryptor())
                    using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        // Encrypt data 
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        ///     Decrypts a given payload <paramref name="data" /> using the symmetric AES algorithm
        /// </summary>
        /// <param name="data">The data to decrypt</param>
        /// <returns><paramref name="data" /> AES decrypted</returns>
        public byte[] Decrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
            using (var derivedBytes = new Rfc2898DeriveBytes(Password, Salt, Iterations))
            {
                // Create AES algorithm with 256 bit key and 128-bit block size 
                using (var aes = new AesManaged())
                {
                    aes.Key = derivedBytes.GetBytes(aes.KeySize/8);
                    aes.IV = derivedBytes.GetBytes(aes.BlockSize/8);
                    // Create Memory and Crypto Streams 
                    using (var memoryStream = new MemoryStream())
                    using (var transform = aes.CreateDecryptor())
                    using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        // Encrypt data 
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}
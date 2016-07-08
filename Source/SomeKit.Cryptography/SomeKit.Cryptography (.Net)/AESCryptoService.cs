using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cryptography
{
    public sealed class AESCryptoService : IAESCryptoService
    {
        /// <summary>
        /// AES encrypts data according to the password and salt given.
        /// </summary>
        /// <param name="dataToEncrypt">Unencrypted data.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>Encrypted data.</returns>
        public static string Encrypt(string dataToEncrypt, string password, string salt)
        {
            AesManaged aes = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            try
            {
                //Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt));
                //Create AES algorithm with 256 bit key and 128-bit block size 
                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);
                //Create Memory and Crypto Streams 
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                //Encrypt Data 
                byte[] data = Encoding.UTF8.GetBytes(dataToEncrypt);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                //Return Base 64 String 
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            finally
            {
                if (cryptoStream != null)
                    cryptoStream.Close();
                if (memoryStream != null)
                    memoryStream.Close();
                if (aes != null)
                    aes.Clear();
            }
        }

        /// <summary>
        /// AES decrypts data according to the password and salt given.
        /// </summary>
        /// <param name="dataToDecrypt">Encrypted data.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>Decrypted data.</returns>
        public static string Decrypt(string dataToDecrypt, string password, string salt)
        {
            AesManaged aes = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            try
            {
                //Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt));
                //Create AES algorithm with 256 bit key and 128-bit block size 
                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);
                //Create Memory and Crypto Streams 
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                //Decrypt Data 
                byte[] data = Convert.FromBase64String(dataToDecrypt);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                //Return Decrypted String 
                byte[] decryptBytes = memoryStream.ToArray();
                return Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
            }
            finally
            {
                if (cryptoStream != null)
                    cryptoStream.Close();
                if (memoryStream != null)
                    memoryStream.Close();
                if (aes != null)
                    aes.Clear();
            }
        }
    }
}

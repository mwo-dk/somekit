using System;

namespace SomeKit.Cryptography.Extensions
{
    /// <summary>
    /// Extension methods for byte arrays
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts a given payload to a BASE 64 string
        /// </summary>
        /// <param name="data">The data to convert</param>
        /// <returns><paramref name="data"/> as a BASE 64 encoded string</returns>
        public static string ToBase64(this byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Converts a given payload from a BASE 64 string format to original byte array
        /// </summary>
        /// <param name="data">The data represented as a BASE 64 encoded string</param>
        /// <returns>Data as a raw byte array</returns>
        public static byte[] FromBase64(this string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException(nameof(data));

            return Convert.FromBase64String(data);
        }

        /// <summary>
        /// In place reverses a the payload <paramref name="data"/>
        /// </summary>
        /// <param name="data">The payload to reverse</param>
        /// <returns><paramref name="data"/> reversed in place</returns>
        public static byte[] InPlaceReverse(this byte[] data)
        {
            if (data != null && data.Length > 1)
            {
                var length = data.Length;
                for (var n = 0; n < length / 2; ++n)
                {
                    var temp = data[length - n - 1];
                    data[length - n - 1] = data[n];
                    data[n] = temp;
                }
            }

            return data;
        }
    }
}

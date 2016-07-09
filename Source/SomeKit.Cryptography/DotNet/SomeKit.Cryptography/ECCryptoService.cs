using System;
using System.IO;
using System.Security.Cryptography;

namespace SomeKit.Cryptography
{
    /// <summary>
    ///     Implements <see cref="ICryptoService" />
    /// </summary>
    public sealed class EcCryptoService : ICryptoService
    {
        private byte[] _iv;
        private byte[] _key;
        private byte[] _peerPublicKey;
        private byte[] _privateKey;
        private byte[] _publicKey;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="generateKey">Flag telling wether to generate keys</param>
        public EcCryptoService(bool generateKey = true)
        {
            if (generateKey)
            {
                var cngKey = GenerateExportableKey();
                _privateKey = cngKey.Export(CngKeyBlobFormat.EccPrivateBlob);
                _publicKey = cngKey.Export(CngKeyBlobFormat.EccPublicBlob);
            }
        }

        /// <summary>
        ///     The private key
        /// </summary>
        public byte[] PrivateKey
        {
            get { return _privateKey; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _privateKey = value;
                PublicKey = GetAlgoritm(_privateKey).PublicKey.ToByteArray();
            }
        }

        /// <summary>
        ///     The public key
        /// </summary>
        public byte[] PublicKey
        {
            get { return _publicKey; }
            set
            {
                _publicKey = value;
                if (_publicKey != null && _peerPublicKey != null)
                    AttachToPeer();
            }
        }

        /// <summary>
        ///     The public key of the peer
        /// </summary>
        public byte[] PeerPublicKey
        {
            get { return _peerPublicKey; }
            set
            {
                _peerPublicKey = value;
                if (_publicKey != null && _peerPublicKey != null)
                    AttachToPeer();
            }
        }

        /// <inheritdoc />
        public byte[] Encrypt(byte[] data)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = _key;

                // Encrypt the message 
                using (var ciphertext = new MemoryStream())
                using (var cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                    _iv = aes.IV;
                    return ciphertext.ToArray();
                }
            }
        }

        /// <inheritdoc />
        public byte[] Decrypt(byte[] data)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = _key;
                aes.IV = _iv;
                // Decrypt the message 
                using (var plaintext = new MemoryStream())
                {
                    using (var cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                        return plaintext.ToArray();
                    }
                }
            }
        }

        /// <summary>
        ///     Sets the initialization vector
        /// </summary>
        /// <param name="iv">The new initialization vector</param>
        /// <returns>This service</returns>
        public EcCryptoService WithInitializationVector(byte[] iv)
        {
            if (iv == null)
                throw new ArgumentNullException(nameof(iv));

            _iv = iv;
            return this;
        }

        /// <inheritdoc />
        public static CngKey GenerateExportableKey()
        {
            return CngKey.Create(CngAlgorithm.ECDiffieHellmanP256, null,
                new CngKeyCreationParameters {ExportPolicy = CngExportPolicies.AllowPlaintextArchiving});
        }

        private void AttachToPeer()
        {
            using (var ecc = GetAlgoritm(_privateKey))
            {
                _key = ecc.DeriveKeyMaterial(CngKey.Import(_peerPublicKey, CngKeyBlobFormat.EccPublicBlob));
            }
        }

        private static ECDiffieHellmanCng GetAlgoritm(byte[] privateKey = null)
        {
            var result = privateKey == null
                ? new ECDiffieHellmanCng()
                : new ECDiffieHellmanCng(CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob));
            result.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            result.HashAlgorithm = CngAlgorithm.Sha256;
            return result;
        }
    }
}
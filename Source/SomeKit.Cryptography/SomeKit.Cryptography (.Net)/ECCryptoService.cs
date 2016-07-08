using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SomeKit.Cryptography__.Net_;

namespace SomeKit.Cryptography
{
    public sealed class ECCryptoService : IECCryptoService
    {
        private byte[] _privateKey;
        private byte[] _publicKey;
        private byte[] _peerPublicKey;
        private byte[] _key;

        public ECCryptoService(bool generateKey = true)
        {
            if (generateKey)
            {
                var cngKey = GenerateExportableKey();
                this._privateKey = cngKey.Export(CngKeyBlobFormat.EccPrivateBlob);
                this._publicKey = cngKey.Export(CngKeyBlobFormat.EccPublicBlob);
            }
        }

        public byte[] PrivateKey
        {
            get { return this._privateKey; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                this._privateKey = value;
                this.PublicKey = GetAlgoritm(this._privateKey).PublicKey.ToByteArray();
            }
        }

        public byte[] PublicKey
        {
            get { return this._publicKey; }
            set
            {
                this._publicKey = value;
                if (this._publicKey != null && this._peerPublicKey != null)
                    this.AttachToPeer();
            }
        }

        public byte[] PeerPublicKey
        {
            get { return this._peerPublicKey; }
            set
            {
                this._peerPublicKey = value;
                if (this._publicKey != null && this._peerPublicKey != null)
                    this.AttachToPeer();
            }
        }

        public Tuple<byte[], byte[]> Encrypt(byte[] data)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = this._key;

                // Encrypt the message 
                using (MemoryStream ciphertext = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                    return new Tuple<byte[], byte[]>(ciphertext.ToArray(), aes.IV);
                }
            }
        }

        public byte[] Decrypt(byte[] data, byte[] iv)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = this._key;
                aes.IV = iv;
                // Decrypt the message 
                using (MemoryStream plaintext = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                        return plaintext.ToArray();
                    }
                }
            }
        }
        public static CngKey GenerateExportableKey()
        {
            return CngKey.Create(CngAlgorithm.ECDiffieHellmanP256, null,
                new CngKeyCreationParameters { ExportPolicy = CngExportPolicies.AllowPlaintextArchiving });
        }

        private void AttachToPeer()
        {
            using (var ecc = GetAlgoritm(this._privateKey))
            {
                this._key = ecc.DeriveKeyMaterial(CngKey.Import(this._peerPublicKey, CngKeyBlobFormat.EccPublicBlob));
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

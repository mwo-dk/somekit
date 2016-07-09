using System;
using System.Security.Cryptography;

namespace SomeKit.Cryptography
{
    /// <summary>
    ///     Implaments <see cref="ICryptoService" />
    /// </summary>
    public sealed class RsaCryptoService :
        ICryptoService,
        ISigningService
    {
        private byte[] _privateKey;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="generateKey">Flag telling whether to auto-generate the keys</param>
        public RsaCryptoService(bool generateKey = true)
        {
            if (!generateKey)
                return;

            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                _privateKey = rsaCrypto.ExportCspBlob(true);
                PublicKey = rsaCrypto.ExportCspBlob(false);
            }
        }

        /// <inheritdoc />
        public byte[] PrivateKey
        {
            get { return _privateKey; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                using (var rsaCrypto = new RSACryptoServiceProvider())
                {
                    rsaCrypto.ImportCspBlob(value);
                    _privateKey = rsaCrypto.ExportCspBlob(true);
                    PublicKey = rsaCrypto.ExportCspBlob(false);
                }
            }
        }

        /// <inheritdoc />
        public byte[] PublicKey { get; set; }

        /// <inheritdoc />
        public byte[] Encrypt(byte[] data)
        {
            using (var rsaCsp = new RSACryptoServiceProvider())
            {
                rsaCsp.ImportCspBlob(PublicKey);
                return rsaCsp.Encrypt(data, false);
            }
        }

        /// <inheritdoc />
        public byte[] Decrypt(byte[] encryptedData)
        {
            using (var rsaCsp = new RSACryptoServiceProvider())
            {
                rsaCsp.ImportCspBlob(_privateKey);
                return rsaCsp.Decrypt(encryptedData, false);
            }
        }

        /// <inheritdoc />
        public byte[] HashAndSign(byte[] data)
        {
            using (var rsaCsp = new RSACryptoServiceProvider())
            {
                using (var hash = new SHA384Managed())
                {
                    rsaCsp.ImportCspBlob(_privateKey);
                    var hashedData = hash.ComputeHash(data);
                    return rsaCsp.SignHash(hashedData, CryptoConfig.MapNameToOID("SHA384"));
                }
            }
        }

        /// <summary>
        ///     Verifies whether the payload <paramref name="signedData" /> is signed with the provided signature (
        ///     <paramref name="signature" />)
        /// </summary>
        /// <param name="signedData">The payload to verify</param>
        /// <param name="signature">The signature to verify against</param>
        public void VerifySignedData(byte[] signedData, byte[] signature)
        {
            using (var rsaCsp = new RSACryptoServiceProvider())
            using (var hash = new SHA384Managed())
            {
                rsaCsp.ImportCspBlob(PublicKey);
                bool dataOk = rsaCsp.VerifyData(signedData, CryptoConfig.MapNameToOID("SHA384"), signature);
                if (!dataOk)
                    throw new Exception("Data validity could not be verified.");
                var hashedData = hash.ComputeHash(signedData);
                bool hashOk = rsaCsp.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA384"), signature);
                if (!hashOk)
                    throw new Exception("Signature validity could not be verified.");
            }
        }

        /// <inheritdoc />
        public void ImportXml(string rsaCryptoServiceAsXml)
        {
            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                rsaCrypto.FromXmlString(rsaCryptoServiceAsXml);
                PrivateKey = rsaCrypto.ExportCspBlob(true);
            }
        }

        /// <inheritdoc />
        public string ExportXml(bool includePrivateKey)
        {
            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                rsaCrypto.ImportCspBlob(includePrivateKey ? _privateKey : PublicKey);
                return rsaCrypto.ToXmlString(includePrivateKey);
            }
        }
    }
}
using System;
using System.Security.Cryptography;

namespace SomeKit.Cryptography
{
    /// <summary>
    /// Implaments <see cref="ICryptoService"/>
    /// </summary>
    public sealed class RSACryptoService : 
        ICryptoService,
        ISigningService
    {
        private byte[] _privateKey;
        private byte[] _publicKey;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="generateKey">Flag telling whether to auto-generate the keys</param>
        public RSACryptoService(bool generateKey = true)
        {
            if (!generateKey)
                return;

            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                _privateKey = rsaCrypto.ExportCspBlob(true);
                _publicKey = rsaCrypto.ExportCspBlob(false);
            }
        }
        ///<inheritdoc/>
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
                    _publicKey = rsaCrypto.ExportCspBlob(false);
                }
            }
        }
        ///<inheritdoc/>
        public byte[] PublicKey { get { return _publicKey; } set { _publicKey = value; } }
        ///<inheritdoc/>
        public byte[] Encrypt(byte[] data)
        {
            using (var rsaCSP = new RSACryptoServiceProvider())
            {
                rsaCSP.ImportCspBlob(_publicKey);
                return rsaCSP.Encrypt(data, false);
            }
        }
        ///<inheritdoc/>
        public byte[] Decrypt(byte[] encryptedData)
        {
            using (var rsaCSP = new RSACryptoServiceProvider())
            {
                rsaCSP.ImportCspBlob(_privateKey);
                return rsaCSP.Decrypt(encryptedData, false);
            }
        }
        ///<inheritdoc/>
        public byte[] HashAndSign(byte[] data)
        {
            using (var rsaCSP = new RSACryptoServiceProvider())
            {
                using (var hash = new SHA384Managed())
                {
                    rsaCSP.ImportCspBlob(_privateKey);
                    var hashedData = hash.ComputeHash(data);
                    return rsaCSP.SignHash(hashedData, CryptoConfig.MapNameToOID("SHA384"));
                }
            }
        }
        /// <summary>
        /// Verifies whether the payload <paramref name="signedData"/> is signed with the provided signature (<paramref name="signature"/>)
        /// </summary>
        /// <param name="signedData">The payload to verify</param>
        /// <param name="signature">The signature to verify against</param>
        public void VerifySignedData(byte[] signedData, byte[] signature)
        {
            using (var rsaCSP = new RSACryptoServiceProvider())
            using (var hash = new SHA384Managed())
            {

                rsaCSP.ImportCspBlob(_publicKey);
                bool dataOk = rsaCSP.VerifyData(signedData, CryptoConfig.MapNameToOID("SHA384"), signature);
                if (!dataOk)
                    throw new Exception("Data validity could not be verified.");
                var hashedData = hash.ComputeHash(signedData);
                bool hashOk = rsaCSP.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA384"), signature);
                if (!hashOk)
                    throw new Exception("Signature validity could not be verified.");
            }
        }

        ///<inheritdoc/>
        public void ImportXml(string rsaCryptoServiceAsXml)
        {
            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                rsaCrypto.FromXmlString(rsaCryptoServiceAsXml);
                PrivateKey = rsaCrypto.ExportCspBlob(true);
            }
        }
        ///<inheritdoc/>
        public string ExportXml(bool includePrivateKey)
        {
            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                rsaCrypto.ImportCspBlob(includePrivateKey ? _privateKey : _publicKey);
                return rsaCrypto.ToXmlString(includePrivateKey);
            }
        }
    }
}

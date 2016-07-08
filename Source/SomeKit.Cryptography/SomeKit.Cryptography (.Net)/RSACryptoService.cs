using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cryptography
{
    public sealed class RSACryptoService : IRSACryptoService
    {
        private byte[] _privateKey;
        private byte[] _publicKey;

        public RSACryptoService(bool generateKey = true)
        {
            if (!generateKey)
                return;

            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                this._privateKey = rsaCrypto.ExportCspBlob(true);
                this._publicKey = rsaCrypto.ExportCspBlob(false);
            }
        }

        public byte[] PrivateKey
        {
            get { return this._privateKey; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                using (var rsaCrypto = new RSACryptoServiceProvider())
                {
                    rsaCrypto.ImportCspBlob(value);
                    this._privateKey = rsaCrypto.ExportCspBlob(true);
                    this._publicKey = rsaCrypto.ExportCspBlob(false);
                }
            }
        }

        public byte[] PublicKey { get { return this._publicKey; } set { this._publicKey = value; } }

        public byte[] Encrypt(byte[] data)
        {
            using (var rsaCSP = new RSACryptoServiceProvider())
            {
                rsaCSP.ImportCspBlob(this._publicKey);
                return rsaCSP.Encrypt(data, false);
            }
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            using (var rsaCSP = new RSACryptoServiceProvider())
            {
                rsaCSP.ImportCspBlob(this._privateKey);
                return rsaCSP.Decrypt(encryptedData, false);
            }
        }

        public byte[] HashAndSign(byte[] data)
        {
            using (var rsaCSP = new RSACryptoServiceProvider())
            {
                var hash = new SHA384Managed();
                rsaCSP.ImportCspBlob(this._privateKey);
                var hashedData = hash.ComputeHash(data);
                return rsaCSP.SignHash(hashedData, CryptoConfig.MapNameToOID("SHA384"));
            }
        }

        public void VerifySignature(byte[] signedData, byte[] signature)
        {
            using (var rsaCSP = new RSACryptoServiceProvider())
            using (var hash = new SHA384Managed())
            {

                rsaCSP.ImportCspBlob(this._publicKey);
                bool dataOK = rsaCSP.VerifyData(signedData, CryptoConfig.MapNameToOID("SHA384"), signature);
                if (!dataOK)
                    throw new Exception("Data validity could not be verified.");
                var hashedData = hash.ComputeHash(signedData);
                bool hashOk = rsaCSP.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA384"), signature);
                if (!hashOk)
                    throw new Exception("Signature validity could not be verified.");
            }
        }


        public void ImportXml(string rsaCryptoServiceAsXml)
        {
            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                rsaCrypto.FromXmlString(rsaCryptoServiceAsXml); ;
                this.PrivateKey = rsaCrypto.ExportCspBlob(true);
            }
        }

        public string ExportXml(bool includePrivateKey)
        {
            using (var rsaCrypto = new RSACryptoServiceProvider())
            {
                if (includePrivateKey)
                    rsaCrypto.ImportCspBlob(this._privateKey);
                else rsaCrypto.ImportCspBlob(this._publicKey);
                return rsaCrypto.ToXmlString(includePrivateKey);
            }
        }
    }
}

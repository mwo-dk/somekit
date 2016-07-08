using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cryptography
{
    public interface IRSACryptoService
    {
        byte[] PrivateKey { set; }
        byte[] PublicKey { get; }
        byte[] Encrypt(byte[] data);
        byte[] Decrypt(byte[] encryptedData);

        byte[] HashAndSign(byte[] data);
        void VerifySignature(byte[] signedData, byte[] signature);

        void ImportXml(string rsaCryptoServiceAsXml);
        string ExportXml(bool includePrivateKey);
    }
}

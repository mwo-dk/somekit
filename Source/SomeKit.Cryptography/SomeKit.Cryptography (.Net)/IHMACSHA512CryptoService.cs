using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cryptography
{
    public interface IHMACSHA512CryptoService
    {
        void GenerateSecretKey();
        byte[] SecretKey { get; set; }
        byte[] HashAndSign(byte[] data);
        void VerifyData(byte[] signedData);
    }
}

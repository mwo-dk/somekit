using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cryptography__.Net_
{
    public interface IECCryptoService
    {
        byte[] PrivateKey { set; }
        byte[] PublicKey { get; }
        byte[] PeerPublicKey { set; }
        Tuple<byte[], byte[]> Encrypt(byte[] data);
        byte[] Decrypt(byte[] data, byte[] iv);
    }
}

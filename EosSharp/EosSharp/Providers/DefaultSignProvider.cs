using Cryptography.ECDSA;
using System.Threading.Tasks;

namespace EosSharp
{
    public class DefaultSignProvider : ISignProvider
    {
        public Task<byte[]> SignTransaction(byte[] trx)
        {
            //Sign message
            //TEST key
            var seckey = Hex.HexToBytes("80f3a375e00cc5147f30bee97bb5d54b31a12eee148a1ac31ac9edc4ecd13bc1f80cc8148e");
            var data = Sha256Manager.GetHash(trx);
            return Task.FromResult(Secp256K1Manager.SignCompressedCompact(data, seckey));
        }
    }
}
using Cryptography.ECDSA;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EosSharp.Helpers;

namespace EosSharp
{
    public class DefaultSignProvider : ISignProvider
    {
        private readonly Dictionary<string, string> Keys = new Dictionary<string, string>();

        public DefaultSignProvider()
        {
            //TEST Wif
            Keys.Add("EOS6MRyAjQq8ud7hVNYcfnVPJqcVpscN5So8BhtHuGYqET5GDW5CV", "5KQwrPbwdL6PhXujxW37FSSQZ1JiwsST4cqQzDeyXtP79zkvFD3");
        }

        public Task<IEnumerable<string>> GetAvailableKeys()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> Sign(string chainId, List<string> requiredKeys, byte[] trxBytes)
        {
            var keyTypeBytes = Encoding.UTF8.GetBytes("K1");
            var data = new List<byte[]>()
            {
                Hex.HexToBytes(chainId),
                trxBytes,
                new byte[32]
            };

            var hash = Sha256Manager.GetHash(SerializationHelper.Combine(data));

            return Task.FromResult(requiredKeys.Select(key => {
                var privKeyBytes = Base58.RemoveCheckSum(Base58.Decode(Keys[key])).Skip(1).ToArray();
                var sign = Secp256K1Manager.SignCompressedCompact(hash, privKeyBytes);
                var check = new List<byte[]>() { sign, keyTypeBytes};
                var checksum = Ripemd160Manager.GetHash(SerializationHelper.Combine(check)).Take(4).ToArray();
                var signAndChecksum = new List<byte[]>() { sign, checksum };

                return "SIG_K1_" + Base58.Encode(SerializationHelper.Combine(signAndChecksum));
            }));
        }
    }
}
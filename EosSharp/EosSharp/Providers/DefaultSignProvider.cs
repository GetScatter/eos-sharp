using Cryptography.ECDSA;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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

        public Task<IEnumerable<string>> Sign(string chainId, List<string> requiredKeys, object transaction)
        {
            var keyTypeBytes = Encoding.UTF8.GetBytes("K1");
            var data = new List<byte>(Hex.HexToBytes(chainId));
            data.AddRange(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(transaction)));
            data.AddRange(new byte[32]);

            var hash = Sha256Manager.GetHash(data.ToArray());

            return Task.FromResult(requiredKeys.Select(key => {
                var privKeyBytes = Base58.RemoveCheckSum(Base58.Decode(Keys[key])).Skip(1).ToArray();
                var sign = Secp256K1Manager.SignCompressedCompact(hash, privKeyBytes);

                var check = new List<byte>(sign);
                check.AddRange(keyTypeBytes);
                var checksum = Ripemd160Manager.GetHash(check.ToArray()).Take(4);

                var signAndChecksum = new List<byte>(sign);
                signAndChecksum.AddRange(checksum);

                return "SIG_K1_" + Base58.Encode(signAndChecksum.ToArray());
            }));
        }
    }
}
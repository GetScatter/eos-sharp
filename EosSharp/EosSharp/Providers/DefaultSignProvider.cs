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
            Keys.Add("EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr", "5K57oSZLpfzePvQNpsLS6NfKXLhhRARNU13q6u2ZPQCGHgKLbTA");
        }

        public DefaultSignProvider(string privateKey)
        {
            //TODO
        }

        public DefaultSignProvider(List<string> privateKeys)
        {
            //TEST TODO
        }

        public DefaultSignProvider(Dictionary<string, string> keys)
        {
            Keys = keys;
        }

        public Task<IEnumerable<string>> GetAvailableKeys()
        {
            return Task.FromResult(Keys.Keys.AsEnumerable());
        }

        public Task<IEnumerable<string>> Sign(string chainId, List<string> requiredKeys, byte[] signBytes)
        {
            var keyTypeBytes = Encoding.UTF8.GetBytes("K1");
            var data = new List<byte[]>()
            {
                Hex.HexToBytes(chainId),
                signBytes,
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
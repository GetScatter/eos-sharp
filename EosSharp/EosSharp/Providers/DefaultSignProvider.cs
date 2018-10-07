using Cryptography.ECDSA;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EosSharp.Helpers;
using System;
using EosSharp.Api.v1;

namespace EosSharp
{
    public class DefaultSignProvider : ISignProvider
    {
        private readonly byte[] KeyTypeBytes = Encoding.UTF8.GetBytes("K1");
        private readonly Dictionary<string, byte[]> Keys = new Dictionary<string, byte[]>();

        public DefaultSignProvider(string privateKey)
        {
            var privKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(privateKey);
            var pubKey = CryptoHelper.PubKeyBytesToString(Secp256K1Manager.GetPublicKey(privKeyBytes, true));
            Keys.Add(pubKey, privKeyBytes);
        }

        public DefaultSignProvider(List<string> privateKeys)
        {
            if (privateKeys == null || privateKeys.Count == 0)
                throw new ArgumentNullException("privateKeys");

            foreach(var key in privateKeys)
            {
                var privKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(key);
                var pubKey = CryptoHelper.PubKeyBytesToString(Secp256K1Manager.GetPublicKey(privKeyBytes, true));
                Keys.Add(pubKey, privKeyBytes);
            }
        }

        public DefaultSignProvider(Dictionary<string, string> encodedKeys)
        {
            if (encodedKeys == null || encodedKeys.Count == 0)
                throw new ArgumentNullException("encodedKeys");

            foreach (var keyPair in encodedKeys)
            {
                var privKeyBytes = CryptoHelper.GetPrivateKeyBytesWithoutCheckSum(keyPair.Value);
                Keys.Add(keyPair.Key, privKeyBytes);
            }
        }

        public DefaultSignProvider(Dictionary<string, byte[]> keys)
        {
            if (keys == null || keys.Count == 0)
                throw new ArgumentNullException("encodedKeys");

            Keys = keys;
        }

        public Task<IEnumerable<string>> GetAvailableKeys()
        {
            return Task.FromResult(Keys.Keys.AsEnumerable());
        }

        public Task<IEnumerable<string>> Sign(string chainId, IEnumerable<string> requiredKeys, byte[] signBytes, IEnumerable<string> abiNames = null)
        {
            var data = new List<byte[]>()
            {
                Hex.HexToBytes(chainId),
                signBytes,
                new byte[32]
            };

            var hash = Sha256Manager.GetHash(SerializationHelper.Combine(data));

            return Task.FromResult(requiredKeys.Select(key =>
            {
                var sign = Secp256K1Manager.SignCompressedCompact(hash, Keys[key]);
                var check = new List<byte[]>() { sign, KeyTypeBytes };
                var checksum = Ripemd160Manager.GetHash(SerializationHelper.Combine(check)).Take(4).ToArray();
                var signAndChecksum = new List<byte[]>() { sign, checksum };

                return "SIG_K1_" + Base58.Encode(SerializationHelper.Combine(signAndChecksum));
            }));
        }
    }
}
using Cryptography.ECDSA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EosSharp.Helpers
{
    public class CryptoHelper
    {
        public static byte[] GetKeyBytes(string key)
        {
            return Base58.RemoveCheckSum(Base58.Decode(key)).Skip(1).ToArray();
        }

        public static string PubKeyBytesToString(byte[] keyBytes, byte[] keyTypeBytes = null, string prefix = "EOS")
        {
            List<byte[]> check = new List<byte[]>()
            {
                keyBytes,
                keyTypeBytes
            };

            var checksum = Ripemd160Manager.GetHash(SerializationHelper.Combine(check)).Take(4).ToArray();
            return prefix + Base58.Encode(SerializationHelper.Combine(new List<byte[]>() {
                keyBytes,
                checksum
            }));
        }
    }
}

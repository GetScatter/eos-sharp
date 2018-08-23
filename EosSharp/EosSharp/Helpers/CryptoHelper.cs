using Cryptography.ECDSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EosSharp.Helpers
{
    public class CryptoHelper
    {
        public static readonly int PUB_KEY_DATA_SIZE = 33;
        public static readonly int PRIV_KEY_DATA_SIZE = 32;
        public static readonly int SIGN_KEY_DATA_SIZE = 64;

        public static byte[] GetKeyBytes(string key)
        {
            return Base58.RemoveCheckSum(Base58.Decode(key));
        }

        public static byte[] GetPrivateKeyBytesWithoutCheckSum(string privateKey)
        {
            //if (privateKey.StartsWith("PVT_R1_"))
            //    return PrivKeyStringToBytes(privateKey).Skip(4).ToArray();
            //else
            //    return PrivKeyStringToBytes(privateKey).Skip(5).ToArray();

            return GetKeyBytes(privateKey).Skip(1).ToArray();
        }

        public static string PubKeyBytesToString(byte[] keyBytes, byte[] keyTypeBytes = null, string prefix = "EOS")
        {
            List<byte[]> check = new List<byte[]>()
            {
                keyBytes,
                keyTypeBytes
            };

            var checksum = Ripemd160Manager.GetHash(SerializationHelper.Combine(check)).Take(4).ToArray();
            return (prefix ?? "") + Base58.Encode(SerializationHelper.Combine(new List<byte[]>() {
                keyBytes,
                checksum
            }));
        }

        public static byte[] PubKeyStringToBytes(string key, string prefix = "EOS")
        {
            if(key.StartsWith("PUB_R1_"))
            {
                return StringToKey(key.Substring(7), PUB_KEY_DATA_SIZE, "R1");
            }
            else
            {
                return StringToKey(key.Substring(prefix.Length), PUB_KEY_DATA_SIZE);
            }
        }

        public static byte[] PrivKeyStringToBytes(string key)
        {
            if (key.StartsWith("PVT_R1_"))
                return StringToKey(key.Substring(7), PRIV_KEY_DATA_SIZE, "R1");
            else
                return StringToKey(key, PRIV_KEY_DATA_SIZE, hasVersion: true);
        }

        public static byte[] SignStringToSignature(string sign)
        {
            if (sign.StartsWith("SIG_K1_"))
                return StringToKey(sign.Substring(7), SIGN_KEY_DATA_SIZE, "K1");
            if (sign.StartsWith("SIG_R1_"))
                return StringToKey(sign.Substring(7), SIGN_KEY_DATA_SIZE, "R1");
            else
                throw new Exception("unrecognized signature format.");
        }

        public static byte[] StringToKey(string key, int size, string suffix = null, bool hasVersion = false) 
        {
            var whole = Base58.Decode(key);
            var keyBytes = whole;
            byte[] digest = null;

            if(!string.IsNullOrWhiteSpace(suffix))
            {
                digest = Ripemd160Manager.GetHash(SerializationHelper.Combine(new List<byte[]>() {
                    keyBytes,
                    Encoding.UTF8.GetBytes(suffix)
                }));
            }
            else
            {
                digest = Sha256Manager.GetHash(Sha256Manager.GetHash(keyBytes));
            }

            //TODO commented for now
            //if (!whole.Skip(size + (hasVersion ? 1 : 0)).SequenceEqual(digest.Take(4)))
            //{
            //    throw new Exception("checksum doesn't match");
            //}
            return keyBytes;
        }
    }
}

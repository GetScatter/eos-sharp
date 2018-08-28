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

        public static byte[] GetPrivateKeyBytesWithoutCheckSum(string privateKey)
        {
            if (privateKey.StartsWith("PVT_R1_"))
                return PrivKeyStringToBytes(privateKey).Take(PRIV_KEY_DATA_SIZE).ToArray();
            else
                return PrivKeyStringToBytes(privateKey).Skip(1).Take(PRIV_KEY_DATA_SIZE).ToArray();
        }

        public static string PubKeyBytesToString(byte[] keyBytes, string keyType = null, string prefix = "EOS")
        {
            return KeyToString(keyBytes, keyType, prefix);
        }

        public static string PrivKeyBytesToString(byte[] keyBytes, string keyType = "R1", string prefix = "PVT_R1_")
        {
            return KeyToString(keyBytes, keyType, prefix);
        }

        public static string SignBytesToString(byte[] signBytes, string keyType = "K1", string prefix = "SIG_K1_")
        {
            return KeyToString(signBytes, keyType, prefix);
        }

        public static byte[] PubKeyStringToBytes(string key, string prefix = "EOS")
        {
            if(key.StartsWith("PUB_R1_"))
            {
                return StringToKey(key.Substring(7), PUB_KEY_DATA_SIZE, "R1");
            }
            else if(key.StartsWith(prefix))
            {
                return StringToKey(key.Substring(prefix.Length), PUB_KEY_DATA_SIZE);
            }
            else
            {
                throw new Exception("unrecognized public key format.");
            }
        }

        public static byte[] PrivKeyStringToBytes(string key)
        {
            if (key.StartsWith("PVT_R1_"))
                return StringToKey(key.Substring(7), PRIV_KEY_DATA_SIZE, "R1");
            else
                return StringToKey(key, PRIV_KEY_DATA_SIZE, "sha256x2");
        }

        public static byte[] SignStringToBytes(string sign)
        {
            if (sign.StartsWith("SIG_K1_"))
                return StringToKey(sign.Substring(7), SIGN_KEY_DATA_SIZE, "K1");
            if (sign.StartsWith("SIG_R1_"))
                return StringToKey(sign.Substring(7), SIGN_KEY_DATA_SIZE, "R1");
            else
                throw new Exception("unrecognized signature format.");
        }

        public static byte[] StringToKey(string key, int size, string keyType = null) 
        {
            var keyBytes = Base58.Decode(key);
            byte[] digest = null;
            int versionSize = 0;

            if(keyType == "sha256x2")
            {
                versionSize = 1;
                digest = Sha256Manager.GetHash(Sha256Manager.GetHash(keyBytes.Take(size + versionSize).ToArray()));
            }
            else if(!string.IsNullOrWhiteSpace(keyType))
            {
                digest = Ripemd160Manager.GetHash(SerializationHelper.Combine(new List<byte[]>() {
                    keyBytes.Take(size).ToArray(),
                    Encoding.UTF8.GetBytes(keyType)
                }));
            }
            else
            {
                digest = Ripemd160Manager.GetHash(keyBytes.Take(size).ToArray());
            }

            if (!keyBytes.Skip(size + versionSize).SequenceEqual(digest.Take(4)))
            {
                throw new Exception("checksum doesn't match.");
            }
            return keyBytes;
        }

        public static string KeyToString(byte[] key, string keyType, string prefix = null)
        {
            byte[] digest = null;

            if (keyType == "sha256x2")
            {
                digest = Sha256Manager.GetHash(Sha256Manager.GetHash(SerializationHelper.Combine(new List<byte[]>() {
                    new byte[] { 128 },
                    key
                })));
            }
            else if (!string.IsNullOrWhiteSpace(keyType))
            {
                digest = Ripemd160Manager.GetHash(SerializationHelper.Combine(new List<byte[]>() {
                    key,
                    Encoding.UTF8.GetBytes(keyType)
                }));
            }
            else
            {
                digest = Ripemd160Manager.GetHash(key);
            }

            return prefix + Base58.Encode(SerializationHelper.Combine(new List<byte[]>() {
                key,
                digest.Take(4).ToArray()
            }));
        }
    }
}

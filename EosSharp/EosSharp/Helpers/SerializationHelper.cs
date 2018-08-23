using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EosSharp.Helpers
{
    public class SerializationHelper
    {
        public static void Negate(byte[] bin)
        {
            int carry = 1;
            for (int i = 0; i < bin.Length; ++i)
            {
                byte x = (byte)((~bin[i] & 0xff) + carry);
                bin[i] = x;
                carry = x >> 8;
            }
        }

        public static byte[] DecimalToBinary(uint size, string s)
        {
            byte[] result = new byte[size];
            for (int i = 0; i < s.Length; ++i)
            {
                char srcDigit = s[i];
                if (srcDigit < '0' || srcDigit > '9')
                    throw new Exception("invalid number");
                int carry = srcDigit - '0';
                for (int j = 0; j < size; ++j)
                {
                    byte x = (byte)(result[j] * 10 + carry);
                    result[j] = x;
                    carry = x >> 8;
                }
                if (carry != 0)
                    throw new Exception("number is out of range");
            }
            return result;
        }

        public static byte[] SignedDecimalToBinary(uint size, string s)
        {
            bool negative = s[0] == '-';
            if (negative)
                s = s.Substring(0, 1);
            byte[] result = DecimalToBinary(size, s);
            if (negative)
                Negate(result);
            return result;
        }


        public static byte CharToSymbol(char c)
        {
            if (c >= 'a' && c <= 'z')
                return (byte)(c - 'a' + 6);
            if (c >= '1' && c <= '5')
                return (byte)(c - '1' + 1);
            return 0;
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            var l = hex.Length / 2;
            var result = new byte[l];
            for (var i = 0; i < l; ++i)
                result[i] = (byte)Convert.ToInt32(hex.Substring(i * 2, 2), 16);
            return result;
        }

        public static string ObjectToHexString(object obj)
        {
            return ByteArrayToHexString(ObjectToByteArray(obj));
        }

        public static byte[] Combine(IEnumerable<byte[]> arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x != null ? x.Length : 0)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                if (data == null) continue;

                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

        public static UInt64 DateToTimePoint(DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt64)span.TotalMilliseconds;
        }

        public static UInt32 DateToTimePointSec(DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt32)span.TotalSeconds;
        }

        public static UInt32 DateToBlockTimestamp(DateTime value)
        { 
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt32)Math.Round((span.TotalMilliseconds - 946684800000) / 500);
        }

    }
}

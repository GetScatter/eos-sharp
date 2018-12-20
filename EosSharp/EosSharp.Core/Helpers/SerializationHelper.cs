using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EosSharp.Helpers
{
    public class SerializationHelper
    {
        public static bool IsNegative(byte[] bin)
        {
            return (bin[bin.Length - 1] & 0x80) != 0;
        }

        public static void Negate(byte[] bin)
        {
            int carry = 1;
            for (int i = 0; i < bin.Length; ++i)
            {
                int x = (~bin[i] & 0xff) + carry;
                bin[i] = (byte)x;
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
                    int x = result[j] * 10 + carry;
                    result[j] = (byte)x;
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

        public static string BinaryToDecimal(byte[] bin, int minDigits = 1)
        {
            var result = new List<char>(minDigits);

            for (int i = 0; i < minDigits; i++)
            {
                result.Add('0');
            }

            for (int i = bin.Length - 1; i >= 0; --i)
            {
                int carry = bin[i];
                for (int j = 0; j < result.Count; ++j)
                {
                    int x = ((result[j] - '0') << 8) + carry;
                    result[j] = (char)('0' + (x % 10));
                    carry = (x / 10) | 0;
                }
                while (carry != 0)
                {
                    result.Add((char)('0' + carry % 10));
                    carry = (carry / 10) | 0;
                }
            }
            result.Reverse();
            return string.Join("", result);
        }

        public static string SignedBinaryToDecimal(byte[] bin, int minDigits = 1)
        {
            if (IsNegative(bin))
            {
                Negate(bin);
                return '-' + BinaryToDecimal(bin, minDigits);
            }
            return BinaryToDecimal(bin, minDigits);
        }

        public static byte[] Base64FcStringToByteArray(string s)
        {
            //fc adds extra '='
            if((s.Length & 3) == 1 && s[s.Length - 1] == '=')
            {
                return Convert.FromBase64String(s.Substring(0, s.Length - 1));
            }

            return Convert.FromBase64String(s);
        }

        public static byte CharToSymbol(char c)
        {
            if (c >= 'a' && c <= 'z')
                return (byte)(c - 'a' + 6);
            if (c >= '1' && c <= '5')
                return (byte)(c - '1' + 1);
            return 0;
        }

        public static string SnakeCaseToPascalCase(string s)
        {
            var result = s.ToLower().Replace("_", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            result = info.ToTitleCase(result).Replace(" ", string.Empty);
            return result;
        }

        public static string PascalCaseToSnakeCase(string s)
        {
            var builder = new StringBuilder();
            bool first = true;
            foreach(var c in s)
            {
                if(char.IsUpper(c))
                {
                    if (!first)
                        builder.Append('_');
                    builder.Append(char.ToLower(c));
                }
                else
                {
                    builder.Append(c);
                }

                if (first)
                    first = false;
            }
            return builder.ToString();
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

        public static DateTime TimePointToDate(long ticks)
        {
            return new DateTime(ticks + new DateTime(1970, 1, 1).Ticks);
        }

        public static UInt32 DateToTimePointSec(DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt32)span.TotalSeconds;
        }

        public static DateTime TimePointSecToDate(UInt32 secs)
        {
            return new DateTime(secs * 1000 + new DateTime(1970, 1, 1).Ticks);
        }

        public static UInt32 DateToBlockTimestamp(DateTime value)
        { 
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt32)Math.Round((span.TotalMilliseconds - 946684800000) / 500);
        }

        public static DateTime BlockTimestampToDate(UInt32 slot)
        {
            return new DateTime(slot * 500 + 946684800000 + new DateTime(1970, 1, 1).Ticks);
        }
    }
}

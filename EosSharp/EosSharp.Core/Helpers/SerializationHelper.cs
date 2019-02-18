using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EosSharp.Core.Helpers
{
    public class SerializationHelper
    {
        /// <summary>
        /// Is a big Number negative
        /// </summary>
        /// <param name="bin">big number in byte array</param>
        /// <returns></returns>
        public static bool IsNegative(byte[] bin)
        {
            return (bin[bin.Length - 1] & 0x80) != 0;
        }

        /// <summary>
        /// Negate a big number
        /// </summary>
        /// <param name="bin">big number in byte array</param>
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

        /// <summary>
        /// Convert an unsigned decimal number as string to a big number
        /// </summary>
        /// <param name="size">Size in bytes of the big number</param>
        /// <param name="s">decimal encoded as string</param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert an signed decimal number as string to a big number
        /// </summary>
        /// <param name="size">Size in bytes of the big number</param>
        /// <param name="s">decimal encoded as string</param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert big number to an unsigned decimal number
        /// </summary>
        /// <param name="bin">big number as byte array</param>
        /// <param name="minDigits">0-pad result to this many digits</param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert big number to an signed decimal number
        /// </summary>
        /// <param name="bin">big number as byte array</param>
        /// <param name="minDigits">0-pad result to this many digits</param>
        /// <returns></returns>
        public static string SignedBinaryToDecimal(byte[] bin, int minDigits = 1)
        {
            if (IsNegative(bin))
            {
                Negate(bin);
                return '-' + BinaryToDecimal(bin, minDigits);
            }
            return BinaryToDecimal(bin, minDigits);
        }

        /// <summary>
        /// Convert base64 with fc prefix to byte array
        /// </summary>
        /// <param name="s">string to convert</param>
        /// <returns></returns>
        public static byte[] Base64FcStringToByteArray(string s)
        {
            //fc adds extra '='
            if((s.Length & 3) == 1 && s[s.Length - 1] == '=')
            {
                return Convert.FromBase64String(s.Substring(0, s.Length - 1));
            }

            return Convert.FromBase64String(s);
        }

        /// <summary>
        /// Convert ascii char to symbol value
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static byte CharToSymbol(char c)
        {
            if (c >= 'a' && c <= 'z')
                return (byte)(c - 'a' + 6);
            if (c >= '1' && c <= '5')
                return (byte)(c - '1' + 1);
            return 0;
        }

        /// <summary>
        /// Convert snake case string to pascal case
        /// </summary>
        /// <param name="s">string to convert</param>
        /// <returns></returns>
        public static string SnakeCaseToPascalCase(string s)
        {
            var result = s.ToLower().Replace("_", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            result = info.ToTitleCase(result).Replace(" ", string.Empty);
            return result;
        }

        /// <summary>
        /// Convert pascal case string to snake case
        /// </summary>
        /// <param name="s">string to convert</param>
        /// <returns></returns>
        public static string PascalCaseToSnakeCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

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

        /// <summary>
        /// Serialize object to byte array
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns></returns>
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

        /// <summary>
        /// Encode byte array to hexadecimal string
        /// </summary>
        /// <param name="ba">byte array to convert</param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        /// <summary>
        /// Decode hexadecimal string to byte array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hex)
        {
            var l = hex.Length / 2;
            var result = new byte[l];
            for (var i = 0; i < l; ++i)
                result[i] = (byte)Convert.ToInt32(hex.Substring(i * 2, 2), 16);
            return result;
        }

        /// <summary>
        /// Serialize object to hexadecimal encoded string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToHexString(object obj)
        {
            return ByteArrayToHexString(ObjectToByteArray(obj));
        }

        /// <summary>
        /// Combina multiple arrays into one
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert DateTime to `time_point` (miliseconds since epoch)
        /// </summary>
        /// <param name="value">date to convert</param>
        /// <returns></returns>
        public static UInt64 DateToTimePoint(DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt64)(span.Ticks / TimeSpan.TicksPerMillisecond);
        }

        /// <summary>
        /// Convert `time_point` (miliseconds since epoch) to DateTime
        /// </summary>
        /// <param name="ticks">time_point ticks to convert</param>
        /// <returns></returns>
        public static DateTime TimePointToDate(long ticks)
        {
            return new DateTime(ticks + new DateTime(1970, 1, 1).Ticks);
        }

        /// <summary>
        /// Convert DateTime to `time_point_sec` (seconds since epoch)
        /// </summary>
        /// <param name="value">date to convert</param>
        /// <returns></returns>
        public static UInt32 DateToTimePointSec(DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt32)((span.Ticks / TimeSpan.TicksPerSecond) & 0xffffffff);
        }

        /// <summary>
        /// Convert `time_point_sec` (seconds since epoch) to DateTime
        /// </summary>
        /// <param name="secs">time_point_sec to convert</param>
        /// <returns></returns>
        public static DateTime TimePointSecToDate(UInt32 secs)
        {
            return new DateTime(secs * TimeSpan.TicksPerSecond + new DateTime(1970, 1, 1).Ticks);
        }

        /// <summary>
        /// Convert DateTime to `block_timestamp_type` (half-seconds since a different epoch)
        /// </summary>
        /// <param name="value">date to convert</param>
        /// <returns></returns>
        public static UInt32 DateToBlockTimestamp(DateTime value)
        { 
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt32)((UInt64)Math.Round((double)(span.Ticks / TimeSpan.TicksPerMillisecond - 946684800000) / 500) & 0xffffffff);
        }

        /// <summary>
        /// Convert `block_timestamp_type` (half-seconds since a different epoch) to DateTime
        /// </summary>
        /// <param name="slot">block_timestamp slot to convert</param>
        /// <returns></returns>
        public static DateTime BlockTimestampToDate(UInt32 slot)
        {
            return new DateTime(slot * TimeSpan.TicksPerMillisecond * 500 + 946684800000 + new DateTime(1970, 1, 1).Ticks);
        }
    }
}

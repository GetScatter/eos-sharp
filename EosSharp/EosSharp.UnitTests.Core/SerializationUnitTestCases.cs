using EosSharp.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EosSharp.UnitTests
{
    public class SerializationUnitTestCases
    {
        public void DoubleSerialization()
        {
            string doubleStr = "10001";
            var doubleBytes = SerializationHelper.DecimalToBinary(8, doubleStr);
            var doubleBytes2 = BitConverter.GetBytes(Convert.ToUInt64(doubleStr));
            var doubleResStr = SerializationHelper.BinaryToDecimal(doubleBytes);
        }

        public void DecimalSerialization()
        {
            string decimalStr = "10001";
            var decimalBytes = SerializationHelper.DecimalToBinary(16, decimalStr);

            Int32[] bits = decimal.GetBits(Convert.ToDecimal(decimalStr));
            List<byte> bytes = new List<byte>();
            foreach (Int32 i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }
            var decimalBytes2 = bytes.ToArray();

            var decimalResStr = SerializationHelper.BinaryToDecimal(decimalBytes);
        }
    }
}

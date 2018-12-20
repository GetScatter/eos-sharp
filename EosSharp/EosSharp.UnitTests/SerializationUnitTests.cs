using EosSharp.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EosSharp.UnitTests
{
    [TestClass]
    public class SerializationUnitTests
    {
        [TestMethod]
        [TestCategory("Serialization Tests")]
        public void DoubleSerialization()
        {
            string doubleStr = "10001";
            var doubleBytes = SerializationHelper.DecimalToBinary(8, doubleStr);
            var doubleBytes2 = BitConverter.GetBytes(Convert.ToUInt64(doubleStr));
            var doubleResStr = SerializationHelper.BinaryToDecimal(doubleBytes);
            Assert.IsTrue(doubleStr == doubleResStr && doubleBytes.SequenceEqual(doubleBytes2));
        }

        [TestMethod]
        [TestCategory("Serialization Tests")]
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
            Assert.IsTrue(decimalStr == decimalResStr && decimalBytes.SequenceEqual(decimalBytes2));
        }
    }
}

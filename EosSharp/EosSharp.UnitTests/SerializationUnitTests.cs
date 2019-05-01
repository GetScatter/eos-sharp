// Auto Generated, do not edit.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace EosSharp.UnitTests
{
    [TestClass]
    public class SerializationUnitTests
    {
        SerializationUnitTestCases SerializationUnitTestCases;
        public SerializationUnitTests()
        {
            SerializationUnitTestCases = new SerializationUnitTestCases();
        }

		[TestMethod]
        [TestCategory("Serialization Tests")]
        public void DoubleSerialization()
        {
            bool success = false;
            try
            {
                SerializationUnitTestCases.DoubleSerialization();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Serialization Tests")]
        public void DecimalSerialization()
        {
            bool success = false;
            try
            {
                SerializationUnitTestCases.DecimalSerialization();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
	}
}
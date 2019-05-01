// Auto Generated, do not edit.
using Newtonsoft.Json;
using System;

namespace EosSharp.UnitTests.Unity3D
{
    public class SerializationUnitTests
    {
        SerializationUnitTestCases SerializationUnitTestCases;
        public SerializationUnitTests()
        {
            SerializationUnitTestCases = new SerializationUnitTestCases();
        }

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

            if(success)
				Console.WriteLine("Test DoubleSerialization run successfuly.");
			else
				Console.WriteLine("Test DoubleSerialization run failed.");
        }
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

            if(success)
				Console.WriteLine("Test DecimalSerialization run successfuly.");
			else
				Console.WriteLine("Test DecimalSerialization run failed.");
        }

		public void TestAll()
        {
			DoubleSerialization();
			DecimalSerialization();
        }
	}
}
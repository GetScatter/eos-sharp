
// Auto Generated, do not edit.
using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EosSharp.UnitTests
{
    [TestClass]
    public class EosUnitTests
    {
        EosUnitTestCases EosUnitTestCases;
        public EosUnitTests()
        {
            var eosConfig = new EosConfigurator()
            {
                SignProvider = new DefaultSignProvider("5K57oSZLpfzePvQNpsLS6NfKXLhhRARNU13q6u2ZPQCGHgKLbTA"),

                //HttpEndpoint = "https://nodes.eos42.io", //Mainnet
                //ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                //HttpEndpoint = "https://nodeos01.btuga.io",
                //ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"

				HttpEndpoint = "http://localhost:8888",
                ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            };
            var eosApi = new EosApi(eosConfig, new HttpHandler());

            EosUnitTestCases = new EosUnitTestCases(new Eos(eosConfig));
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task GetBlock()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.GetBlock();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task GetTableRows()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.GetTableRows();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task GetTableRowsGeneric()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.GetTableRowsGeneric();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task GetProducers()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.GetProducers();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task GetScheduledTransactions()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.GetScheduledTransactions();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task CreateTransactionArrayData()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.CreateTransactionArrayData();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task CreateTransactionAnonymousObjectData()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.CreateTransactionAnonymousObjectData();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task CreateTransaction()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.CreateTransaction();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Eos Tests")]
        public async Task CreateNewAccount()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.CreateNewAccount();
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
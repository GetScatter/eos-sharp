
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
    public class ApiUnitTests
    {
        ApiUnitTestCases ApiUnitTestCases;
        public ApiUnitTests()
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

            ApiUnitTestCases = new ApiUnitTestCases(eosConfig, eosApi);
        }

		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetInfo()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetInfo();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetAccount()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetAccount();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetCode()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetCode();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetAbi()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetAbi();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetRawCodeAndAbi()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetRawCodeAndAbi();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetRawAbi()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetRawAbi();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task AbiJsonToBin()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.AbiJsonToBin();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task AbiBinToJson()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.AbiBinToJson();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetRequiredKeys()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetRequiredKeys();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetBlock()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetBlock();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetBlockHeaderState()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetBlockHeaderState();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetTableRows()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetTableRows();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetTableByScope()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetTableByScope();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetCurrencyBalance()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetCurrencyBalance();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetCurrencyStats()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetCurrencyStats();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetProducers()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetProducers();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetProducerSchedule()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetProducerSchedule();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetScheduledTransactions()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetScheduledTransactions();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task PushTransaction()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.PushTransaction();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetActions()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetActions();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetTransaction()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetTransaction();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetKeyAccounts()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetKeyAccounts();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            Assert.IsTrue(success);
        }
		[TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetControlledAccounts()
        {
            bool success = false;
            try
            {
                await ApiUnitTestCases.GetControlledAccounts();
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
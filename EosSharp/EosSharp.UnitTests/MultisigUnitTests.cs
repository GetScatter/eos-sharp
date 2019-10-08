using Cryptography.ECDSA;
using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Helpers;
using EosSharp.Core.Interfaces;
using EosSharp.Core.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EosSharp.UnitTests
{
    [TestClass]
    public class MultisigUnitTests
    {
        EosUnitTestCases EosUnitTestCases;
        public MultisigUnitTests()
        {
            var eosConfig = new EosConfigurator()
            {
                SignProvider = new CombinedSignersProvider(new List<ISignProvider>() {
                    new DefaultSignProvider("5KQwrPbwdL6PhXujxW37FSSQZ1JiwsST4cqQzDeyXtP79zkvFD3"),
                    new DefaultSignProvider("5JjWBn4DKVPe7DSXXXK852CQeEVBQjyqW9s7vbzXAQqxLxca5Hz")
                }),

                //HttpEndpoint = "https://nodes.eos42.io", //Mainnet
                //ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                HttpEndpoint = "https://jungle2.cryptolions.io",
                ChainId = "e70aaab8997e1dfce58fbfac80cbbb8fecec7b99cf982a9444273cbc64c41473"

                //HttpEndpoint = "http://localhost:8888",
                //ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            };
            EosUnitTestCases = new EosUnitTestCases(new Eos(eosConfig));
        }

        [TestMethod]
        [TestCategory("Multisig Tests")]
        public async Task CreateTransaction2ProvidersAsync()
        {
            bool success = false;
            try
            {
                await EosUnitTestCases.CreateTransaction2Providers();
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

using EosSharp.Api.v1;
using EosSharp.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EosSharp.UnitTests
{
    [TestClass]
    public class EosUnitTests
    {
        Eos Eos { get; set; }
        public EosUnitTests()
        {
            Eos = new Eos(new EosConfigurator()
            {
                SignProvider = new DefaultSignProvider("5K57oSZLpfzePvQNpsLS6NfKXLhhRARNU13q6u2ZPQCGHgKLbTA"),

                HttpEndpoint = "https://api.eossweden.se", //Mainnet
                ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                //HttpEndpoint = "https://nodeos01.btuga.io",
                //ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            });
        }

        [TestMethod]
        [TestCategory("Eos Tests")]
        public async Task GetBlock()
        {
            bool success = false;
            try
            {
                var result = await Eos.GetBlock("13503532");
                success = true;
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.StatusCode);
                Console.WriteLine(ex.Content);
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
                var result = await Eos.GetTableRows(new GetTableRowsRequest()
                {
                    Json = false,
                    Code = "eosio",
                    Scope = "eosio",
                    Table = "producers"
                });
                success = true;
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.StatusCode);
                Console.WriteLine(ex.Content);
            }

            Assert.IsTrue(success);
        }

        class Stat
        {
            [JsonProperty("issuer")]
            public string Issuer { get; set; }
            [JsonProperty("max_supply")]
            public string MaxSupply { get; set; }
            [JsonProperty("supply")]
            public string Supply { get; set; }
        }

        [TestMethod]
        [TestCategory("Eos Tests")]
        public async Task GetTableRowsGeneric()
        {
            bool success = false;
            try
            {
                var result = await Eos.GetTableRows<Stat>(new GetTableRowsRequest()
                {
                    Json = true,
                    Code = "eosio.token",
                    Scope = "EOS",
                    Table = "stat"
                });
                success = true;
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.StatusCode);
                Console.WriteLine(ex.Content);
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
                var result = await Eos.CreateTransaction(new Transaction()
                {
                    Actions = new List<Api.v1.Action>()
                    {
                        new Api.v1.Action()
                        {
                            Account = "eosio.token",
                            Authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() {Actor = "tester112345", Permission = "active" }
                            },
                            Name = "transfer",
                            Data = new { from = "tester112345", to = "tester212345", quantity = "0.0001 EOS", memo = "hello crypto world!" }
                        }
                    }
                });
                success = true;
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.StatusCode);
                Console.WriteLine(ex.Content);
            }

            Assert.IsTrue(success);
        }
    }
}

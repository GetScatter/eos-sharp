using EosSharp.Api.v1;
using EosSharp.Providers;
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
    public class SignUnitTests
    {
        readonly EosConfigurator EosConfig = null;
        EosApi DefaultApi { get; set; }
        public SignUnitTests()
        {
            EosConfig = new EosConfigurator()
            {
                SignProvider = new DefaultSignProvider("5K57oSZLpfzePvQNpsLS6NfKXLhhRARNU13q6u2ZPQCGHgKLbTA"),

                //HttpEndpoint = "https://nodes.eos42.io", //Mainnet
                //ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                HttpEndpoint = "https://nodeos01.btuga.io",
                ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            };
            DefaultApi = new EosApi(EosConfig);
        }

        [TestMethod]
        [TestCategory("Signature Tests")]
        public async Task SignProvider()
        {
            var signProvider = new DefaultSignProvider("5K57oSZLpfzePvQNpsLS6NfKXLhhRARNU13q6u2ZPQCGHgKLbTA");
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
            
            Assert.IsTrue((await signProvider.GetAvailableKeys()).All(ak => requiredKeys.Contains(ak)));
        }

        [TestMethod]
        [TestCategory("Signature Tests")]
        public async Task SignHelloWorld()
        {
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
            var helloBytes = Encoding.UTF8.GetBytes("Hello world!");
            var signatures = await EosConfig.SignProvider.Sign(DefaultApi.Config.ChainId, requiredKeys, helloBytes);

            Assert.IsTrue(signatures.First() == "SIG_K1_JxtwrzV246xdAgqgH36oX5MjMeg1sEFdUWuwnE9Fhr9eqi5JzgmKXm9UEJgNZMLYdnZhphL1QmE8aW7rTDPC8k8acvkoMR");
        }

        [TestMethod]
        [TestCategory("Signature Tests")]
        public async Task SignTransaction()
        {
            var trx = new Transaction()
            {
                // trx info
                MaxNetUsageWords = 0,
                MaxCpuUsageMs = 0,
                DelaySec = 0,
                ContextFreeActions = new List<Api.v1.Action>(),
                TransactionExtensions = new List<Extension>(),
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
                        Data = new { from = "tester112345", to = "tester212345", quantity = "1.0000 EOS", memo = "hello crypto world!" }
                    }
                }
            };

            var abiSerializer = new AbiSerializationProvider(DefaultApi);
            var packedTrx = await abiSerializer.SerializePackedTransaction(trx);
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
            var signatures = await EosConfig.SignProvider.Sign(DefaultApi.Config.ChainId, requiredKeys, packedTrx);

            Assert.IsTrue(signatures.First() == "SIG_K1_K9atRrkPT67BSEdiEqCDvaokWJKXwFRYXni98zMGosvNM1kb9WpNNn1SQe5S8MaKqe495gUbLVnbAH7oLiEN8LoaMG9i3p");
        }
    }
}

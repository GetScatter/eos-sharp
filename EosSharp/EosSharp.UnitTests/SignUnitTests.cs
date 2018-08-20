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
        EosApi DefaultApi { get; set; }
        public SignUnitTests()
        {
            DefaultApi = new EosApi(new EosConfigurator()
            {
                //HttpEndpoint = "https://nodes.eos42.io", //Mainnet
                //ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                HttpEndpoint = "https://nodeos01.btuga.io",
                ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            });
        }

        [TestMethod]
        public async Task SignHelloWorld()
        {
            var signProvider = new DefaultSignProvider();
            var chainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f";
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
            var helloBytes = Encoding.UTF8.GetBytes("Hello world!");
            var signatures = await signProvider.Sign(chainId, requiredKeys, helloBytes);

            Assert.IsTrue(signatures.First() == "SIG_K1_JxtwrzV246xdAgqgH36oX5MjMeg1sEFdUWuwnE9Fhr9eqi5JzgmKXm9UEJgNZMLYdnZhphL1QmE8aW7rTDPC8k8acvkoMR");
        }

        [TestMethod]
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
            var signProvider = new DefaultSignProvider();
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
            var signatures = await signProvider.Sign(DefaultApi.Config.ChainId, requiredKeys, packedTrx);

            Console.WriteLine(JsonConvert.SerializeObject(signatures));

            Assert.IsTrue(true);
        }
    }
}

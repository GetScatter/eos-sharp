using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using EosSharp.Api.v1;
using EosSharp.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using EosSharp.Helpers;
using EosSharp.Providers;

namespace EosSharp.UnitTests
{
    [TestClass]
    public class ApiUnitTests
    {
        EosApi DefaultApi { get; set; }
        public ApiUnitTests()
        {
            DefaultApi = new EosApi(new EosConfigurator()
            {
                //HttpEndpoint = "https://nodeos01.btuga.io",
                HttpEndpoint = "https://nodes.eos42.io", //Mainnet
                ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"
            });
        }

        [TestMethod]
        public async Task GetInfo()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetInfo();
                success = true;
            }
            catch(ApiException ex)
            {
                Console.WriteLine(ex.StatusCode);
                Console.WriteLine(ex.Content);
            }

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task GetAccount()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetAccount(new GetAccountRequest() {
                    AccountName = "eosio"
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
        public async Task GetCode()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetCode(new GetCodeRequest()
                {
                    AccountName = "eosio.token",
                    CodeAsWasm = true
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
        public async Task GetAbi()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetAbi(new GetAbiRequest() {
                    AccountName = "eosio.token"
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
        public async Task GetRawCodeAndAbi()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetRawCodeAndAbi(new GetRawCodeAndAbiRequest() {
                    AccountName = "eosio.token"
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
        public async Task AbiJsonToBin()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.AbiJsonToBin(new AbiJsonToBinRequest() {
                    Code = "eosio.token",
                    Action = "transfer",
                    Args = new { from = "eosio", to = "eosio.names", quantity = "1.0000 EOS", memo = "hello crypto world!" }
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
        public async Task AbiBinToJson()
        {
            bool success = false;
            try
            {
                var binArgsResult = await DefaultApi.AbiJsonToBin(new AbiJsonToBinRequest()
                {
                    Code = "eosio.token",
                    Action = "transfer",
                    Args = new { from = "eosio", to = "eosio.names", quantity = "1.0000 EOS", memo = "hello crypto world!" }
                });

                var result = await DefaultApi.AbiBinToJson(new AbiBinToJsonRequest()
                {
                    Code = "eosio.token",
                    Action = "transfer",
                    Binargs = binArgsResult.Binargs
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

        //TODO add inputs and types
        [TestMethod]
        public async Task GetRequiredKeys()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetRequiredKeys(new GetRequiredKeysRequest()
                {
                    AvailableKeys = "",
                    Transaction = ""
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

        //TODO add missing types
        [TestMethod]
        public async Task GetBlock()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetBlock(new GetBlockRequest() {
                    BlockNumOrId = "1"
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

        //TODO add missing types
        [TestMethod]
        public async Task GetBlockHeaderState()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetBlockHeaderState(new GetBlockHeaderStateRequest()
                {
                    BlockNumOrId = "1"
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

        //TODO check implementation for json/binary
        [TestMethod]
        public async Task GetTableRows()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetTableRows(new GetTableRowsRequest() {
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
        public async Task GetCurrencyBalance()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetCurrencyBalance(new GetCurrencyBalanceRequest()
                {
                     Code = "eosio.token",
                     Account = "tester112345",
                     Symbol = "EOS"
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
        public async Task GetCurrencyStats()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetCurrencyStats(new GetCurrencyStatsRequest()
                {
                    Code = "eosio.token",
                    Symbol = "EOS"
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

        //TODO check implementation for json/binary
        [TestMethod]
        public async Task GetProducers()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetProducers(new GetProducersRequest()
                {
                    Json = true,                    
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
        public async Task GetProducerSchedule()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetProducerSchedule();
                success = true;
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.StatusCode);
                Console.WriteLine(ex.Content);
            }

            Assert.IsTrue(success);
        }

        //TODO check implementation for json/binary
        [TestMethod]
        public async Task GetScheduledTransactions()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetScheduledTransactions(new GetScheduledTransactionsRequest() {
                    Json = true
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

        //TODO add inputs and types
        [TestMethod]
        public async Task PushTransaction()
        {
            bool success = false;
            try
            {
                var trx = new Transaction()
                {
                    Expiration = DateTime.Now,
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
                                new PermissionLevel() {Actor = "eosio", Permission = "active" }
                            },
                            Name = "transfer",
                            Data = new { from = "eosio", to = "eosio.token", quantity = "1.0000 EOS", memo = "hello crypto world!" }
                        }
                    }
                };

                var signProvider = new DefaultSignProvider();
                var abiSerializer = new AbiSerializationProvider();
                var requiredKeys = new List<string>() { "EOS6MRyAjQq8ud7hVNYcfnVPJqcVpscN5So8BhtHuGYqET5GDW5CV" };
                var packedTrx = abiSerializer.SerializePackedTransaction(trx);
                var signs = await signProvider.Sign(DefaultApi.Config.ChainId, requiredKeys, packedTrx);

                var result = await DefaultApi.PushTransaction(new PushTransactionRequest()
                {
                    Signatures = signs.ToArray(),
                    Compression = 0,
                    PackedContextFreeData = "",
                    PackedTrx = SerializationHelper.ByteArrayToHexString(packedTrx)
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
        public async Task GetActions()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetActions(new GetActionsRequest() {
                    AccountName = "eosio"
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

        //TODO add id
        [TestMethod]
        public async Task GetTransaction()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetTransaction(new GetTransactionRequest()
                {
                    Id = ""
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
        public async Task GetKeyAccounts()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetKeyAccounts(new GetKeyAccountsRequest()
                {
                    PublicKey = "EOS6MRyAjQq8ud7hVNYcfnVPJqcVpscN5So8BhtHuGYqET5GDW5CV"
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
        public async Task GetControlledAccounts()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetControlledAccounts(new GetControlledAccountsRequest()
                {
                    ControllingAccount = "eosio"
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

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
        readonly EosConfigurator EosConfig = null;
        EosApi DefaultApi { get; set; }
        public ApiUnitTests()
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
        public async Task GetRawCodeAndAbi()
        {
            bool success = false;
            try
            {
                var result = await DefaultApi.GetRawCodeAndAbi(new GetRawCodeAndAbiRequest() {
                    AccountName = "eosio.token"
                });

                var wasmBytes = SerializationHelper.Base64FcStringToByteArray(result.Wasm);
                var abiBytes = SerializationHelper.Base64FcStringToByteArray(result.Abi);

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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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

        [TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetRequiredKeys()
        {
            bool success = false;
            try
            {
                var getInfoResult = await DefaultApi.GetInfo();
                var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
                {
                    BlockNumOrId = getInfoResult.LastIrreversibleBlockNum.Value.ToString()
                });

                var trx = new Transaction()
                {
                    //trx headers
                    Expiration = getInfoResult.HeadBlockTime.Value.AddSeconds(60), //expire Seconds
                    RefBlockNum = (UInt16)(getInfoResult.LastIrreversibleBlockNum.Value & 0xFFFF),
                    RefBlockPrefix = getBlockResult.RefBlockPrefix,
                    // trx info
                    MaxNetUsageWords = 0,
                    MaxCpuUsageMs = 0,
                    DelaySec = 0,
                    ContextFreeActions = new List<Api.v1.Action>(),
                    
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
                    },
                    TransactionExtensions = new List<Extension>()
                };

                int actionIndex = 0;
                var abiSerializer = new AbiSerializationProvider(DefaultApi);
                var abiResponses = await abiSerializer.GetTransactionAbis(trx);

                foreach (var action in trx.ContextFreeActions)
                {
                    action.Data = SerializationHelper.ByteArrayToHexString(abiSerializer.SerializeActionData(action, abiResponses[actionIndex++].Abi));
                }

                foreach (var action in trx.Actions)
                {
                    action.Data = SerializationHelper.ByteArrayToHexString(abiSerializer.SerializeActionData(action, abiResponses[actionIndex++].Abi));
                }

                var getRequiredResult = await DefaultApi.GetRequiredKeys(new GetRequiredKeysRequest()
                {
                    AvailableKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" },
                    Transaction = trx
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
        [TestCategory("Api Tests")]
        public async Task GetBlock()
        {
            bool success = false;
            try
            {
                var getInfoResult = await DefaultApi.GetInfo();
                var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
                {
                    BlockNumOrId = getInfoResult.LastIrreversibleBlockNum.Value.ToString()
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
        [TestCategory("Api Tests")]
        public async Task GetBlockHeaderState()
        {
            bool success = false;
            try
            {
                var getInfoResult = await DefaultApi.GetInfo();
                var result = await DefaultApi.GetBlockHeaderState(new GetBlockHeaderStateRequest()
                {
                    BlockNumOrId = getInfoResult.LastIrreversibleBlockNum.Value.ToString()
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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

        [TestMethod]
        [TestCategory("Api Tests")]
        public async Task PushTransaction()
        {
            bool success = false;
            try
            {
                await CreateTransaction();

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
        [TestCategory("Api Tests")]
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

        [TestMethod]
        [TestCategory("Api Tests")]
        public async Task GetTransaction()
        {
            bool success = false;
            try
            {
                var trxResult = await CreateTransaction();

                var result = await DefaultApi.GetTransaction(new GetTransactionRequest()
                {
                    Id = trxResult.TransactionId
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
        [TestCategory("Api Tests")]
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
        [TestCategory("Api Tests")]
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

        private async Task<PushTransactionResponse> CreateTransaction()
        {
            var getInfoResult = await DefaultApi.GetInfo();
            var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
            {
                BlockNumOrId = getInfoResult.LastIrreversibleBlockNum.Value.ToString()
            });


            var trx = new Transaction()
            {
                //trx headers
                Expiration = getInfoResult.HeadBlockTime.Value.AddSeconds(60), //expire Seconds
                RefBlockNum = (UInt16)(getInfoResult.LastIrreversibleBlockNum.Value & 0xFFFF),
                RefBlockPrefix = getBlockResult.RefBlockPrefix,
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
                            Data = new { from = "tester112345", to = "tester212345", quantity = "0.0001 EOS", memo = "hello crypto world!" }
                        }
                    }
            };

            var abiSerializer = new AbiSerializationProvider(DefaultApi);
            var packedTrx = await abiSerializer.SerializePackedTransaction(trx);
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
            var signatures = await EosConfig.SignProvider.Sign(DefaultApi.Config.ChainId, requiredKeys, packedTrx);

            return await DefaultApi.PushTransaction(new PushTransactionRequest()
            {
                Signatures = signatures.ToArray(),
                Compression = 0,
                PackedContextFreeData = "",
                PackedTrx = SerializationHelper.ByteArrayToHexString(packedTrx)
            });
        }
    }
}

using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Helpers;
using EosSharp.Core.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            DefaultApi = new EosApi(EosConfig, new HttpHelper());
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
                var result = await DefaultApi.GetAccount(new GetAccountRequest() {
                    account_name = "eosio"
                });
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
                var result = await DefaultApi.GetCode(new GetCodeRequest()
                {
                    account_name = "eosio.token",
                    code_as_wasm = true
                });
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
                var result = await DefaultApi.GetAbi(new GetAbiRequest() {
                    account_name = "eosio.token"
                });
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
                var result = await DefaultApi.GetRawCodeAndAbi(new GetRawCodeAndAbiRequest() {
                    account_name = "eosio.token"
                });

                var abiSerializer = new AbiSerializationProvider(DefaultApi);
                var abiObject = abiSerializer.DeserializePackedAbi(result.abi);

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
                var result = await DefaultApi.GetRawAbi(new GetRawAbiRequest()
                {
                    account_name = "eosio.token"
                });

                var abiSerializer = new AbiSerializationProvider(DefaultApi);
                var abiObject = abiSerializer.DeserializePackedAbi(result.abi);

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
                var result = await DefaultApi.AbiJsonToBin(new AbiJsonToBinRequest() {
                    code = "eosio.token",
                    action = "transfer",
                    args = new { from = "eosio", to = "eosio.names", quantity = "1.0000 EOS", memo = "hello crypto world!" }
                });
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
                var binArgsResult = await DefaultApi.AbiJsonToBin(new AbiJsonToBinRequest()
                {
                    code = "eosio.token",
                    action = "transfer",
                    args = new { from = "eosio", to = "eosio.names", quantity = "1.0000 EOS", memo = "hello crypto world!" }
                });

                var result = await DefaultApi.AbiBinToJson(new AbiBinToJsonRequest()
                {
                    code = "eosio.token",
                    action = "transfer",
                    binargs = binArgsResult.binargs
                });
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
                var getInfoResult = await DefaultApi.GetInfo();
                var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
                {
                    block_num_or_id = getInfoResult.last_irreversible_block_num.ToString()
                });

                var trx = new Transaction()
                {
                    //trx headers
                    expiration = getInfoResult.head_block_time.AddSeconds(60), //expire Seconds
                    ref_block_num = (UInt16)(getInfoResult.last_irreversible_block_num & 0xFFFF),
                    ref_block_prefix = getBlockResult.ref_block_prefix,
                    // trx info
                    max_net_usage_words = 0,
                    max_cpu_usage_ms = 0,
                    delay_sec = 0,
                    context_free_actions = new List<Core.Api.v1.Action>(),                    
                    actions = new List<Core.Api.v1.Action>()
                    {
                        new Core.Api.v1.Action()
                        {
                            account = "eosio.token",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() {actor = "tester112345", permission = "active" }
                            },
                            name = "transfer",
                            data = new { from = "tester112345", to = "tester212345", quantity = "1.0000 EOS", memo = "hello crypto world!" }
                        }
                    },
                    transaction_extensions = new List<Extension>()
                };

                int actionIndex = 0;
                var abiSerializer = new AbiSerializationProvider(DefaultApi);
                var abiResponses = await abiSerializer.GetTransactionAbis(trx);

                foreach (var action in trx.context_free_actions)
                {
                    action.data = SerializationHelper.ByteArrayToHexString(abiSerializer.SerializeActionData(action, abiResponses[actionIndex++]));
                }

                foreach (var action in trx.actions)
                {
                    action.data = SerializationHelper.ByteArrayToHexString(abiSerializer.SerializeActionData(action, abiResponses[actionIndex++]));
                }

                var getRequiredResult = await DefaultApi.GetRequiredKeys(new GetRequiredKeysRequest()
                {
                    available_keys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" },
                    transaction = trx
                });
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
                var getInfoResult = await DefaultApi.GetInfo();
                var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
                {
                    block_num_or_id = getInfoResult.last_irreversible_block_num.ToString()
                });

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
                var getInfoResult = await DefaultApi.GetInfo();
                var result = await DefaultApi.GetBlockHeaderState(new GetBlockHeaderStateRequest()
                {
                    block_num_or_id = getInfoResult.last_irreversible_block_num.ToString()
                });
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
                var result = await DefaultApi.GetTableRows(new GetTableRowsRequest() {
                    json = true,
                    code = "eosio",
                    scope = "eosio",
                    table = "producers"
                });
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
                var result = await DefaultApi.GetTableByScope(new GetTableByScopeRequest()
                {
                    code = "eosio.token",
                    table = "accounts"
                });
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
                var result = await DefaultApi.GetCurrencyBalance(new GetCurrencyBalanceRequest()
                {
                     code = "eosio.token",
                     account = "tester112345",
                     symbol = "EOS"
                });
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
                var result = await DefaultApi.GetCurrencyStats(new GetCurrencyStatsRequest()
                {
                    code = "eosio.token",
                    symbol = "EOS"
                });
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
                var result = await DefaultApi.GetProducers(new GetProducersRequest()
                {
                    json = false,                    
                });
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
                var result = await DefaultApi.GetProducerSchedule();
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
                var result = await DefaultApi.GetScheduledTransactions(new GetScheduledTransactionsRequest() {
                    json = true
                });
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
                await CreateTransaction();

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
                var result = await DefaultApi.GetActions(new GetActionsRequest() {
                    account_name = "eosio"
                });
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
                var trxResult = await CreateTransaction();

                var result = await DefaultApi.GetTransaction(new GetTransactionRequest()
                {
                    id = trxResult.transaction_id
                });
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
                var result = await DefaultApi.GetKeyAccounts(new GetKeyAccountsRequest()
                {
                    public_key = "EOS6MRyAjQq8ud7hVNYcfnVPJqcVpscN5So8BhtHuGYqET5GDW5CV"
                });
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
                var result = await DefaultApi.GetControlledAccounts(new GetControlledAccountsRequest()
                {
                    controlling_account = "eosio"
                });
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

        Assert.IsTrue(success);
        }

        private async Task<PushTransactionResponse> CreateTransaction()
        {
            var getInfoResult = await DefaultApi.GetInfo();
            var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
            {
                block_num_or_id = getInfoResult.last_irreversible_block_num.ToString()
            });


            var trx = new Transaction()
            {
                //trx headers
                expiration = getInfoResult.head_block_time.AddSeconds(60), //expire Seconds
                ref_block_num = (UInt16)(getInfoResult.last_irreversible_block_num & 0xFFFF),
                ref_block_prefix = getBlockResult.ref_block_prefix,
                // trx info
                max_net_usage_words = 0,
                max_cpu_usage_ms = 0,
                delay_sec = 0,
                context_free_actions = new List<Core.Api.v1.Action>(),
                transaction_extensions = new List<Extension>(),
                actions = new List<Core.Api.v1.Action>()
                {
                    new Core.Api.v1.Action()
                    {
                        account = "eosio.token",
                        authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {actor = "tester112345", permission = "active" }
                        },
                        name = "transfer",
                        data = new { from = "tester112345", to = "tester212345", quantity = "0.0001 EOS", memo = "hello crypto world!" }
                    }
                }
            };

            var abiSerializer = new AbiSerializationProvider(DefaultApi);
            var packedTrx = await abiSerializer.SerializePackedTransaction(trx);
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
            var signatures = await EosConfig.SignProvider.Sign(DefaultApi.Config.ChainId, requiredKeys, packedTrx);

            return await DefaultApi.PushTransaction(new PushTransactionRequest()
            {
                signatures = signatures.ToArray(),
                compression = 0,
                packed_context_free_data = "",
                packed_trx = SerializationHelper.ByteArrayToHexString(packedTrx)
            });
        }
    }
}

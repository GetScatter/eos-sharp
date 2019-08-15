using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Helpers;
using EosSharp.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosSharp.UnitTests
{
    public class ApiUnitTestCases
    {
        private EosConfigurator EosConfig;
        private EosApi DefaultApi;

        public ApiUnitTestCases(EosConfigurator eosConfig, EosApi api)
        {
            EosConfig = eosConfig;
            DefaultApi = api;
        }

        public Task GetInfo()
        {
            return DefaultApi.GetInfo();
        }

        public Task GetAccount()
        {
            return DefaultApi.GetAccount(new GetAccountRequest()
            {
                account_name = "eosio"
            });
        }

        public Task GetCode()
        {
            return DefaultApi.GetCode(new GetCodeRequest()
            {
                account_name = "eosio.token",
                code_as_wasm = true
            });
        }

        public Task GetAbi()
        {
            return DefaultApi.GetAbi(new GetAbiRequest()
            {
                account_name = "eosio.token"
            });
        }

        public async Task GetRawCodeAndAbi()
        {
            var result = await DefaultApi.GetRawCodeAndAbi(new GetRawCodeAndAbiRequest()
            {
                account_name = "eosio.token"
            });

            var abiSerializer = new AbiSerializationProvider(DefaultApi);
            var abiObject = abiSerializer.DeserializePackedAbi(result.abi);
        }

        public async Task GetRawAbi()
        {
            var result = await DefaultApi.GetRawAbi(new GetRawAbiRequest()
            {
                account_name = "eosio.token"
            });

            var abiSerializer = new AbiSerializationProvider(DefaultApi);
            var abiObject = abiSerializer.DeserializePackedAbi(result.abi);
        }

        public Task AbiJsonToBin()
        {
            return DefaultApi.AbiJsonToBin(new AbiJsonToBinRequest() {
                code = "eosio.token",
                action = "transfer",
                args = new Dictionary<string, object>() {
                    { "from", "eosio" },
                    { "to", "eosio.names" },
                    { "quantity", "1.0000 EOS" },
                    { "memo", "hello crypto world!" }
                }
            });
        }

        public async Task AbiBinToJson()
        {
            var binArgsResult = await DefaultApi.AbiJsonToBin(new AbiJsonToBinRequest()
            {
                code = "eosio.token",
                action = "transfer",
                args = new Dictionary<string, object>() {
                    { "from", "eosio" },
                    { "to", "eosio.names" },
                    { "quantity", "1.0000 EOS" },
                    { "memo", "hello crypto world!" }
                }
            });

            await DefaultApi.AbiBinToJson(new AbiBinToJsonRequest()
            {
                code = "eosio.token",
                action = "transfer",
                binargs = binArgsResult.binargs
            });
        }

        public async Task GetRequiredKeys()
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
                        data = new Dictionary<string, object>() {
                            { "from", "tester112345" },
                            { "to", "tester212345" },
                            { "quantity", "1.0000 EOS" },
                            { "memo", "hello crypto world!" }
                        }
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
        }

        public async Task GetBlock()
        {
            var getInfoResult = await DefaultApi.GetInfo();
            var getBlockResult = await DefaultApi.GetBlock(new GetBlockRequest()
            {
                block_num_or_id = getInfoResult.last_irreversible_block_num.ToString()
            });
        }

        public async Task GetBlockHeaderState()
        {
            var getInfoResult = await DefaultApi.GetInfo();
            var result = await DefaultApi.GetBlockHeaderState(new GetBlockHeaderStateRequest()
            {
                block_num_or_id = getInfoResult.head_block_num.ToString()
            });
        }

        public Task GetTableRows()
        {
            return DefaultApi.GetTableRows(new GetTableRowsRequest()
            {
                json = true,
                code = "eosio.token",
                scope = "EOS",
                table = "stat"
            });
        }

        public Task GetTableByScope()
        {
            return DefaultApi.GetTableByScope(new GetTableByScopeRequest()
            {
                code = "eosio.token",
                table = "accounts"
            });
        }

        public Task GetCurrencyBalance()
        {
            return DefaultApi.GetCurrencyBalance(new GetCurrencyBalanceRequest()
            {
                    code = "eosio.token",
                    account = "tester112345",
                    symbol = "EOS"
            });
        }

        public Task GetCurrencyStats()
        {
            return DefaultApi.GetCurrencyStats(new GetCurrencyStatsRequest()
            {
                code = "eosio.token",
                symbol = "EOS"
            });
        }

        public Task GetProducers()
        {
            return DefaultApi.GetProducers(new GetProducersRequest()
            {
                json = false,                    
            });
        }

        public Task GetProducerSchedule()
        {
            return DefaultApi.GetProducerSchedule();
        }

        public Task GetScheduledTransactions()
        {
            return DefaultApi.GetScheduledTransactions(new GetScheduledTransactionsRequest() {
                json = true
            });
        }

        public Task PushTransaction()
        {
            return CreateTransaction();
        }

        public Task GetActions()
        {
            return DefaultApi.GetActions(new GetActionsRequest() {
                account_name = "eosio"
            });
        }

        public async Task GetTransaction()
        {
            var trxResult = await CreateTransaction();

            var result = await DefaultApi.GetTransaction(new GetTransactionRequest()
            {
                id = trxResult.transaction_id
            });
        }

        public Task GetKeyAccounts()
        {
            return DefaultApi.GetKeyAccounts(new GetKeyAccountsRequest()
            {
                public_key = "EOS6MRyAjQq8ud7hVNYcfnVPJqcVpscN5So8BhtHuGYqET5GDW5CV"
            });
        }

        public Task GetControlledAccounts()
        {
            return DefaultApi.GetControlledAccounts(new GetControlledAccountsRequest()
            {
                controlling_account = "eosio"
            });
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
                            new PermissionLevel() {actor = "player1", permission = "active" }
                        },
                        name = "transfer",
                        data = new Dictionary<string, object>() {
                            { "from", "player1" },
                            { "to", "player2" },
                            { "quantity", "1.0000 EOS" },
                            { "memo", "hello crypto world!" }
                        }
                    }
                }
            };

            var abiSerializer = new AbiSerializationProvider(DefaultApi);
            var packedTrx = await abiSerializer.SerializePackedTransaction(trx);
            var requiredKeys = new List<string>() { "EOS6MRyAjQq8ud7hVNYcfnVPJqcVpscN5So8BhtHuGYqET5GDW5CV" };
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

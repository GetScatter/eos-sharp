using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Helpers;
using EosSharp.Core.Interfaces;
using EosSharp.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosSharp.Core
{
    /// <summary>
    /// Client wrapper to interact with eos blockchains.
    /// </summary>
    public class EosBase
    {
        private EosConfigurator EosConfig { get; set; }
        private EosApi Api { get; set; }
        private AbiSerializationProvider AbiSerializer { get; set; }

        /// <summary>
        /// Client wrapper constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        /// <param name="httpHandler">Http handler implementation</param>
        public EosBase(EosConfigurator config, IHttpHandler httpHandler)
        {
            EosConfig = config;
            if (EosConfig == null)
            {
                throw new ArgumentNullException("config");
            }
            Api = new EosApi(EosConfig, httpHandler);
            AbiSerializer = new AbiSerializationProvider(Api);
        }

        #region Api Methods

        /// <summary>
        /// Query for blockchain information
        /// </summary>
        /// <returns>Blockchain information</returns>
        public Task<GetInfoResponse> GetInfo()
        {
            return Api.GetInfo();
        }

        /// <summary>
        /// Query for account information
        /// </summary>
        /// <param name="accountName">account to query information</param>
        /// <returns>account information</returns>
        public Task<GetAccountResponse> GetAccount(string accountName)
        {
            return Api.GetAccount(new GetAccountRequest()
            {
                account_name = accountName
            });
        }

        /// <summary>
        /// Query for smart contract detailed information
        /// </summary>
        /// <param name="accountName">smart contract account name</param>
        /// <param name="codeAsWasm">query code as wasm, wast otherwise</param>
        /// <returns>smart contract information</returns>
        public Task<GetCodeResponse> GetCode(string accountName, bool codeAsWasm)
        {
            return Api.GetCode(new GetCodeRequest()
            {
                account_name = accountName,
                code_as_wasm = codeAsWasm
            });
        }

        /// <summary>
        /// Query for smart contract abi detailed information
        /// </summary>
        /// <param name="accountName">smart contract account name</param>
        /// <returns></returns>
        public async Task<Abi> GetAbi(string accountName)
        {
            return (await Api.GetAbi(new GetAbiRequest()
            {
                account_name = accountName
            })).abi;
        }

        /// <summary>
        /// Query for smart contract abi detailed information
        /// </summary>
        /// <param name="accountName">smart contract account name</param>
        /// <param name="abiHash">TODO</param>
        /// <returns>smart contract abi information as Base64FcString</returns>
        public Task<GetRawAbiResponse> GetRawAbi(string accountName, string abiHash = null)
        {
            return Api.GetRawAbi(new GetRawAbiRequest()
            {
                account_name = accountName,
                abi_hash = abiHash
            });
        }

        /// <summary>
        /// Query for smart contract raw wasm and abi information
        /// </summary>
        /// <param name="accountName">smart contract account name</param>
        /// <returns>smart contract wasm and abi information</returns>
        public Task<GetRawCodeAndAbiResponse> GetRawCodeAndAbi(string accountName)
        {
            return Api.GetRawCodeAndAbi(new GetRawCodeAndAbiRequest()
            {
                account_name = accountName
            });
        }

        /// <summary>
        /// Transform action data to packed binary format
        /// </summary>
        /// <param name="code">smart contract account name</param>
        /// <param name="action">action name</param>
        /// <param name="data">action </param>
        /// <returns></returns>
        public async Task<string> AbiJsonToBin(string code, string action, object data)
        {
            return (await Api.AbiJsonToBin(new AbiJsonToBinRequest()
            {
                code = code,
                action = action,
                args = data
            })).binargs;
        }

        /// <summary>
        /// Transform action data as packed binary format to object
        /// </summary>
        /// <param name="code"></param>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<object> AbiBinToJson(string code, string action, string data)
        {
            return (await Api.AbiBinToJson(new AbiBinToJsonRequest()
            {
                code = code,
                action = action,
                binargs = data
            })).args;
        }

        /// <summary>
        /// Calculate required keys to sign the given transaction
        /// </summary>
        /// <param name="availableKeys">available public keys list</param>
        /// <param name="trx">transaction requiring signatures</param>
        /// <returns>required public keys</returns>
        public async Task<List<string>> GetRequiredKeys(List<string> availableKeys, Transaction trx)
        {
            int actionIndex = 0;
            var abiSerializer = new AbiSerializationProvider(Api);
            var abiResponses = await abiSerializer.GetTransactionAbis(trx);

            foreach (var action in trx.context_free_actions)
            {
                action.data = SerializationHelper.ByteArrayToHexString(abiSerializer.SerializeActionData(action, abiResponses[actionIndex++]));
            }

            foreach (var action in trx.actions)
            {
                action.data = SerializationHelper.ByteArrayToHexString(abiSerializer.SerializeActionData(action, abiResponses[actionIndex++]));
            }

            return (await Api.GetRequiredKeys(new GetRequiredKeysRequest()
            {
                available_keys = availableKeys,
                transaction = trx
            })).required_keys;
        }

        /// <summary>
        /// Query for blockchain block information
        /// </summary>
        /// <param name="blockNumOrId">block number or id to query information</param>
        /// <returns>block information</returns>
        public Task<GetBlockResponse> GetBlock(string blockNumOrId)
        {
            return Api.GetBlock(new GetBlockRequest()
            {
                block_num_or_id = blockNumOrId
            });
        }

        /// <summary>
        /// Query block head state information
        /// </summary>
        /// <param name="blockNumOrId">block number or id</param>
        /// <returns>TODO</returns>
        public Task<GetBlockHeaderStateResponse> GetBlockHeaderState(string blockNumOrId)
        {
            return Api.GetBlockHeaderState(new GetBlockHeaderStateRequest()
            {
                block_num_or_id = blockNumOrId
            });
        }

        /// <summary>
        /// Query for blockchain smart contract table state information
        /// </summary>
        /// <typeparam name="TRowType">Type used for each row</typeparam>
        /// <param name="request.Json">Request rows using json or raw format</param>
        /// <param name="request.Code">accountName of the contract to search for table rows</param>
        /// <param name="request.Scope">scope text segmenting the table set</param>
        /// <param name="request.Table">table name</param>
        /// <param name="request.TableKey">unused so far?</param>
        /// <param name="request.LowerBound">lower bound for the selected index value</param>
        /// <param name="request.UpperBound">upper bound for the selected index value</param>
        /// <param name="request.KeyType">Type of the index choosen, ex: i64</param>
        /// <param name="request.IndexPosition">1 - primary(first), 2 - secondary index(in order defined by multi_index), 3 - third index, etc</param>
        /// <returns>Rows and if is there More rows to be fetched</returns>
        public async Task<GetTableRowsResponse<TRowType>> GetTableRows<TRowType>(GetTableRowsRequest request)
        {
            if (request.json)
            {
                return await Api.GetTableRows<TRowType>(request);
            }
            else
            {
                var apiResult = await Api.GetTableRows(request);
                var result = new GetTableRowsResponse<TRowType>()
                {
                    more = apiResult.more
                };

                var unpackedRows = new List<TRowType>();

                var abi = await AbiSerializer.GetAbi(request.code);
                var table = abi.tables.First(t => t.name == request.table);

                foreach (var rowData in apiResult.rows)
                {
                    unpackedRows.Add(AbiSerializer.DeserializeStructData<TRowType>(table.type, (string)rowData, abi));
                }

                result.rows = unpackedRows;
                return result;
            }
        }

        /// <summary>
        /// Query for blockchain smart contract table state information
        /// </summary>
        /// <param name="request.Json">Request rows using json or raw format</param>
        /// <param name="request.Code">accountName of the contract to search for table rows</param>
        /// <param name="request.Scope">scope text segmenting the table set</param>
        /// <param name="request.Table">table name</param>
        /// <param name="request.TableKey">unused so far?</param>
        /// <param name="request.LowerBound">lower bound for the selected index value</param>
        /// <param name="request.UpperBound">upper bound for the selected index value</param>
        /// <param name="request.KeyType">Type of the index choosen, ex: i64</param>
        /// <param name="request.IndexPosition">1 - primary(first), 2 - secondary index(in order defined by multi_index), 3 - third index, etc</param>
        /// <returns>Rows and if is there More rows to be fetched</returns>
        public async Task<GetTableRowsResponse> GetTableRows(GetTableRowsRequest request)
        {
            var result = await Api.GetTableRows(request);

            if (!request.json)
            {
                var unpackedRows = new List<object>();

                var abi = await AbiSerializer.GetAbi(request.code);
                var table = abi.tables.First(t => t.name == request.table);

                foreach (var rowData in result.rows)
                {
                    unpackedRows.Add(AbiSerializer.DeserializeStructData(table.type, (string)rowData, abi));
                }

                result.rows = unpackedRows;
            }

            return result;
        }

        /// <summary>
        /// Query account balance for a given token
        /// </summary>
        /// <param name="code">token smart contract account</param>
        /// <param name="account">account name to check</param>
        /// <param name="symbol">token symbol (optional)</param>
        /// <returns>token balances</returns>
        public async Task<List<string>> GetCurrencyBalance(string code, string account, string symbol)
        {
            return (await Api.GetCurrencyBalance(new GetCurrencyBalanceRequest()
            {
                code = code,
                account = account,
                symbol = symbol
            })).assets;
        }

        /// <summary>
        /// Query token statistics
        /// </summary>
        /// <param name="code">token smart contract account</param>
        /// <param name="symbol">token symbol (optional)</param>
        /// <returns>currencies statistics</returns>
        public async Task<Dictionary<string, CurrencyStat>> GetCurrencyStats(string code, string symbol)
        {
            return (await Api.GetCurrencyStats(new GetCurrencyStatsRequest()
            {
                code = code,
                symbol = symbol
            })).stats;
        }

        /// <summary>
        /// Query producers information
        /// </summary>
        /// <param name="request.json">get list as json</param>
        /// <param name="request.lower_bound">lower bound for the selected index value</param>
        /// <param name="request.limit">limit the amount of results. Default 50</param>
        /// <returns>producers information</returns>
        public async Task<GetProducersResponse> GetProducers(GetProducersRequest request)
        {
            var result = await Api.GetProducers(request);

            if (!request.json)
            {
                var unpackedRows = new List<object>();

                foreach (var rowData in result.rows)
                {
                    unpackedRows.Add(AbiSerializer.DeserializeType<Producer>((string)rowData));
                }

                result.rows = unpackedRows;
            }

            return result;
        }

        /// <summary>
        /// Query producers schedule
        /// </summary>
        /// <returns>Active, pending and proposed schedule</returns>
        public Task<GetProducerScheduleResponse> GetProducerSchedule()
        {
            return Api.GetProducerSchedule();
        }

        /// <summary>
        /// Query scheduled transactions
        /// </summary>
        /// <param name="request.json">get list as json</param>
        /// <param name="request.lower_bound">lower bound for the selected index value</param>
        /// <param name="request.limit">limit the amount of results. Default 50</param>
        /// <returns>Scheduled transactions</returns>
        public async Task<GetScheduledTransactionsResponse> GetScheduledTransactions(GetScheduledTransactionsRequest request)
        {
            var result = await Api.GetScheduledTransactions(request);

            if (!request.json)
            {
                foreach (var trx in result.transactions)
                {
                    try
                    {
                        trx.transaction = await AbiSerializer.DeserializePackedTransaction((string)trx.transaction);
                    }
                    catch (Exception)
                    {
                        //ignore transactions with invalid abi's
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a signed transaction using the signature provider and broadcasts it to the network
        /// </summary>
        /// <param name="trx">Transaction to send</param>
        /// <returns>transaction id</returns>
        public async Task<string> CreateTransaction(Transaction trx)
        {
            if (EosConfig.SignProvider == null)
                throw new ArgumentNullException("SignProvider");

            GetInfoResponse getInfoResult = null;
            string chainId = EosConfig.ChainId;

            if (string.IsNullOrWhiteSpace(chainId))
            {
                getInfoResult = await Api.GetInfo();
                chainId = getInfoResult.chain_id;
            }

            if (trx.expiration == DateTime.MinValue ||
               trx.ref_block_num == 0 ||
               trx.ref_block_prefix == 0)
            {
                if (getInfoResult == null)
                    getInfoResult = await Api.GetInfo();

                var getBlockResult = await Api.GetBlock(new GetBlockRequest()
                {
                    block_num_or_id = getInfoResult.last_irreversible_block_num.ToString()
                });

                trx.expiration = getInfoResult.head_block_time.AddSeconds(EosConfig.ExpireSeconds);
                trx.ref_block_num = (UInt16)(getInfoResult.last_irreversible_block_num & 0xFFFF);
                trx.ref_block_prefix = getBlockResult.ref_block_prefix;
            }

            var packedTrx = await AbiSerializer.SerializePackedTransaction(trx);
            var availableKeys = await EosConfig.SignProvider.GetAvailableKeys();
            var requiredKeys = await GetRequiredKeys(availableKeys.ToList(), trx);

            IEnumerable<string> abis = null;

            if (trx.actions != null)
                abis = trx.actions.Select(a => a.account);

            var signatures = await EosConfig.SignProvider.Sign(chainId, requiredKeys, packedTrx, abis);

            var result = await Api.PushTransaction(new PushTransactionRequest()
            {
                signatures = signatures.ToArray(),
                compression = 0,
                packed_context_free_data = "",
                packed_trx = SerializationHelper.ByteArrayToHexString(packedTrx)
            });

            return result.transaction_id;
        }

        /// <summary>
        /// Query for account actions log
        /// </summary>
        /// <param name="accountName">account to query information</param>
        /// <param name="pos">Absolute sequence positon -1 is the end/last action</param>
        /// <param name="offset">Number of actions relative to pos, negative numbers return [pos-offset,pos), positive numbers return [pos,pos+offset)</param>
        /// <returns></returns>
        public Task<GetActionsResponse> GetActions(string accountName, Int32 pos, Int32 offset)
        {
            return Api.GetActions(new GetActionsRequest()
            {
                account_name = accountName,
                pos = pos,
                offset = offset
            });
        }

        /// <summary>
        /// Query transaction information
        /// </summary>
        /// <param name="transactionId">transaction id</param>
        /// <returns>Transaction information</returns>
        public Task<GetTransactionResponse> GetTransaction(string transactionId)
        {
            return Api.GetTransaction(new GetTransactionRequest()
            {
                id = transactionId
            });
        }

        /// <summary>
        /// Query public key accounts
        /// </summary>
        /// <param name="publicKey">public key</param>
        /// <returns>account names</returns>
        public async Task<List<string>> GetKeyAccounts(string publicKey)
        {
            return (await Api.GetKeyAccounts(new GetKeyAccountsRequest()
            {
                public_key = publicKey
            })).account_names;
        }

        /// <summary>
        /// Query controlled accounts by a given account
        /// </summary>
        /// <param name="accountName">account name to search</param>
        /// <returns>controlled account names</returns>
        public async Task<List<string>> GetControlledAccounts(string accountName)
        {
            return (await Api.GetControlledAccounts(new GetControlledAccountsRequest()
            {
                controlling_account = accountName
            })).controlled_accounts;
        }

        #endregion
    }
}

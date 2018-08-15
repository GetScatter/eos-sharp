  
// Auto Generated, do not edit.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EosSharp.Api.v1
{
    public class GetInfoResponse
    {
		[JsonProperty("server_version")]   
		public string ServerVersion { get; set; }
		[JsonProperty("chain_id")]   
		public string ChainId { get; set; }
		[JsonProperty("head_block_num")]   
		public string HeadBlockNum { get; set; }
		[JsonProperty("last_irreversible_block_num")]   
		public string LastIrreversibleBlockNum { get; set; }
		[JsonProperty("last_irreversible_block_id")]   
		public string LastIrreversibleBlockId { get; set; }
		[JsonProperty("head_block_id")]   
		public string HeadBlockId { get; set; }
		[JsonProperty("head_block_time")]   
		public string HeadBlockTime { get; set; }
		[JsonProperty("head_block_producer")]   
		public string HeadBlockProducer { get; set; }
		[JsonProperty("virtual_block_cpu_limit")]   
		public string VirtualBlockCpuLimit { get; set; }
		[JsonProperty("virtual_block_net_limit")]   
		public string VirtualBlockNetLimit { get; set; }
		[JsonProperty("block_cpu_limit")]   
		public string BlockCpuLimit { get; set; }
		[JsonProperty("block_net_limit")]   
		public string BlockNetLimit { get; set; }
    }

    public class GetAccountRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
    }
    public class GetAccountResponse
    {
    }

    public class GetCodeRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("code_as_wasm")]   
		public string CodeAsWasm { get; set; }
    }
    public class GetCodeResponse
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("wast")]   
		public string Wast { get; set; }
		[JsonProperty("wasm")]   
		public string Wasm { get; set; }
		[JsonProperty("code_hash")]   
		public string CodeHash { get; set; }
		[JsonProperty("abi")]   
		public string Abi { get; set; }
    }

    public class GetAbiRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
    }
    public class GetAbiResponse
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("abi")]   
		public string Abi { get; set; }
    }

    public class GetRawCodeAndAbiRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
    }
    public class GetRawCodeAndAbiResponse
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("wasm")]   
		public string Wasm { get; set; }
		[JsonProperty("abi")]   
		public string Abi { get; set; }
    }

    public class AbiJsonToBinRequest
    {
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("action")]   
		public string Action { get; set; }
		[JsonProperty("args")]   
		public string Args { get; set; }
    }
    public class AbiJsonToBinResponse
    {
		[JsonProperty("binargs")]   
		public string Binargs { get; set; }
    }

    public class AbiBinToJsonRequest
    {
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("action")]   
		public string Action { get; set; }
		[JsonProperty("binargs")]   
		public string Binargs { get; set; }
    }
    public class AbiBinToJsonResponse
    {
		[JsonProperty("args")]   
		public string Args { get; set; }
    }

    public class GetRequiredKeysRequest
    {
		[JsonProperty("transaction")]   
		public string Transaction { get; set; }
		[JsonProperty("available_keys")]   
		public string AvailableKeys { get; set; }
    }
    public class GetRequiredKeysResponse
    {
    }

    public class GetBlockRequest
    {
		[JsonProperty("block_num_or_id")]   
		public string BlockNumOrId { get; set; }
    }
    public class GetBlockResponse
    {
    }

    public class GetBlockHeaderStateRequest
    {
		[JsonProperty("block_num_or_id")]   
		public string BlockNumOrId { get; set; }
    }
    public class GetBlockHeaderStateResponse
    {
    }

    public class GetTableRowsRequest
    {
		[JsonProperty("json")]   
		public bool? Json { get; set; } = false;
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("scope")]   
		public string Scope { get; set; }
		[JsonProperty("table")]   
		public string Table { get; set; }
		[JsonProperty("table_key")]   
		public string TableKey { get; set; }
		[JsonProperty("lower_bound")]   
		public string LowerBound { get; set; } = "0";
		[JsonProperty("upper_bound")]   
		public string UpperBound { get; set; } = "-1";
		[JsonProperty("limit")]   
		public UInt32? Limit { get; set; } = 10;
		[JsonProperty("key_type")]   
		public string KeyType { get; set; }
		[JsonProperty("index_position")]   
		public string IndexPosition { get; set; }
    }
    public class GetTableRowsResponse
    {
		[JsonProperty("rows")]   
		public List<object> Rows { get; set; }
		[JsonProperty("more")]   
		public bool? More { get; set; }
    }

    public class GetCurrencyBalanceRequest
    {
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("account")]   
		public string Account { get; set; }
		[JsonProperty("symbol")]   
		public string Symbol { get; set; }
    }
    public class GetCurrencyBalanceResponse
    {
		[JsonProperty("assets")]   
		public List<string> Assets { get; set; }
    }

    public class GetCurrencyStatsRequest
    {
		[JsonProperty("name")]   
		public string Name { get; set; }
		[JsonProperty("symbol")]   
		public string Symbol { get; set; }
    }
    public class GetCurrencyStatsResponse
    {
		[JsonProperty("supply")]   
		public string Supply { get; set; }
		[JsonProperty("max_supply")]   
		public string MaxSupply { get; set; }
		[JsonProperty("issuer")]   
		public string Issuer { get; set; }
    }

    public class GetProducersRequest
    {
		[JsonProperty("json")]   
		public bool? Json { get; set; } = false;
		[JsonProperty("lower_bound")]   
		public string LowerBound { get; set; }
		[JsonProperty("limit")]   
		public UInt32? Limit { get; set; } = 50;
    }
    public class GetProducersResponse
    {
		[JsonProperty("rows")]   
		public List<object> Rows { get; set; }
		[JsonProperty("total_producer_vote_weight")]   
		public double? TotalProducerVoteWeight { get; set; }
		[JsonProperty("more")]   
		public bool? More { get; set; }
    }

    public class GetProducerScheduleResponse
    {
		[JsonProperty("active")]   
		public List<object> Active { get; set; }
		[JsonProperty("pending")]   
		public List<object> Pending { get; set; }
		[JsonProperty("proposed")]   
		public List<object> Proposed { get; set; }
    }

    public class GetScheduledTransactionsRequest
    {
		[JsonProperty("json")]   
		public bool? Json { get; set; } = false;
		[JsonProperty("lower_bound")]   
		public string LowerBound { get; set; }
		[JsonProperty("limit")]   
		public UInt32? Limit { get; set; } = 50;
    }
    public class GetScheduledTransactionsResponse
    {
		[JsonProperty("transactions")]   
		public List<object> Transactions { get; set; }
		[JsonProperty("more")]   
		public bool? More { get; set; }
    }

    public class PushBlockRequest
    {
		[JsonProperty("block")]   
		public string Block { get; set; }
    }
    public class PushBlockResponse
    {
    }

    public class PushTransactionRequest
    {
		[JsonProperty("signed_transaction")]   
		public string SignedTransaction { get; set; }
    }
    public class PushTransactionResponse
    {
		[JsonProperty("transaction_id")]   
		public string TransactionId { get; set; }
		[JsonProperty("processed")]   
		public string Processed { get; set; }
    }

    public class PushTransactionsRequest
    {
		[JsonProperty("signed_transaction")]   
		public List<string> SignedTransaction { get; set; }
    }
    public class PushTransactionsResponse
    {
		[JsonProperty("results")]   
		public List<string> Results { get; set; }
    }

    public class GetActionsRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("pos")]   
		public Int32? Pos { get; set; }
		[JsonProperty("offset")]   
		public Int32? Offset { get; set; }
    }
    public class GetActionsResponse
    {
		[JsonProperty("actions")]   
		public List<string> Actions { get; set; }
		[JsonProperty("last_irreversible_block")]   
		public UInt32? LastIrreversibleBlock { get; set; }
		[JsonProperty("time_limit_exceeded_error")]   
		public bool? TimeLimitExceededError { get; set; }
    }

    public class GetTransactionRequest
    {
		[JsonProperty("id")]   
		public string Id { get; set; }
		[JsonProperty("block_num_hint")]   
		public string BlockNumHint { get; set; }
    }
    public class GetTransactionResponse
    {
		[JsonProperty("id")]   
		public string Id { get; set; }
		[JsonProperty("trx")]   
		public string Trx { get; set; }
		[JsonProperty("block_time")]   
		public string BlockTime { get; set; }
		[JsonProperty("block_num")]   
		public UInt32? BlockNum { get; set; }
		[JsonProperty("last_irreversible_block")]   
		public UInt32? LastIrreversibleBlock { get; set; }
		[JsonProperty("traces")]   
		public List<string> Traces { get; set; }
    }

    public class GetKeyAccountsRequest
    {
		[JsonProperty("public_key")]   
		public string PublicKey { get; set; }
    }
    public class GetKeyAccountsResponse
    {
		[JsonProperty("account_names")]   
		public List<string> AccountNames { get; set; }
    }

    public class GetControlledAccountsRequest
    {
		[JsonProperty("controlling_account")]   
		public string ControllingAccount { get; set; }
    }
    public class GetControlledAccountsResponse
    {
		[JsonProperty("controlled_accounts")]   
		public List<string> ControlledAccounts { get; set; }
    }

}


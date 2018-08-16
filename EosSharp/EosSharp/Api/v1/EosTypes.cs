  
// Auto Generated, do not edit.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EosSharp.Api.v1
{
	#region generate api types
	public class Resource
    {
		[JsonProperty("used")]   
		public Int32 Used { get; set; }
		[JsonProperty("available")]   
		public Int32 Available { get; set; }
		[JsonProperty("max")]   
		public Int32 Max { get; set; }
    }
	public class AuthorityKey
    {
		[JsonProperty("key")]   
		public string Key { get; set; }
		[JsonProperty("weight")]   
		public Int32 Weight { get; set; }
    }
	public class AuthorityAccount
    {
		[JsonProperty("account")]   
		public string Account { get; set; }
		[JsonProperty("weight")]   
		public Int32 Weight { get; set; }
    }
	public class AuthorityWait
    {
		[JsonProperty("wait_sec")]   
		public string WaitSec { get; set; }
		[JsonProperty("weight")]   
		public Int32 Weight { get; set; }
    }
	public class Authority
    {
		[JsonProperty("threshold")]   
		public UInt32 Threshold { get; set; }
		[JsonProperty("keys")]   
		public List<AuthorityKey> Keys { get; set; }
		[JsonProperty("accounts")]   
		public List<AuthorityAccount> Accounts { get; set; }
		[JsonProperty("waits")]   
		public List<AuthorityWait> Waits { get; set; }
    }
	public class Permission
    {
		[JsonProperty("perm_name")]   
		public string PermName { get; set; }
		[JsonProperty("parent")]   
		public string Parent { get; set; }
		[JsonProperty("required_auth")]   
		public Authority RequiredAuth { get; set; }
    }
	public class AbiType
    {
		[JsonProperty("new_type_name")]   
		public string NewTypeName { get; set; }
		[JsonProperty("type")]   
		public string Type { get; set; }
    }
	public class AbiField
    {
		[JsonProperty("name")]   
		public string Name { get; set; }
		[JsonProperty("type")]   
		public string Type { get; set; }
    }
	public class AbiStruct
    {
		[JsonProperty("name")]   
		public string Name { get; set; }
		[JsonProperty("base")]   
		public string Base { get; set; }
		[JsonProperty("fields")]   
		public List<AbiField> Fields { get; set; }
    }
	public class AbiAction
    {
		[JsonProperty("name")]   
		public string Name { get; set; }
		[JsonProperty("type")]   
		public string Type { get; set; }
		[JsonProperty("ricardian_contract")]   
		public string RicardianContract { get; set; }
    }
	public class AbiTable
    {
		[JsonProperty("name")]   
		public string Name { get; set; }
		[JsonProperty("index_type")]   
		public string IndexType { get; set; }
		[JsonProperty("key_names")]   
		public List<string> KeyNames { get; set; }
		[JsonProperty("key_types")]   
		public List<string> KeyTypes { get; set; }
		[JsonProperty("type")]   
		public string Type { get; set; }
    }
	public class Abi
    {
		[JsonProperty("version")]   
		public string Version { get; set; }
		[JsonProperty("types")]   
		public List<AbiType> Types { get; set; }
		[JsonProperty("structs")]   
		public List<AbiStruct> Structs { get; set; }
		[JsonProperty("actions")]   
		public List<AbiAction> Actions { get; set; }
		[JsonProperty("tables")]   
		public List<AbiTable> Tables { get; set; }
		[JsonProperty("ricardian_clauses")]   
		public List<string> RicardianClauses { get; set; }
		[JsonProperty("error_messages")]   
		public List<string> ErrorMessages { get; set; }
		[JsonProperty("abi_extensions")]   
		public List<string> AbiExtensions { get; set; }
    }
	public class CurrencyStat
    {
		[JsonProperty("supply")]   
		public string Supply { get; set; }
		[JsonProperty("max_supply")]   
		public string MaxSupply { get; set; }
		[JsonProperty("issuer")]   
		public string Issuer { get; set; }
    }
	#endregion

	#region generate api method types
    public class GetInfoResponse
    {
		[JsonProperty("server_version")]   
		public string ServerVersion { get; set; }
		[JsonProperty("chain_id")]   
		public string ChainId { get; set; }
		[JsonProperty("head_block_num")]   
		public UInt32? HeadBlockNum { get; set; }
		[JsonProperty("last_irreversible_block_num")]   
		public UInt32? LastIrreversibleBlockNum { get; set; }
		[JsonProperty("last_irreversible_block_id")]   
		public string LastIrreversibleBlockId { get; set; }
		[JsonProperty("head_block_id")]   
		public string HeadBlockId { get; set; }
		[JsonProperty("head_block_time")]   
		public DateTime? HeadBlockTime { get; set; }
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
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("head_block_num")]   
		public UInt32? HeadBlockNum { get; set; }
		[JsonProperty("head_block_time")]   
		public DateTime? HeadBlockTime { get; set; }
		[JsonProperty("privileged")]   
		public bool? Privileged { get; set; }
		[JsonProperty("last_code_update")]   
		public DateTime? LastCodeUpdate { get; set; }
		[JsonProperty("created")]   
		public DateTime? Created { get; set; }
		[JsonProperty("ram_quota")]   
		public Int32? RamQuota { get; set; }
		[JsonProperty("net_weight")]   
		public Int32? NetWeight { get; set; }
		[JsonProperty("cpu_weight")]   
		public Int32? CpuWeight { get; set; }
		[JsonProperty("net_limit")]   
		public Resource NetLimit { get; set; }
		[JsonProperty("cpu_limit")]   
		public Resource CpuLimit { get; set; }
		[JsonProperty("ram_usage")]   
		public UInt32? RamUsage { get; set; }
		[JsonProperty("permissions")]   
		public List<Permission> Permissions { get; set; }
		[JsonProperty("total_resources")]   
		public string TotalResources { get; set; }
		[JsonProperty("self_delegated_bandwidth")]   
		public string SelfDelegatedBandwidth { get; set; }
		[JsonProperty("refund_request")]   
		public object RefundRequest { get; set; }
		[JsonProperty("voter_info")]   
		public object VoterInfo { get; set; }
    }

    public class GetCodeRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("code_as_wasm")]   
		public bool? CodeAsWasm { get; set; }
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
		public Abi Abi { get; set; }
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
		public Abi Abi { get; set; }
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
		[JsonProperty("timestamp")]   
		public DateTime? Timestamp { get; set; }
		[JsonProperty("producer")]   
		public string Producer { get; set; }
		[JsonProperty("confirmed")]   
		public UInt32 Confirmed { get; set; }
		[JsonProperty("previous")]   
		public string Previous { get; set; }
		[JsonProperty("transaction_mroot")]   
		public string TransactionMroot { get; set; }
		[JsonProperty("action_mroot")]   
		public string ActionMroot { get; set; }
		[JsonProperty("schedule_version")]   
		public UInt32 ScheduleVersion { get; set; }
		[JsonProperty("new_producers")]   
		public string NewProducers { get; set; }
		[JsonProperty("header_extensions")]   
		public List<object> HeaderExtensions { get; set; }
		[JsonProperty("producer_signature")]   
		public string ProducerSignature { get; set; }
		[JsonProperty("transactions")]   
		public List<object> Transactions { get; set; }
		[JsonProperty("id")]   
		public string Id { get; set; }
		[JsonProperty("block_num")]   
		public UInt32 BlockNum { get; set; }
		[JsonProperty("ref_block_prefix")]   
		public UInt32 RefBlockPrefix { get; set; }
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
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("symbol")]   
		public string Symbol { get; set; }
    }
    public class GetCurrencyStatsResponse
    {
		[JsonProperty("stats")]   
		public Dictionary<string, CurrencyStat> Stats { get; set; }
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

	#endregion
}


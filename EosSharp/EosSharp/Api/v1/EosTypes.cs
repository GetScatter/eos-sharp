  

  

// Auto Generated, do not edit.
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace EosSharp.Api.v1
{
	#region generate api types
	[Serializable]
	public class Resource
    {
		[JsonProperty("used")]
		public Int32 Used { get; set; }
		[JsonProperty("available")]
		public Int32 Available { get; set; }
		[JsonProperty("max")]
		public Int32 Max { get; set; }
    }
	[Serializable]
	public class AuthorityKey
    {
		[JsonProperty("key")]
		public string Key { get; set; }
		[JsonProperty("weight")]
		public Int32 Weight { get; set; }
    }
	[Serializable]
	public class AuthorityAccount
    {
		[JsonProperty("account")]
		public string Account { get; set; }
		[JsonProperty("weight")]
		public Int32 Weight { get; set; }
    }
	[Serializable]
	public class AuthorityWait
    {
		[JsonProperty("wait_sec")]
		public string WaitSec { get; set; }
		[JsonProperty("weight")]
		public Int32 Weight { get; set; }
    }
	[Serializable]
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
	[Serializable]
	public class Permission
    {
		[JsonProperty("perm_name")]
		public string PermName { get; set; }
		[JsonProperty("parent")]
		public string Parent { get; set; }
		[JsonProperty("required_auth")]
		public Authority RequiredAuth { get; set; }
    }
	[Serializable]
	public class AbiType
    {
		[JsonProperty("new_type_name")]
		public string NewTypeName { get; set; }
		[JsonProperty("type")]
		public string Type { get; set; }
    }
	[Serializable]
	public class AbiField
    {
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("type")]
		public string Type { get; set; }
    }
	[Serializable]
	public class AbiStruct
    {
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("base")]
		public string Base { get; set; }
		[JsonProperty("fields")]
		public List<AbiField> Fields { get; set; }
    }
	[Serializable]
	public class AbiAction
    {
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("type")]
		public string Type { get; set; }
		[JsonProperty("ricardian_contract")]
		public string RicardianContract { get; set; }
    }
	[Serializable]
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
	[Serializable]
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
	[Serializable]
	public class CurrencyStat
    {
		[JsonProperty("supply")]
		public string Supply { get; set; }
		[JsonProperty("max_supply")]
		public string MaxSupply { get; set; }
		[JsonProperty("issuer")]
		public string Issuer { get; set; }
    }
	[Serializable]
	public class Producer
    {
		[JsonProperty("owner")]
		public string Owner { get; set; }
		[JsonProperty("total_votes")]
		public double? TotalVotes { get; set; }
		[JsonProperty("producer_key")]
		public string ProducerKey { get; set; }
		[JsonProperty("is_active")]
		public bool? IsActive { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("unpaid_blocks")]
		public UInt32? UnpaidBlocks { get; set; }
		[JsonProperty("last_claim_time")]
		public string LastClaimTime { get; set; }
		[JsonProperty("location")]
		public UInt32 Location { get; set; }
    }
	[Serializable]
	public class ScheduleProducers
    {
		[JsonProperty("producer_name")]
		public string ProducerName { get; set; }
		[JsonProperty("block_signing_key")]
		public string BlockSigningKey { get; set; }
    }
	[Serializable]
	public class Schedule
    {
		[JsonProperty("version")]
		public UInt32? Version { get; set; }
		[JsonProperty("producers")]
		public List<ScheduleProducers> Producers { get; set; }
    }
	[Serializable]
	public class PermissionLevel
    {
		[JsonProperty("actor")]
		public string Actor { get; set; }
		[JsonProperty("permission")]
		public string Permission { get; set; }
    }
	[Serializable]
	public class Extension
    {
		[JsonProperty("type")]
		public UInt16 Type { get; set; }
		[JsonProperty("data")]
		public object Data { get; set; }
    }
	[Serializable]
	public class Action
    {
		[JsonProperty("account")]
		public string Account { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("authorization")]
		public List<PermissionLevel> Authorization { get; set; }
		[JsonProperty("data")]
		public object Data { get; set; }
		[JsonProperty("hex_data")]
		public string HexData { get; set; }
    }
	[Serializable]
	public class Transaction
    {
		[JsonProperty("expiration")]
		public DateTime Expiration { get; set; }
		[JsonProperty("ref_block_num")]
		public UInt16 RefBlockNum { get; set; }
		[JsonProperty("ref_block_prefix")]
		public UInt32 RefBlockPrefix { get; set; }
		[JsonProperty("max_net_usage_words")]
		public UInt32 MaxNetUsageWords { get; set; }
		[JsonProperty("max_cpu_usage_ms")]
		public byte MaxCpuUsageMs { get; set; }
		[JsonProperty("delay_sec")]
		public UInt32 DelaySec { get; set; }
		[JsonProperty("context_free_actions")]
		public List<Action> ContextFreeActions { get; set; }
		[JsonProperty("actions")]
		public List<Action> Actions { get; set; }
		[JsonProperty("transaction_extensions")]
		public List<Extension> TransactionExtensions { get; set; }
    }
	[Serializable]
	public class ScheduledTransaction
    {
		[JsonProperty("trx_id")]
		public string TrxId { get; set; }
		[JsonProperty("sender")]
		public string Sender { get; set; }
		[JsonProperty("sender_id")]
		public string SenderId { get; set; }
		[JsonProperty("payer")]
		public string Payer { get; set; }
		[JsonProperty("delay_until")]
		public DateTime? DelayUntil { get; set; }
		[JsonProperty("expiration")]
		public DateTime? Expiration { get; set; }
		[JsonProperty("published")]
		public DateTime? Published { get; set; }
		[JsonProperty("transaction")]
		public Transaction Transaction { get; set; }
    }
	[Serializable]
	public class Receipt
    {
		[JsonProperty("receiver")]
		public string Receiver { get; set; }
		[JsonProperty("act_digest")]
		public string ActDigest { get; set; }
		[JsonProperty("global_sequence")]
		public UInt64? GlobalSequence { get; set; }
		[JsonProperty("recv_sequence")]
		public UInt64? RecvSequence { get; set; }
		[JsonProperty("auth_sequence")]
		public object AuthSequence { get; set; }
		[JsonProperty("code_sequence")]
		public UInt64? CodeSequence { get; set; }
		[JsonProperty("abi_sequence")]
		public UInt64? AbiSequence { get; set; }
    }
	[Serializable]
	public class ActionTrace
    {
		[JsonProperty("receipt")]
		public Receipt Receipt { get; set; }
		[JsonProperty("act")]
		public Action Act { get; set; }
		[JsonProperty("elapsed")]
		public UInt32? Elapsed { get; set; }
		[JsonProperty("cpu_usage")]
		public UInt32? CpuUsage { get; set; }
		[JsonProperty("console")]
		public string Console { get; set; }
		[JsonProperty("total_cpu_usage")]
		public UInt32? TotalCpuUsage { get; set; }
		[JsonProperty("trx_id")]
		public string TrxId { get; set; }
		[JsonProperty("inline_traces")]
		public List<ActionTrace> InlineTraces { get; set; }
    }
	[Serializable]
	public class GlobalAction
    {
		[JsonProperty("global_action_seq")]
		public UInt64? GlobalActionSeq { get; set; }
		[JsonProperty("account_action_seq")]
		public UInt64? AccountActionSeq { get; set; }
		[JsonProperty("block_num")]
		public UInt32? BlockNum { get; set; }
		[JsonProperty("block_time")]
		public DateTime? BlockTime { get; set; }
		[JsonProperty("action_trace")]
		public ActionTrace ActionTrace { get; set; }
    }
	#endregion

	#region generate api method types
	[Serializable]
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

	[Serializable]
    public class GetAccountRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
    }
	[Serializable]
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

	[Serializable]
    public class GetCodeRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("code_as_wasm")]   
		public bool? CodeAsWasm { get; set; }
    }
	[Serializable]
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

	[Serializable]
    public class GetAbiRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
    }
	[Serializable]
    public class GetAbiResponse
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("abi")]   
		public Abi Abi { get; set; }
    }

	[Serializable]
    public class GetRawCodeAndAbiRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
    }
	[Serializable]
    public class GetRawCodeAndAbiResponse
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("wasm")]   
		public string Wasm { get; set; }
		[JsonProperty("abi")]   
		public string Abi { get; set; }
    }

	[Serializable]
    public class AbiJsonToBinRequest
    {
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("action")]   
		public string Action { get; set; }
		[JsonProperty("args")]   
		public object Args { get; set; }
    }
	[Serializable]
    public class AbiJsonToBinResponse
    {
		[JsonProperty("binargs")]   
		public string Binargs { get; set; }
    }

	[Serializable]
    public class AbiBinToJsonRequest
    {
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("action")]   
		public string Action { get; set; }
		[JsonProperty("binargs")]   
		public string Binargs { get; set; }
    }
	[Serializable]
    public class AbiBinToJsonResponse
    {
		[JsonProperty("args")]   
		public object Args { get; set; }
    }

	[Serializable]
    public class GetRequiredKeysRequest
    {
		[JsonProperty("transaction")]   
		public string Transaction { get; set; }
		[JsonProperty("available_keys")]   
		public string AvailableKeys { get; set; }
    }
	[Serializable]
    public class GetRequiredKeysResponse
    {
    }

	[Serializable]
    public class GetBlockRequest
    {
		[JsonProperty("block_num_or_id")]   
		public string BlockNumOrId { get; set; }
    }
	[Serializable]
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

	[Serializable]
    public class GetBlockHeaderStateRequest
    {
		[JsonProperty("block_num_or_id")]   
		public string BlockNumOrId { get; set; }
    }
	[Serializable]
    public class GetBlockHeaderStateResponse
    {
    }

	[Serializable]
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
	[Serializable]
    public class GetTableRowsResponse
    {
		[JsonProperty("rows")]   
		public List<object> Rows { get; set; }
		[JsonProperty("more")]   
		public bool? More { get; set; }
    }

	[Serializable]
    public class GetCurrencyBalanceRequest
    {
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("account")]   
		public string Account { get; set; }
		[JsonProperty("symbol")]   
		public string Symbol { get; set; }
    }
	[Serializable]
    public class GetCurrencyBalanceResponse
    {
		[JsonProperty("assets")]   
		public List<string> Assets { get; set; }
    }

	[Serializable]
    public class GetCurrencyStatsRequest
    {
		[JsonProperty("code")]   
		public string Code { get; set; }
		[JsonProperty("symbol")]   
		public string Symbol { get; set; }
    }
	[Serializable]
    public class GetCurrencyStatsResponse
    {
		[JsonProperty("stats")]   
		public Dictionary<string, CurrencyStat> Stats { get; set; }
    }

	[Serializable]
    public class GetProducersRequest
    {
		[JsonProperty("json")]   
		public bool? Json { get; set; } = false;
		[JsonProperty("lower_bound")]   
		public string LowerBound { get; set; }
		[JsonProperty("limit")]   
		public UInt32? Limit { get; set; } = 50;
    }
	[Serializable]
    public class GetProducersResponse
    {
		[JsonProperty("rows")]   
		public List<Producer> Rows { get; set; }
		[JsonProperty("total_producer_vote_weight")]   
		public double? TotalProducerVoteWeight { get; set; }
		[JsonProperty("more")]   
		public string More { get; set; }
    }

	[Serializable]
    public class GetProducerScheduleResponse
    {
		[JsonProperty("active")]   
		public Schedule Active { get; set; }
		[JsonProperty("pending")]   
		public Schedule Pending { get; set; }
		[JsonProperty("proposed")]   
		public Schedule Proposed { get; set; }
    }

	[Serializable]
    public class GetScheduledTransactionsRequest
    {
		[JsonProperty("json")]   
		public bool? Json { get; set; } = false;
		[JsonProperty("lower_bound")]   
		public string LowerBound { get; set; }
		[JsonProperty("limit")]   
		public UInt32? Limit { get; set; } = 50;
    }
	[Serializable]
    public class GetScheduledTransactionsResponse
    {
		[JsonProperty("transactions")]   
		public List<ScheduledTransaction> Transactions { get; set; }
		[JsonProperty("more")]   
		public string More { get; set; }
    }

	[Serializable]
    public class PushTransactionRequest
    {
		[JsonProperty("signatures")]   
		public string[] Signatures { get; set; }
		[JsonProperty("compression")]   
		public UInt32? Compression { get; set; }
		[JsonProperty("packed_context_free_data")]   
		public string PackedContextFreeData { get; set; }
		[JsonProperty("packed_trx")]   
		public string PackedTrx { get; set; }
    }
	[Serializable]
    public class PushTransactionResponse
    {
		[JsonProperty("transaction_id")]   
		public string TransactionId { get; set; }
		[JsonProperty("processed")]   
		public string Processed { get; set; }
    }

	[Serializable]
    public class GetActionsRequest
    {
		[JsonProperty("account_name")]   
		public string AccountName { get; set; }
		[JsonProperty("pos")]   
		public Int32? Pos { get; set; }
		[JsonProperty("offset")]   
		public Int32? Offset { get; set; }
    }
	[Serializable]
    public class GetActionsResponse
    {
		[JsonProperty("actions")]   
		public List<GlobalAction> Actions { get; set; }
		[JsonProperty("last_irreversible_block")]   
		public UInt32? LastIrreversibleBlock { get; set; }
		[JsonProperty("time_limit_exceeded_error")]   
		public bool? TimeLimitExceededError { get; set; }
    }

	[Serializable]
    public class GetTransactionRequest
    {
		[JsonProperty("id")]   
		public string Id { get; set; }
		[JsonProperty("block_num_hint")]   
		public string BlockNumHint { get; set; }
    }
	[Serializable]
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

	[Serializable]
    public class GetKeyAccountsRequest
    {
		[JsonProperty("public_key")]   
		public string PublicKey { get; set; }
    }
	[Serializable]
    public class GetKeyAccountsResponse
    {
		[JsonProperty("account_names")]   
		public List<string> AccountNames { get; set; }
    }

	[Serializable]
    public class GetControlledAccountsRequest
    {
		[JsonProperty("controlling_account")]   
		public string ControllingAccount { get; set; }
    }
	[Serializable]
    public class GetControlledAccountsResponse
    {
		[JsonProperty("controlled_accounts")]   
		public List<string> ControlledAccounts { get; set; }
    }

	#endregion
}


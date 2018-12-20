  

  

// Auto Generated, do not edit.
using EosSharp.Core.DataAttributes;
using System;
using System.Collections.Generic;

namespace EosSharp.Core.Api.v1
{
	#region generate api types
	[Serializable]
	public class Symbol
    {
		
		public string Name { get; set; }
		
		public byte Precision { get; set; }
    }
	[Serializable]
	public class Resource
    {
		
		public Int64 Used { get; set; }
		
		public Int64 Available { get; set; }
		
		public Int64 Max { get; set; }
    }
	[Serializable]
	public class AuthorityKey
    {
		
		public string Key { get; set; }
		
		public Int32 Weight { get; set; }
    }
	[Serializable]
	public class AuthorityAccount
    {
		
		public string Account { get; set; }
		
		public Int32 Weight { get; set; }
    }
	[Serializable]
	public class AuthorityWait
    {
		
		public string WaitSec { get; set; }
		
		public Int32 Weight { get; set; }
    }
	[Serializable]
	public class Authority
    {
		
		public UInt32 Threshold { get; set; }
		
		public List<AuthorityKey> Keys { get; set; }
		
		public List<AuthorityAccount> Accounts { get; set; }
		
		public List<AuthorityWait> Waits { get; set; }
    }
	[Serializable]
	public class Permission
    {
		
		public string PermName { get; set; }
		
		public string Parent { get; set; }
		
		public Authority RequiredAuth { get; set; }
    }
	[Serializable]
	public class AbiType
    {
		
		public string NewTypeName { get; set; }
		
		public string Type { get; set; }
    }
	[Serializable]
	public class AbiField
    {
		
		public string Name { get; set; }
		
		public string Type { get; set; }
    }
	[Serializable]
	public class AbiStruct
    {
		
		public string Name { get; set; }
		
		public string Base { get; set; }
		
		public List<AbiField> Fields { get; set; }
    }
	[Serializable]
	public class AbiAction
    {
		[AbiFieldType("name")]
		public string Name { get; set; }
		
		public string Type { get; set; }
		
		public string RicardianContract { get; set; }
    }
	[Serializable]
	public class AbiTable
    {
		[AbiFieldType("name")]
		public string Name { get; set; }
		
		public string IndexType { get; set; }
		
		public List<string> KeyNames { get; set; }
		
		public List<string> KeyTypes { get; set; }
		
		public string Type { get; set; }
    }
	[Serializable]
	public class Abi
    {
		
		public string Version { get; set; }
		
		public List<AbiType> Types { get; set; }
		
		public List<AbiStruct> Structs { get; set; }
		
		public List<AbiAction> Actions { get; set; }
		
		public List<AbiTable> Tables { get; set; }
		
		public List<AbiRicardianClause> RicardianClauses { get; set; }
		
		public List<string> ErrorMessages { get; set; }
		
		public List<Extension> AbiExtensions { get; set; }
		
		public List<Variant> Variants { get; set; }
    }
	[Serializable]
	public class AbiRicardianClause
    {
		
		public string Id { get; set; }
		
		public string Body { get; set; }
    }
	[Serializable]
	public class CurrencyStat
    {
		
		public string Supply { get; set; }
		
		public string MaxSupply { get; set; }
		
		public string Issuer { get; set; }
    }
	[Serializable]
	public class Producer
    {
		[AbiFieldType("name")]
		public string Owner { get; set; }
		[AbiFieldType("float64")]
		public double TotalVotes { get; set; }
		[AbiFieldType("public_key")]
		public string ProducerKey { get; set; }
		
		public bool IsActive { get; set; }
		
		public string Url { get; set; }
		
		public UInt32 UnpaidBlocks { get; set; }
		
		public UInt64 LastClaimTime { get; set; }
		
		public UInt16 Location { get; set; }
    }
	[Serializable]
	public class ScheduleProducers
    {
		
		public string ProducerName { get; set; }
		
		public string BlockSigningKey { get; set; }
    }
	[Serializable]
	public class Schedule
    {
		
		public UInt32? Version { get; set; }
		
		public List<ScheduleProducers> Producers { get; set; }
    }
	[Serializable]
	public class PermissionLevel
    {
		
		public string Actor { get; set; }
		
		public string Permission { get; set; }
    }
	[Serializable]
	public class Extension
    {
		
		public UInt16 Type { get; set; }
		
		public byte[] Data { get; set; }
    }
	[Serializable]
	public class Variant
    {
		
		public string Name { get; set; }
		
		public List<string> Type { get; set; }
    }
	[Serializable]
	public class Action
    {
		
		public string Account { get; set; }
		
		public string Name { get; set; }
		
		public List<PermissionLevel> Authorization { get; set; }
		
		public object Data { get; set; }
		
		public string HexData { get; set; }
    }
	[Serializable]
	public class Transaction
    {
		
		public DateTime Expiration { get; set; }
		
		public UInt16 RefBlockNum { get; set; }
		
		public UInt32 RefBlockPrefix { get; set; }
		
		public UInt32 MaxNetUsageWords { get; set; }
		
		public byte MaxCpuUsageMs { get; set; }
		
		public UInt32 DelaySec { get; set; }
		
		public List<Action> ContextFreeActions { get; set; } = new List<Action>();
		
		public List<Action> Actions { get; set; } = new List<Action>();
		
		public List<Extension> TransactionExtensions { get; set; } = new List<Extension>();
    }
	[Serializable]
	public class ScheduledTransaction
    {
		
		public string TrxId { get; set; }
		
		public string Sender { get; set; }
		
		public string SenderId { get; set; }
		
		public string Payer { get; set; }
		
		public DateTime? DelayUntil { get; set; }
		
		public DateTime? Expiration { get; set; }
		
		public DateTime? Published { get; set; }
		
		public Object Transaction { get; set; }
    }
	[Serializable]
	public class Receipt
    {
		
		public string Receiver { get; set; }
		
		public string ActDigest { get; set; }
		
		public UInt64? GlobalSequence { get; set; }
		
		public UInt64? RecvSequence { get; set; }
		
		public List<List<object>> AuthSequence { get; set; }
		
		public UInt64? CodeSequence { get; set; }
		
		public UInt64? AbiSequence { get; set; }
    }
	[Serializable]
	public class ActionTrace
    {
		
		public Receipt Receipt { get; set; }
		
		public Action Act { get; set; }
		
		public UInt32? Elapsed { get; set; }
		
		public UInt32? CpuUsage { get; set; }
		
		public string Console { get; set; }
		
		public UInt32? TotalCpuUsage { get; set; }
		
		public string TrxId { get; set; }
		
		public List<ActionTrace> InlineTraces { get; set; }
    }
	[Serializable]
	public class GlobalAction
    {
		
		public UInt64? GlobalActionSeq { get; set; }
		
		public UInt64? AccountActionSeq { get; set; }
		
		public UInt32? BlockNum { get; set; }
		
		public DateTime? BlockTime { get; set; }
		
		public ActionTrace ActionTrace { get; set; }
    }
	[Serializable]
	public class TransactionReceipt
    {
		
		public string Status { get; set; }
		
		public UInt32? CpuUsageUs { get; set; }
		
		public UInt32? NetUsageWords { get; set; }
		
		public object Trx { get; set; }
    }
	[Serializable]
	public class ProcessedTransaction
    {
		
		public string Id { get; set; }
		
		public TransactionReceipt Receipt { get; set; }
		
		public UInt32 Elapsed { get; set; }
		
		public UInt32 NetUsage { get; set; }
		
		public bool Scheduled { get; set; }
		
		public string Except { get; set; }
		
		public List<ActionTrace> ActionTraces { get; set; }
    }
	[Serializable]
	public class DetailedTransaction
    {
		
		public TransactionReceipt Receipt { get; set; }
		
		public Transaction Trx { get; set; }
    }
	[Serializable]
	public class PackedTransaction
    {
		
		public string Compression { get; set; }
		
		public List<object> ContextFreeData { get; set; }
		
		public string Id { get; set; }
		
		public string PackedContextFreeData { get; set; }
		
		public string PackedTrx { get; set; }
		
		public List<string> Signatures { get; set; }
		
		public Transaction Transaction { get; set; }
    }
	[Serializable]
	public class RefundRequest
    {
		
		public string CpuAmount { get; set; }
		
		public string NetAmount { get; set; }
    }
	[Serializable]
	public class SelfDelegatedBandwidth
    {
		
		public string CpuWeight { get; set; }
		
		public string From { get; set; }
		
		public string NetWeight { get; set; }
		
		public string To { get; set; }
    }
	[Serializable]
	public class TotalResources
    {
		
		public string CpuWeight { get; set; }
		
		public string NetWeight { get; set; }
		
		public string Owner { get; set; }
		
		public UInt64? RamBytes { get; set; }
    }
	[Serializable]
	public class VoterInfo
    {
		
		public bool? IsProxy { get; set; }
		
		public double? LastVoteWeight { get; set; }
		
		public string Owner { get; set; }
		
		public List<string> Producers { get; set; }
		
		public double? ProxiedVoteWeight { get; set; }
		
		public string Proxy { get; set; }
		
		public UInt64? Staked { get; set; }
    }
	[Serializable]
	public class ExtendedAsset
    {
		
		public string Quantity { get; set; }
		
		public string Contract { get; set; }
    }
	[Serializable]
	public class TableByScopeResultRow
    {
		
		public string Code { get; set; }
		
		public string Scope { get; set; }
		
		public string Table { get; set; }
		
		public string Payer { get; set; }
		
		public UInt32? Count { get; set; }
    }
	#endregion

	#region generate api method types
	[Serializable]
    public class GetInfoResponse
    {
 
		public string ServerVersion { get; set; }
 
		public string ChainId { get; set; }
 
		public UInt32? HeadBlockNum { get; set; }
 
		public UInt32? LastIrreversibleBlockNum { get; set; }
 
		public string LastIrreversibleBlockId { get; set; }
 
		public string HeadBlockId { get; set; }
 
		public DateTime? HeadBlockTime { get; set; }
 
		public string HeadBlockProducer { get; set; }
 
		public string VirtualBlockCpuLimit { get; set; }
 
		public string VirtualBlockNetLimit { get; set; }
 
		public string BlockCpuLimit { get; set; }
 
		public string BlockNetLimit { get; set; }
    }
	[Serializable]
    public class GetAccountRequest
    {
		public string AccountName { get; set; }
    }
	[Serializable]
    public class GetAccountResponse
    {
 
		public string AccountName { get; set; }
 
		public UInt32? HeadBlockNum { get; set; }
 
		public DateTime? HeadBlockTime { get; set; }
 
		public bool? Privileged { get; set; }
 
		public DateTime? LastCodeUpdate { get; set; }
 
		public DateTime? Created { get; set; }
 
		public Int64? RamQuota { get; set; }
 
		public Int64? NetWeight { get; set; }
 
		public Int64? CpuWeight { get; set; }
 
		public Resource NetLimit { get; set; }
 
		public Resource CpuLimit { get; set; }
 
		public UInt64? RamUsage { get; set; }
 
		public List<Permission> Permissions { get; set; }
 
		public RefundRequest RefundRequest { get; set; }
 
		public SelfDelegatedBandwidth SelfDelegatedBandwidth { get; set; }
 
		public TotalResources TotalResources { get; set; }
 
		public VoterInfo VoterInfo { get; set; }
    }
	[Serializable]
    public class GetCodeRequest
    {
		public string AccountName { get; set; }
		public bool? CodeAsWasm { get; set; }
    }
	[Serializable]
    public class GetCodeResponse
    {
 
		public string AccountName { get; set; }
 
		public string Wast { get; set; }
 
		public string Wasm { get; set; }
 
		public string CodeHash { get; set; }
 
		public Abi Abi { get; set; }
    }
	[Serializable]
    public class GetAbiRequest
    {
		public string AccountName { get; set; }
    }
	[Serializable]
    public class GetAbiResponse
    {
 
		public string AccountName { get; set; }
 
		public Abi Abi { get; set; }
    }
	[Serializable]
    public class GetRawCodeAndAbiRequest
    {
		public string AccountName { get; set; }
    }
	[Serializable]
    public class GetRawCodeAndAbiResponse
    {
 
		public string AccountName { get; set; }
 
		public string Wasm { get; set; }
 
		public string Abi { get; set; }
    }
	[Serializable]
    public class GetRawAbiRequest
    {
		public string AccountName { get; set; }
		public string AbiHash { get; set; }
    }
	[Serializable]
    public class GetRawAbiResponse
    {
 
		public string AccountName { get; set; }
 
		public string CodeHash { get; set; }
 
		public string AbiHash { get; set; }
 
		public string Abi { get; set; }
    }
	[Serializable]
    public class AbiJsonToBinRequest
    {
		public string Code { get; set; }
		public string Action { get; set; }
		public object Args { get; set; }
    }
	[Serializable]
    public class AbiJsonToBinResponse
    {
 
		public string Binargs { get; set; }
    }
	[Serializable]
    public class AbiBinToJsonRequest
    {
		public string Code { get; set; }
		public string Action { get; set; }
		public string Binargs { get; set; }
    }
	[Serializable]
    public class AbiBinToJsonResponse
    {
 
		public object Args { get; set; }
    }
	[Serializable]
    public class GetRequiredKeysRequest
    {
		public Transaction Transaction { get; set; }
		public List<string> AvailableKeys { get; set; }
    }
	[Serializable]
    public class GetRequiredKeysResponse
    {
 
		public List<string> RequiredKeys { get; set; }
    }
	[Serializable]
    public class GetBlockRequest
    {
		public string BlockNumOrId { get; set; }
    }
	[Serializable]
    public class GetBlockResponse
    {
 
		public DateTime? Timestamp { get; set; }
 
		public string Producer { get; set; }
 
		public UInt32 Confirmed { get; set; }
 
		public string Previous { get; set; }
 
		public string TransactionMroot { get; set; }
 
		public string ActionMroot { get; set; }
 
		public UInt32 ScheduleVersion { get; set; }
 
		public string NewProducers { get; set; }
 
		public List<Extension> BlockExtensions { get; set; }
 
		public List<Extension> HeaderExtensions { get; set; }
 
		public string ProducerSignature { get; set; }
 
		public List<TransactionReceipt> Transactions { get; set; }
 
		public string Id { get; set; }
 
		public UInt32 BlockNum { get; set; }
 
		public UInt32 RefBlockPrefix { get; set; }
    }
	[Serializable]
    public class GetBlockHeaderStateRequest
    {
		public string BlockNumOrId { get; set; }
    }
	[Serializable]
    public class GetBlockHeaderStateResponse
    {
    }
	[Serializable]
    public class GetTableRowsRequest
    {
		public bool? Json { get; set; } = false;
		public string Code { get; set; }
		public string Scope { get; set; }
		public string Table { get; set; }
		public string TableKey { get; set; }
		public string LowerBound { get; set; } = "0";
		public string UpperBound { get; set; } = "-1";
		public Int32? Limit { get; set; } = 10;
		public string KeyType { get; set; }
		public string IndexPosition { get; set; }
		public string EncodeType { get; set; } = "dec";
		public bool? Reverse { get; set; }
		public bool? ShowPayer { get; set; }
    }
	[Serializable]
    public class GetTableRowsResponse
    {
 
		public List<object> Rows { get; set; }
 
		public bool? More { get; set; }
    }
	[Serializable]
    public class GetTableRowsResponse<TRowType>
    {
   
		public List<TRowType> Rows { get; set; }
   
		public bool? More { get; set; }
    }
	[Serializable]
    public class GetTableByScopeRequest
    {
		public string Code { get; set; }
		public string Table { get; set; }
		public string LowerBound { get; set; }
		public string UpperBound { get; set; }
		public Int32? Limit { get; set; } = 10;
		public bool? Reverse { get; set; }
    }
	[Serializable]
    public class GetTableByScopeResponse
    {
 
		public List<TableByScopeResultRow> Rows { get; set; }
 
		public string More { get; set; }
    }
	[Serializable]
    public class GetCurrencyBalanceRequest
    {
		public string Code { get; set; }
		public string Account { get; set; }
		public string Symbol { get; set; }
    }
	[Serializable]
    public class GetCurrencyBalanceResponse
    {
 
		public List<string> Assets { get; set; }
    }
	[Serializable]
    public class GetCurrencyStatsRequest
    {
		public string Code { get; set; }
		public string Symbol { get; set; }
    }
	[Serializable]
    public class GetCurrencyStatsResponse
    {
 
		public Dictionary<string, CurrencyStat> Stats { get; set; }
    }
	[Serializable]
    public class GetProducersRequest
    {
		public bool? Json { get; set; } = false;
		public string LowerBound { get; set; }
		public Int32? Limit { get; set; } = 50;
    }
	[Serializable]
    public class GetProducersResponse
    {
 
		public List<object> Rows { get; set; }
 
		public double? TotalProducerVoteWeight { get; set; }
 
		public string More { get; set; }
    }
	[Serializable]
    public class GetProducerScheduleResponse
    {
 
		public Schedule Active { get; set; }
 
		public Schedule Pending { get; set; }
 
		public Schedule Proposed { get; set; }
    }
	[Serializable]
    public class GetScheduledTransactionsRequest
    {
		public bool? Json { get; set; } = false;
		public string LowerBound { get; set; }
		public Int32? Limit { get; set; } = 50;
    }
	[Serializable]
    public class GetScheduledTransactionsResponse
    {
 
		public List<ScheduledTransaction> Transactions { get; set; }
 
		public string More { get; set; }
    }
	[Serializable]
    public class PushTransactionRequest
    {
		public string[] Signatures { get; set; }
		public UInt32? Compression { get; set; }
		public string PackedContextFreeData { get; set; }
		public string PackedTrx { get; set; }
		public Transaction Transaction { get; set; }
    }
	[Serializable]
    public class PushTransactionResponse
    {
 
		public string TransactionId { get; set; }
 
		public ProcessedTransaction Processed { get; set; }
    }
	[Serializable]
    public class GetActionsRequest
    {
		public string AccountName { get; set; }
		public Int32? Pos { get; set; }
		public Int32? Offset { get; set; }
    }
	[Serializable]
    public class GetActionsResponse
    {
 
		public List<GlobalAction> Actions { get; set; }
 
		public UInt32? LastIrreversibleBlock { get; set; }
 
		public bool? TimeLimitExceededError { get; set; }
    }
	[Serializable]
    public class GetTransactionRequest
    {
		public string Id { get; set; }
		public string BlockNumHint { get; set; }
    }
	[Serializable]
    public class GetTransactionResponse
    {
 
		public string Id { get; set; }
 
		public DetailedTransaction Trx { get; set; }
 
		public DateTime? BlockTime { get; set; }
 
		public UInt32? BlockNum { get; set; }
 
		public UInt32? LastIrreversibleBlock { get; set; }
 
		public List<ActionTrace> Traces { get; set; }
    }
	[Serializable]
    public class GetKeyAccountsRequest
    {
		public string PublicKey { get; set; }
    }
	[Serializable]
    public class GetKeyAccountsResponse
    {
 
		public List<string> AccountNames { get; set; }
    }
	[Serializable]
    public class GetControlledAccountsRequest
    {
		public string ControllingAccount { get; set; }
    }
	[Serializable]
    public class GetControlledAccountsResponse
    {
 
		public List<string> ControlledAccounts { get; set; }
    }
	#endregion
}


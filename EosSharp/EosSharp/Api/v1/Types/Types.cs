using Newtonsoft.Json;

namespace EosSharp.Api.v1.Types
{
    public class GetInfoResult
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

}


using System;
using System.Collections.Generic;

namespace EosSharp.Core.Api.v1.Trace
{
	#region trace api types
	[Serializable]
    public class GetBlockTraceRequest
    {
		public string block_num;
    }

    public static class BlockStatuses 
    {
        public const string Irreversible = "irreversible";
    }

	[Serializable]
    public class GetBlockTraceResponse
    {
		public string id { get; set; }
		public UInt32 number { get; set; }
		public string previous_id { get; set; }
		public string status { get; set; }
		public DateTime timestamp { get; set; }
		public string producer { get; set; }
		public List<Transaction> transactions { get; set; }
	}	

    [Serializable]
    public class Transaction
    {
        public string id { get; set; }
        public List<Action> actions { get; set; }
    }

    [Serializable]
    public class Action
    {
        public string receiver { get; set; }
        public string account { get; set; }
        public string action { get; set; }
        public List<Authorization> authorization { get; set; }
        public string data { get; set; }
        public Params @params { get; set; }
    }

    [Serializable]
    public class Authorization
    {
        public string account { get; set; }
        public string permission { get; set; }
    }

    [Serializable]
    public class Params
    {
        public string from { get; set; }
        public string to { get; set; }
        public string quantity { get; set; }
        public string memo { get; set; }
    }
	#endregion
}
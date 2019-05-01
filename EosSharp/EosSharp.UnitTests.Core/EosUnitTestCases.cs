using EosSharp.Core;
using EosSharp.Core.Api.v1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EosSharp.UnitTests
{
    public class EosUnitTestCases
    {
        EosBase Eos { get; set; }
        public EosUnitTestCases(EosBase eos)
        {
            Eos = eos;
        }

        public Task GetBlock()
        {
            return Eos.GetBlock("13503532");
        }

        public Task GetTableRows()
        {
            return Eos.GetTableRows(new GetTableRowsRequest()
            {
                json = false,
                code = "eosio.token",
                scope = "EOS",
                table = "stat"
            });
        }

        [Serializable]
        class Stat
        {
            public string issuer { get; set; }
            public string max_supply { get; set; }
            public string supply { get; set; }
        }

        public Task GetTableRowsGeneric()
        {
            return Eos.GetTableRows<Stat>(new GetTableRowsRequest()
            {
                json = true,
                code = "eosio.token",
                scope = "EOS",
                table = "stat"
            });
        }

        public Task GetProducers()
        {
            return Eos.GetProducers(new GetProducersRequest()
            {
                json = false
            });
        }

        public Task GetScheduledTransactions()
        {
            return Eos.GetScheduledTransactions(new GetScheduledTransactionsRequest()
            {
                json = false
            });
        }

        public Task CreateTransactionArrayData()
        {
            return Eos.CreateTransaction(new Transaction()
            {
                actions = new List<Core.Api.v1.Action>()
                {
                    new Core.Api.v1.Action()
                    {
                        account = "platform",
                        authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {actor = "player1", permission = "active" }
                        },
                        name = "testarr",
                        data = new { user = "player1", array = new List<UInt64>() { 1, 6, 3} }
                        //data = new { user = "player1", array = new UInt64[] { 1, 6, 3} }
                        //data = new { user = "player1", array = new Queue<UInt64>(new UInt64[] { 1, 6, 3}) }
                        //data = new { user = "player1", array = new Stack<UInt64>(new UInt64[] { 1, 6, 3}) }
                        //data = new { user = "player1", array = new ArrayList() { 1, 6, 3} }
                    }
                }
            });
        }

        public Task CreateTransactionActionArrayStructData()
        {
            var args = new List<object>()
            { 
                {
                    new Dictionary<string, object>()
                    {
                        { "air_id", Convert.ToUInt64("8") },
                        { "air_place", Convert.ToString("监测点8888") },
                        { "air_pm2_5", Convert.ToString("pm2.5数值") },
                        { "air_voc", Convert.ToString("voc数值") },
                        { "air_carbon", Convert.ToString("碳数值") },
                        { "air_nitrogen", Convert.ToString("氮数值") },
                        { "air_sulfur", Convert.ToString("硫数值") },
                        { "air_longitude", Convert.ToString("经度") },
                        { "air_latitude", Convert.ToString("纬度") }
                    }
                }
            };

            return Eos.CreateTransaction(new Transaction()
            {
                actions = new List<Core.Api.v1.Action>()
                {
                    new Core.Api.v1.Action()
                    {
                        account = "platform",
                        authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {actor = "player1", permission = "active" }
                        },
                        name = "airquality",
                        data = new {
                            aql = args,
                            a = args,
                            b = args
                        }
                    }
                }
            });
        }

        public Task CreateTransactionAnonymousObjectData()
        {
            return Eos.CreateTransaction(new Transaction()
            {
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
            });
        }

        public Task CreateTransaction()
        {
            return Eos.CreateTransaction(new Transaction()
            {
                actions = new List<Core.Api.v1.Action>()
                {
                    new Core.Api.v1.Action()
                    {
                        account = "eosio.token",
                        authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {actor = "bensigbensig", permission = "active" }
                        },
                        name = "transfer",
                        data = new Dictionary<string, string>()
                        {
                            { "from", "bensigbensig" },
                            { "to", "bluchain1234" },
                            { "quantity", "0.0001 EOS" },
                            { "memo", "hello crypto world!" }
                        }
                    }
                }
            });
        }

        public Task CreateNewAccount()
        {
            return Eos.CreateTransaction(new Transaction()
            {
                actions = new List<Core.Api.v1.Action>()
                {
                    new Core.Api.v1.Action()
                    {
                        account = "eosio",
                        authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() {actor = "tester112345", permission = "active"}
                        },
                        name = "newaccount",
                        data = new Dictionary<string, object>() {
                            { "creator", "tester112345" },
                            { "name", "mynewaccount" },
                            { "owner", new Dictionary<string, object>() {
                                { "threshold", 1 },
                                { "keys", new List<object>() {
                                    new Dictionary<string, object>() {
                                        { "key", "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" },
                                        { "weight", 1}
                                    }
                                }},
                                { "accounts", new List<object>() },
                                { "waits", new List<object>() }
                            }},
                            { "active", new Dictionary<string, object>() {
                                { "threshold", 1 },
                                { "keys", new List<object>() {
                                    new Dictionary<string, object>() {
                                        { "key", "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" },
                                        { "weight", 1}
                                    }
                                }},
                                { "accounts",  new List<object>() },
                                { "waits", new List<object>() }
                            }}
                        }
                    },
                    new Core.Api.v1.Action()
                    {
                        account = "eosio",
                        authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() { actor = "tester112345", permission = "active"}
                        },
                        name = "buyrambytes",
                        data = new Dictionary<string, object>() {
                            { "payer", "tester112345" },
                            { "receiver", "mynewaccount" },
                            { "bytes", 8192 },
                        }
                    },
                    new Core.Api.v1.Action()
                    {
                        account = "eosio",
                        authorization = new List<PermissionLevel>()
                        {
                            new PermissionLevel() { actor = "tester112345", permission = "active"}
                        },
                        name = "delegatebw",
                        data = new Dictionary<string, object>() {
                            { "from", "tester112345" },
                            { "receiver", "mynewaccount" },
                            { "stake_net_quantity", "1.0000 EOS" },
                            { "stake_cpu_quantity", "1.0000 EOS" },
                            { "transfer", false },
                        }
                    }
                }
            });
        }
    }
}

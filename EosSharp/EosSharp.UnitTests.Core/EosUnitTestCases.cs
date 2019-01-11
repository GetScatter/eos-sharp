using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Providers;
using System;
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
                code = "eosio",
                scope = "eosio",
                table = "producers"
            });
        }

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
                            new PermissionLevel() {actor = "tester112345", permission = "active" }
                        },
                        name = "transfer",
                        data = new Dictionary<string, string>()
                        {
                            { "from", "tester112345" },
                            { "to", "tester212345" },
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
                        data = new {
                            creator = "tester112345",
                            name = "mynewaccount",
                            owner = new {
                                threshold = 1,
                                keys = new List<object>() {
                                    new { key = "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr", weight = 1}
                                },
                                accounts =  new List<object>(),
                                waits =  new List<object>()
                            },
                            active = new {
                                threshold = 1,
                                keys = new List<object>() {
                                    new { key = "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr", weight = 1}
                                },
                                accounts =  new List<object>(),
                                waits =  new List<object>()
                            }
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
                        data = new {
                            payer = "tester112345",
                            receiver = "mynewaccount",
                            bytes = 8192,
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
                        data = new {
                            from = "tester112345",
                            receiver = "mynewaccount",
                            stake_net_quantity = "1.0000 EOS",
                            stake_cpu_quantity = "1.0000 EOS",
                            transfer = false,
                        }
                    }
                }
            });
        }
    }
}

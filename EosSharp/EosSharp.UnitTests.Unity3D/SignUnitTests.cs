using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Helpers;
using EosSharp.Core.Providers;
using EosSharp.Unity3D;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EosSharp.UnitTests.Unity3D
{
    public class SignUnitTests
    {
        readonly EosConfigurator EosConfig = null;
        EosApi DefaultApi { get; set; }
        public SignUnitTests()
        {
            EosConfig = new EosConfigurator()
            {
                SignProvider = new DefaultSignProvider("5K57oSZLpfzePvQNpsLS6NfKXLhhRARNU13q6u2ZPQCGHgKLbTA"),

                //HttpEndpoint = "https://nodes.eos42.io", //Mainnet
                //ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"

                HttpEndpoint = "https://nodeos01.btuga.io",
                ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f"
            };
            DefaultApi = new EosApi(EosConfig, new HttpHelper());
        }

        public void GenerateKeyPair()
        {
            Console.WriteLine(JsonConvert.SerializeObject(CryptoHelper.GenerateKeyPair("R1")));
            Console.WriteLine(JsonConvert.SerializeObject(CryptoHelper.GenerateKeyPair()));
        }

        public void Base64ToByteArray()
        {
            string base64EncodedData = "DmVvc2lvOjphYmkvMS4wAQxhY2NvdW50X25hbWUEbmFtZQcIdHJhbnNmZXIABARmcm9tDGFjY291bnRfbmFtZQJ0bwxhY2NvdW50X25hbWUIcXVhbnRpdHkFYXNzZXQEbWVtbwZzdHJpbmcGY3JlYXRlAAIGaXNzdWVyDGFjY291bnRfbmFtZQ5tYXhpbXVtX3N1cHBseQVhc3NldAVpc3N1ZQADAnRvDGFjY291bnRfbmFtZQhxdWFudGl0eQVhc3NldARtZW1vBnN0cmluZwZyZXRpcmUAAghxdWFudGl0eQVhc3NldARtZW1vBnN0cmluZwVjbG9zZQACBW93bmVyDGFjY291bnRfbmFtZQZzeW1ib2wGc3ltYm9sB2FjY291bnQAAQdiYWxhbmNlBWFzc2V0DmN1cnJlbmN5X3N0YXRzAAMGc3VwcGx5BWFzc2V0Cm1heF9zdXBwbHkFYXNzZXQGaXNzdWVyDGFjY291bnRfbmFtZQUAAABXLTzNzQh0cmFuc2ZlcucFIyMgVHJhbnNmZXIgVGVybXMgJiBDb25kaXRpb25zCgpJLCB7e2Zyb219fSwgY2VydGlmeSB0aGUgZm9sbG93aW5nIHRvIGJlIHRydWUgdG8gdGhlIGJlc3Qgb2YgbXkga25vd2xlZGdlOgoKMS4gSSBjZXJ0aWZ5IHRoYXQge3txdWFudGl0eX19IGlzIG5vdCB0aGUgcHJvY2VlZHMgb2YgZnJhdWR1bGVudCBvciB2aW9sZW50IGFjdGl2aXRpZXMuCjIuIEkgY2VydGlmeSB0aGF0LCB0byB0aGUgYmVzdCBvZiBteSBrbm93bGVkZ2UsIHt7dG99fSBpcyBub3Qgc3VwcG9ydGluZyBpbml0aWF0aW9uIG9mIHZpb2xlbmNlIGFnYWluc3Qgb3RoZXJzLgozLiBJIGhhdmUgZGlzY2xvc2VkIGFueSBjb250cmFjdHVhbCB0ZXJtcyAmIGNvbmRpdGlvbnMgd2l0aCByZXNwZWN0IHRvIHt7cXVhbnRpdHl9fSB0byB7e3RvfX0uCgpJIHVuZGVyc3RhbmQgdGhhdCBmdW5kcyB0cmFuc2ZlcnMgYXJlIG5vdCByZXZlcnNpYmxlIGFmdGVyIHRoZSB7e3RyYW5zYWN0aW9uLmRlbGF5fX0gc2Vjb25kcyBvciBvdGhlciBkZWxheSBhcyBjb25maWd1cmVkIGJ5IHt7ZnJvbX19J3MgcGVybWlzc2lvbnMuCgpJZiB0aGlzIGFjdGlvbiBmYWlscyB0byBiZSBpcnJldmVyc2libHkgY29uZmlybWVkIGFmdGVyIHJlY2VpdmluZyBnb29kcyBvciBzZXJ2aWNlcyBmcm9tICd7e3RvfX0nLCBJIGFncmVlIHRvIGVpdGhlciByZXR1cm4gdGhlIGdvb2RzIG9yIHNlcnZpY2VzIG9yIHJlc2VuZCB7e3F1YW50aXR5fX0gaW4gYSB0aW1lbHkgbWFubmVyLgoAAAAAAKUxdgVpc3N1ZQAAAAAAqGzURQZjcmVhdGUAAAAAAKjrsroGcmV0aXJlAAAAAAAAhWlEBWNsb3NlAAIAAAA4T00RMgNpNjQBCGN1cnJlbmN5AQZ1aW50NjQHYWNjb3VudAAAAAAAkE3GA2k2NAEIY3VycmVuY3kBBnVpbnQ2NA5jdXJyZW5jeV9zdGF0cwAAAA===";
            var base64EncodedBytes = SerializationHelper.Base64FcStringToByteArray(base64EncodedData);
        }

        public async Task SignProvider()
        {
            var signProvider = new DefaultSignProvider("5K57oSZLpfzePvQNpsLS6NfKXLhhRARNU13q6u2ZPQCGHgKLbTA");
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
   
            if ((await signProvider.GetAvailableKeys()).All(ak => requiredKeys.Contains(ak)))
                Console.WriteLine("Test SignProvider run successfuly.");
            else
                Console.WriteLine("Test SignProvider run failed.");
        }

        public async Task SignHelloWorld()
        {
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
            var helloBytes = Encoding.UTF8.GetBytes("Hello world!");
            var signatures = await EosConfig.SignProvider.Sign(DefaultApi.Config.ChainId, requiredKeys, helloBytes);

            if (signatures.First() == "SIG_K1_JxtwrzV246xdAgqgH36oX5MjMeg1sEFdUWuwnE9Fhr9eqi5JzgmKXm9UEJgNZMLYdnZhphL1QmE8aW7rTDPC8k8acvkoMR")
                Console.WriteLine("Test SignHelloWorld run successfuly.");
            else
                Console.WriteLine("Test SignHelloWorld run failed.");
        }

        public async Task SignTransaction()
        {
            bool success = false;
            try
            {
                var trx = new Transaction()
                {
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
                };

                var abiSerializer = new AbiSerializationProvider(DefaultApi);
                var packedTrx = await abiSerializer.SerializePackedTransaction(trx);
                var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };
                var signatures = await EosConfig.SignProvider.Sign(DefaultApi.Config.ChainId, requiredKeys, packedTrx);

                success = signatures.First() == "SIG_K1_Jze1PGnAo9MVHkxRxekZQKJebM11AgtK4NhsFtDEZsLujrocvJ5dnhejyr9RQji2K3DWdyUpM9BGyWts7FFr8Wib95hiTj";
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }

            if (success)
                Console.WriteLine("Test SignTransaction run successfuly.");
            else
                Console.WriteLine("Test SignTransaction run failed.");
        }

        public async Task TestAll()
        {
            await SignProvider();
            await SignHelloWorld();
            await SignTransaction();
        }
    }
}

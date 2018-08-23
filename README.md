# eos-sharp
C# client library for EOS blockchains. The library is based on https://github.com/EOSIO/eosjs2

# Prerequisite

Visual Studio 2017 

# Usage

```csharp
Eos eos = new Eos(new EosConfigurator()
{
    SignProvider = new DefaultSignProvider("myprivatekey"),
    HttpEndpoint = "https://nodes.eos42.io", //Mainnet
    ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"
});

var result = await eos.CreateTransaction(new Transaction()
{
    Actions = new List<Api.v1.Action>()
    {
        new Api.v1.Action()
        {
            Account = "eosio.token",
            Authorization = new List<PermissionLevel>()
            {
                new PermissionLevel() {Actor = "tester112345", Permission = "active" }
            },
            Name = "transfer",
            Data = new { from = "tester112345", to = "tester212345", quantity = "0.0001 EOS", memo = "hello crypto world!" }
        }
    }
});
```

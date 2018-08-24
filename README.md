# eos-sharp
C# client library for EOS blockchains. The library is based on https://github.com/EOSIO/eosjs2 and MIT licensed.

# Prerequisite

Visual Studio 2017 

# Usage

The simplest way to do a transaction is to create a new instance of the **Eos** class with a **EosConfigurator** and call **eos.CreateTransaction**

Example:


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


Is also possible to implement your own **ISignProvider** to customize how the signatures and key handling is done.

Example:


```csharp
class SuperSecretSignProvider : ISignProvider
{
   public async Task<IEnumerable<string>> GetAvailableKeys()
   {
        var result = await HttpHelper.GetJsonAsync<SecretResponse>("https://supersecretserver.com/get_available_keys");
        return result.Keys;
   }
   
   public async Task<IEnumerable<string>> Sign(string chainId, List<string> requiredKeys, byte[] signBytes)
   {
        var result = await HttpHelper.PostJsonAsync<SecretSignResponse>("https://supersecretserver.com/sign", new SecretRequest {
            chainId = chainId,
            RequiredKeys = requiredKeys,
            Data = signBytes
        });
        return result.Signatures;
   }
}

Eos eos = new Eos(new EosConfigurator()
{
    SignProvider = new SuperSecretSignProvider(),
    HttpEndpoint = "https://nodes.eos42.io", //Mainnet
    ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906"
});
```

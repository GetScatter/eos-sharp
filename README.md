# eos-sharp
C# client library for EOS blockchains. The library is based on https://github.com/EOSIO/eosjs and MIT licensed.

### Prerequisite to build

Visual Studio 2017 

### Instalation
eos-sharp is now available through nuget https://www.nuget.org/packages/eos-sharp

```
Install-Package eos-sharp
```

### Usage

#### Configuration

In order to interact with eos blockchain you need to create a new instance of the **Eos** class with a **EosConfigurator**.

Example:

```csharp
Eos eos = new Eos(new EosConfigurator()
{    
    HttpEndpoint = "https://nodes.eos42.io", //Mainnet
    ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906",
    ExpireSeconds = 60,
    SignProvider = new DefaultSignProvider("myprivatekey")
});
```
* HttpEndpoint - http or https location of a nodeosd server providing a chain API.
* ChainId - unique ID for the blockchain you're connecting to. If no ChainId is provided it will get from the get_info API call.
* ExpireInSeconds - number of seconds before the transaction will expire. The time is based on the nodeosd's clock. An unexpired transaction that may have had an error is a liability until the expiration is reached, this time should be brief.
* SignProvider - signature implementation to handle available keys and signing transactions. Use the DefaultSignProvider with a privateKey to sign transactions inside the lib.

#### Api read methods

- **GetInfo** call
```csharp
var result = await eos.GetInfo();
```
Returns:
```csharp
class GetInfoResponse 
{ 
    string    ServerVersion
    string    ChainId 
    UInt32?   HeadBlockNum   
    UInt32?   LastIrreversibleBlockNum
    string    LastIrreversibleBlockId
    string    HeadBlockId   
    DateTime? HeadBlockTime 
    string    HeadBlockProducer
    string    VirtualBlockCpuLimit
    string    VirtualBlockNetLimit  
    string    BlockCpuLimit
    string    BlockNetLimit
}
```

- **GetAccount** call
```csharp
var result = await eos.GetAccount("myaccountname");
```
Returns:
```csharp
class GetAccountResponse
{
    string                 AccountName
    UInt32?                HeadBlockNum 
    DateTime?              HeadBlockTime
    bool?                  Privileged
    DateTime?              LastCodeUpdate 
    DateTime?              Created
    Int32?                 RamQuota 
    Int32?                 NetWeight 
    Int32?                 CpuWeight
    Resource               NetLimit
    Resource               CpuLimit 
    UInt32?                RamUsage 
    List<Permission>       Permissions
    RefundRequest          RefundRequest
    SelfDelegatedBandwidth SelfDelegatedBandwidth 
    TotalResources         TotalResources 
    VoterInfo              VoterInfo
}
```

- **GetBlock** call
```csharp
var result = await eos.GetBlock("blockIdOrNumber");
```
Returns:
```csharp
class GetBlockResponse
{
    DateTime?              Timestamp  
    string                 Producer
    UInt32                 Confirmed  
    string                 Previous  
    string                 TransactionMroot  
    string                 ActionMroot 
    UInt32                 ScheduleVersion 
    string                 NewProducers
    List<Extension>        BlockExtensions  
    List<Extension>        HeaderExtensions
    string                 ProducerSignature 
    List<BlockTransaction> Transactions   
    string                 Id
    UInt32                 BlockNum 
    UInt32                 RefBlockPrefix
}
```

- **GetTableRows** call
    * Json
    * Code - accountName of the contract to search for table rows
    * Scope - scope text segmenting the table set
    * Table - table name 
    * TableKey - unused so far?
    * LowerBound - lower bound for the selected index value
    * UpperBound - upper bound for the selected index value
    * KeyType - Type of the index choosen, ex: i64
    * Limit
    * IndexPosition - 1 - primary (first), 2 - secondary index (in order defined by multi_index), 3 - third index, etc
    * EncodeType - dec, hex
	* Reverse - reverse result order
	* ShowPayer - show ram payer

```csharp
var result = await eos.GetTableRows(new GetTableRowsRequest() {
    Json = true,
    Code = "eosio.token",
    Scope = "EOS",
    Table = "stat"
});
```

Returns:

```csharp
class GetTableRowsResponse
{
    List<object> Rows
    bool?        More
}
```

Using generic type

```csharp
/*JsonProperty helps map the fields from the api*/
class Stat
{
    [JsonProperty("issuer")]
    public string Issuer { get; set; }
    [JsonProperty("max_supply")]
    public string MaxSupply { get; set; }
    [JsonProperty("supply")]
    public string Supply { get; set; }
}

var result = await Eos.GetTableRows<Stat>(new GetTableRowsRequest()
{
    Json = true,
    Code = "eosio.token",
    Scope = "EOS",
    Table = "stat"
});
```

Returns:

```csharp
class GetTableRowsResponse<Stat>
{
    List<Stat> Rows
    bool?      More
}
```

- **GetTableByScope** call
    * Code - accountName of the contract to search for tables
    * Table - table name to filter
    * LowerBound - lower bound of scope, optional
    * UpperBound - upper bound of scope, optional
    * Limit
    * Reverse - reverse result order

```csharp
var result = await eos.GetTableByScope(new GetTableByScopeRequest() {
    Code = "eosio.token",
    Table = "accounts"
});
```

Returns:

```csharp
class GetTableByScopeResponse
{
    List<TableByScopeResultRow> Rows
    string More
}

class TableByScopeResultRow
{
    string Code
    string Scope
    string Table
    string Payer
    UInt32? Count
}
```

- **GetActions** call
    * accountName - accountName to get actions history
    * pos - a absolute sequence positon -1 is the end/last action
    * offset - the number of actions relative to pos, negative numbers return [pos-offset,pos), positive numbers return [pos,pos+offset)

```csharp
var result = await eos.GetActions("myaccountname", 0, 10);
```

Returns:

```csharp
class GetActionsResponse
{
    List<GlobalAction> Actions
    UInt32?            LastIrreversibleBlock
    bool?              TimeLimitExceededError
}
```

#### Create Transaction

```csharp
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

Data can also be a Dictionary with key as string. The dictionary value can be any object with nested Dictionaries

```csharp
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
            Data = new Dictionary<string, string>()
            {
                { "from", "tester112345" },
                { "to", "tester212345" },
                { "quantity", "0.0001 EOS" },
                { "memo", "hello crypto world!" }
            }
        }
    }
});
```

Returns the transactionId
#### Custom SignProvider

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

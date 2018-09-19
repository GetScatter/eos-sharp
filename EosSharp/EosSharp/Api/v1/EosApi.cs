  

  

// Auto Generated, do not edit.
using EosSharp.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EosSharp.Api.v1
{
    public class EosApi
    { 
        public EosConfigurator Config { get; set; }
        public EosApi(EosConfigurator config)
        {
            Config = config;
        }

		public async Task<GetInfoResponse> GetInfo(JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_info", Config.HttpEndpoint);
            return await HttpHelper.GetJsonAsync<GetInfoResponse>(url, jsonSettings);
        }
		public async Task<GetAccountResponse> GetAccount(GetAccountRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_account", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetAccountResponse>(url, data, jsonSettings);
        }
		public async Task<GetCodeResponse> GetCode(GetCodeRequest data, bool reload = false, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_code", Config.HttpEndpoint);
            return await HttpHelper.PostJsonWithCacheAsync<GetCodeResponse>(url, data, reload, jsonSettings);
        }
		public async Task<GetAbiResponse> GetAbi(GetAbiRequest data, bool reload = false, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_abi", Config.HttpEndpoint);
            return await HttpHelper.PostJsonWithCacheAsync<GetAbiResponse>(url, data, reload, jsonSettings);
        }
		public async Task<GetRawCodeAndAbiResponse> GetRawCodeAndAbi(GetRawCodeAndAbiRequest data, bool reload = false, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_raw_code_and_abi", Config.HttpEndpoint);
            return await HttpHelper.PostJsonWithCacheAsync<GetRawCodeAndAbiResponse>(url, data, reload, jsonSettings);
        }
		public async Task<GetRawAbiResponse> GetRawAbi(GetRawAbiRequest data, bool reload = false, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_raw_abi", Config.HttpEndpoint);
            return await HttpHelper.PostJsonWithCacheAsync<GetRawAbiResponse>(url, data, reload, jsonSettings);
        }
		public async Task<AbiJsonToBinResponse> AbiJsonToBin(AbiJsonToBinRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/abi_json_to_bin", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<AbiJsonToBinResponse>(url, data, jsonSettings);
        }
		public async Task<AbiBinToJsonResponse> AbiBinToJson(AbiBinToJsonRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/abi_bin_to_json", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<AbiBinToJsonResponse>(url, data, jsonSettings);
        }
		public async Task<GetRequiredKeysResponse> GetRequiredKeys(GetRequiredKeysRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_required_keys", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetRequiredKeysResponse>(url, data, jsonSettings);
        }
		public async Task<GetBlockResponse> GetBlock(GetBlockRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_block", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetBlockResponse>(url, data, jsonSettings);
        }
		public async Task<GetBlockHeaderStateResponse> GetBlockHeaderState(GetBlockHeaderStateRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_block_header_state", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetBlockHeaderStateResponse>(url, data, jsonSettings);
        }
		public async Task<GetTableRowsResponse<TRowType>> GetTableRows<TRowType>(GetTableRowsRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_table_rows", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetTableRowsResponse<TRowType>>(url, data, jsonSettings);
        }
		public async Task<GetTableRowsResponse> GetTableRows(GetTableRowsRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_table_rows", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetTableRowsResponse>(url, data, jsonSettings);
        }
		public async Task<GetCurrencyBalanceResponse> GetCurrencyBalance(GetCurrencyBalanceRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_currency_balance", Config.HttpEndpoint);
			return new GetCurrencyBalanceResponse() { Assets = await HttpHelper.PostJsonAsync<List<string>>(url, data, jsonSettings) };
        }
		public async Task<GetCurrencyStatsResponse> GetCurrencyStats(GetCurrencyStatsRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_currency_stats", Config.HttpEndpoint);
			return new GetCurrencyStatsResponse() { Stats = await HttpHelper.PostJsonAsync<Dictionary<string, CurrencyStat>>(url, data, jsonSettings) };
        }
		public async Task<GetProducersResponse> GetProducers(GetProducersRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_producers", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetProducersResponse>(url, data, jsonSettings);
        }
		public async Task<GetProducerScheduleResponse> GetProducerSchedule(JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_producer_schedule", Config.HttpEndpoint);
            return await HttpHelper.GetJsonAsync<GetProducerScheduleResponse>(url, jsonSettings);
        }
		public async Task<GetScheduledTransactionsResponse> GetScheduledTransactions(GetScheduledTransactionsRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/get_scheduled_transactions", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetScheduledTransactionsResponse>(url, data, jsonSettings);
        }
		public async Task<PushTransactionResponse> PushTransaction(PushTransactionRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/chain/push_transaction", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<PushTransactionResponse>(url, data, jsonSettings);
        }
		public async Task<GetActionsResponse> GetActions(GetActionsRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/history/get_actions", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetActionsResponse>(url, data, jsonSettings);
        }
		public async Task<GetTransactionResponse> GetTransaction(GetTransactionRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/history/get_transaction", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetTransactionResponse>(url, data, jsonSettings);
        }
		public async Task<GetKeyAccountsResponse> GetKeyAccounts(GetKeyAccountsRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/history/get_key_accounts", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetKeyAccountsResponse>(url, data, jsonSettings);
        }
		public async Task<GetControlledAccountsResponse> GetControlledAccounts(GetControlledAccountsRequest data, JsonSerializerSettings jsonSettings = null)
        {
            var url = string.Format("{0}/v1/history/get_controlled_accounts", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetControlledAccountsResponse>(url, data, jsonSettings);
        }
    }
}

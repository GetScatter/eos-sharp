  
// Auto Generated, do not edit.
using EosSharp.Helpers;
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

		public async Task<GetInfoResponse> GetInfo()
        {
            var url = string.Format("{0}/v1/chain/get_info", Config.HttpEndpoint);
            return await HttpHelper.GetJsonAsync<GetInfoResponse>(url);
        }
		public async Task<GetAccountResponse> GetAccount(GetAccountRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_account", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetAccountResponse>(url, data);
        }
		public async Task<GetCodeResponse> GetCode(GetCodeRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_code", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetCodeResponse>(url, data);
        }
		public async Task<GetAbiResponse> GetAbi(GetAbiRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_abi", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetAbiResponse>(url, data);
        }
		public async Task<GetRawCodeAndAbiResponse> GetRawCodeAndAbi(GetRawCodeAndAbiRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_raw_code_and_abi", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetRawCodeAndAbiResponse>(url, data);
        }
		public async Task<AbiJsonToBinResponse> AbiJsonToBin(AbiJsonToBinRequest data)
        {
            var url = string.Format("{0}/v1/chain/abi_json_to_bin", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<AbiJsonToBinResponse>(url, data);
        }
		public async Task<AbiBinToJsonResponse> AbiBinToJson(AbiBinToJsonRequest data)
        {
            var url = string.Format("{0}/v1/chain/abi_bin_to_json", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<AbiBinToJsonResponse>(url, data);
        }
		public async Task<GetRequiredKeysResponse> GetRequiredKeys(GetRequiredKeysRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_required_keys", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetRequiredKeysResponse>(url, data);
        }
		public async Task<GetBlockResponse> GetBlock(GetBlockRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_block", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetBlockResponse>(url, data);
        }
		public async Task<GetBlockHeaderStateResponse> GetBlockHeaderState(GetBlockHeaderStateRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_block_header_state", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetBlockHeaderStateResponse>(url, data);
        }
		public async Task<GetTableRowsResponse> GetTableRows(GetTableRowsRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_table_rows", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetTableRowsResponse>(url, data);
        }
		public async Task<GetCurrencyBalanceResponse> GetCurrencyBalance(GetCurrencyBalanceRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_currency_balance", Config.HttpEndpoint);
			return new GetCurrencyBalanceResponse() { Assets = await HttpHelper.PostJsonAsync<List<string>>(url, data) };
        }
		public async Task<GetCurrencyStatsResponse> GetCurrencyStats(GetCurrencyStatsRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_currency_stats", Config.HttpEndpoint);
			return new GetCurrencyStatsResponse() { Stats = await HttpHelper.PostJsonAsync<Dictionary<string, CurrencyStat>>(url, data) };
        }
		public async Task<GetProducersResponse> GetProducers(GetProducersRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_producers", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetProducersResponse>(url, data);
        }
		public async Task<GetProducerScheduleResponse> GetProducerSchedule()
        {
            var url = string.Format("{0}/v1/chain/get_producer_schedule", Config.HttpEndpoint);
            return await HttpHelper.GetJsonAsync<GetProducerScheduleResponse>(url);
        }
		public async Task<GetScheduledTransactionsResponse> GetScheduledTransactions(GetScheduledTransactionsRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_scheduled_transactions", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetScheduledTransactionsResponse>(url, data);
        }
		public async Task<PushBlockResponse> PushBlock(PushBlockRequest data)
        {
            var url = string.Format("{0}/v1/chain/push_block", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<PushBlockResponse>(url, data);
        }
		public async Task<PushTransactionResponse> PushTransaction(PushTransactionRequest data)
        {
            var url = string.Format("{0}/v1/chain/push_transaction", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<PushTransactionResponse>(url, data);
        }
		public async Task<PushTransactionsResponse> PushTransactions(PushTransactionsRequest data)
        {
            var url = string.Format("{0}/v1/chain/push_transactions", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<PushTransactionsResponse>(url, data);
        }
		public async Task<GetActionsResponse> GetActions(GetActionsRequest data)
        {
            var url = string.Format("{0}/v1/history/get_actions", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetActionsResponse>(url, data);
        }
		public async Task<GetTransactionResponse> GetTransaction(GetTransactionRequest data)
        {
            var url = string.Format("{0}/v1/history/get_transaction", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetTransactionResponse>(url, data);
        }
		public async Task<GetKeyAccountsResponse> GetKeyAccounts(GetKeyAccountsRequest data)
        {
            var url = string.Format("{0}/v1/history/get_key_accounts", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetKeyAccountsResponse>(url, data);
        }
		public async Task<GetControlledAccountsResponse> GetControlledAccounts(GetControlledAccountsRequest data)
        {
            var url = string.Format("{0}/v1/history/get_controlled_accounts", Config.HttpEndpoint);
            return await HttpHelper.PostJsonAsync<GetControlledAccountsResponse>(url, data);
        }
    }
}

  

  

// Auto Generated, do not edit.
using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EosSharp.Core.Api.v1
{

	/// <summary>
    /// EosApi defines api methods to interface with a http handler
    /// </summary>
    public class EosApi
    { 
        public EosConfigurator Config { get; set; }
        public IHttpHandler HttpHandler { get; set; }

		/// <summary>
        /// Eos Client api constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        /// <param name="httpHandler">Http handler implementation</param>
        public EosApi(EosConfigurator config, IHttpHandler httpHandler)
        {
           Config = config;
           HttpHandler = httpHandler;
        }

		public async Task<GetInfoResponse> GetInfo()
        {
            var url = string.Format("{0}/v1/chain/get_info", Config.HttpEndpoint);
            return await HttpHandler.GetJsonAsync<GetInfoResponse>(url);
        }
		public async Task<GetAccountResponse> GetAccount(GetAccountRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_account", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetAccountResponse>(url, data);
        }
		public async Task<GetCodeResponse> GetCode(GetCodeRequest data, bool reload = false)
        {
            var url = string.Format("{0}/v1/chain/get_code", Config.HttpEndpoint);
            return await HttpHandler.PostJsonWithCacheAsync<GetCodeResponse>(url, data, reload);
        }
		public async Task<GetAbiResponse> GetAbi(GetAbiRequest data, bool reload = false)
        {
            var url = string.Format("{0}/v1/chain/get_abi", Config.HttpEndpoint);
            return await HttpHandler.PostJsonWithCacheAsync<GetAbiResponse>(url, data, reload);
        }
		public async Task<GetRawCodeAndAbiResponse> GetRawCodeAndAbi(GetRawCodeAndAbiRequest data, bool reload = false)
        {
            var url = string.Format("{0}/v1/chain/get_raw_code_and_abi", Config.HttpEndpoint);
            return await HttpHandler.PostJsonWithCacheAsync<GetRawCodeAndAbiResponse>(url, data, reload);
        }
		public async Task<GetRawAbiResponse> GetRawAbi(GetRawAbiRequest data, bool reload = false)
        {
            var url = string.Format("{0}/v1/chain/get_raw_abi", Config.HttpEndpoint);
            return await HttpHandler.PostJsonWithCacheAsync<GetRawAbiResponse>(url, data, reload);
        }
		public async Task<AbiJsonToBinResponse> AbiJsonToBin(AbiJsonToBinRequest data)
        {
            var url = string.Format("{0}/v1/chain/abi_json_to_bin", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<AbiJsonToBinResponse>(url, data);
        }
		public async Task<AbiBinToJsonResponse> AbiBinToJson(AbiBinToJsonRequest data)
        {
            var url = string.Format("{0}/v1/chain/abi_bin_to_json", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<AbiBinToJsonResponse>(url, data);
        }
		public async Task<GetRequiredKeysResponse> GetRequiredKeys(GetRequiredKeysRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_required_keys", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetRequiredKeysResponse>(url, data);
        }
		public async Task<GetBlockResponse> GetBlock(GetBlockRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_block", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetBlockResponse>(url, data);
        }
		public async Task<GetBlockHeaderStateResponse> GetBlockHeaderState(GetBlockHeaderStateRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_block_header_state", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetBlockHeaderStateResponse>(url, data);
        }
		public async Task<GetTableRowsResponse<TRowType>> GetTableRows<TRowType>(GetTableRowsRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_table_rows", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetTableRowsResponse<TRowType>>(url, data);
        }
		public async Task<GetTableRowsResponse> GetTableRows(GetTableRowsRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_table_rows", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetTableRowsResponse>(url, data);
        }
		public async Task<GetTableByScopeResponse> GetTableByScope(GetTableByScopeRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_table_by_scope", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetTableByScopeResponse>(url, data);
        }
		public async Task<GetCurrencyBalanceResponse> GetCurrencyBalance(GetCurrencyBalanceRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_currency_balance", Config.HttpEndpoint);
			return new GetCurrencyBalanceResponse() { assets = await HttpHandler.PostJsonAsync<List<string>>(url, data) };
        }
		public async Task<GetCurrencyStatsResponse> GetCurrencyStats(GetCurrencyStatsRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_currency_stats", Config.HttpEndpoint);
			return new GetCurrencyStatsResponse() { stats = await HttpHandler.PostJsonAsync<Dictionary<string, CurrencyStat>>(url, data) };
        }
		public async Task<GetProducersResponse> GetProducers(GetProducersRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_producers", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetProducersResponse>(url, data);
        }
		public async Task<GetProducerScheduleResponse> GetProducerSchedule()
        {
            var url = string.Format("{0}/v1/chain/get_producer_schedule", Config.HttpEndpoint);
            return await HttpHandler.GetJsonAsync<GetProducerScheduleResponse>(url);
        }
		public async Task<GetScheduledTransactionsResponse> GetScheduledTransactions(GetScheduledTransactionsRequest data)
        {
            var url = string.Format("{0}/v1/chain/get_scheduled_transactions", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetScheduledTransactionsResponse>(url, data);
        }
		public async Task<PushTransactionResponse> PushTransaction(PushTransactionRequest data)
        {
            var url = string.Format("{0}/v1/chain/push_transaction", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<PushTransactionResponse>(url, data);
        }
		public async Task<GetActionsResponse> GetActions(GetActionsRequest data)
        {
            var url = string.Format("{0}/v1/history/get_actions", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetActionsResponse>(url, data);
        }
		public async Task<GetTransactionResponse> GetTransaction(GetTransactionRequest data)
        {
            var url = string.Format("{0}/v1/history/get_transaction", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetTransactionResponse>(url, data);
        }
		public async Task<GetKeyAccountsResponse> GetKeyAccounts(GetKeyAccountsRequest data)
        {
            var url = string.Format("{0}/v1/history/get_key_accounts", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetKeyAccountsResponse>(url, data);
        }
		public async Task<GetControlledAccountsResponse> GetControlledAccounts(GetControlledAccountsRequest data)
        {
            var url = string.Format("{0}/v1/history/get_controlled_accounts", Config.HttpEndpoint);
            return await HttpHandler.PostJsonAsync<GetControlledAccountsResponse>(url, data);
        }
    }
}

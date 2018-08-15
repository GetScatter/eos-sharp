using EosSharp.Api.v1.Types;
using EosSharp.Helpers;
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
    }
}



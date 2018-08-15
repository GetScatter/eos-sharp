using EosSharp.Api.v1.Types;
using EosSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<GetInfoResult> GetInfo()
        {
            var url = string.Format("{0}/v1/chain/get_info", Config.HttpEndpoint);
            return await HttpHelper.GetJsonAsync<GetInfoResult>(url);
        }
    }
}

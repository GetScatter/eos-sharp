using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Api.v1.Trace;
using EosSharp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EosSharp.Core.Api.v1
{
	/// <summary>
    /// TraceApi defines api methods for Trace Api Plug-In
    /// </summary>
    public class TraceApi
    { 
        public EosConfigurator Config { get; set; }
        public IHttpHandler HttpHandler { get; set; }

		/// <summary>
        /// Eos Client Trace Api constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        /// <param name="httpHandler">Http handler implementation</param>
        public TraceApi(EosConfigurator config, IHttpHandler httpHandler)
        {
           Config = config;
           HttpHandler = httpHandler;
        }

        private string MethodUrl(string methodName) => $"{Config.HttpEndpoint}/v1/trace_api/{methodName}";

		public async Task<GetBlockTraceResponse> GetBlockTrace(GetBlockTraceRequest data)
        {
            return await HttpHandler.PostJsonAsync<GetBlockTraceResponse>(MethodUrl("get_block"), data);
        }
    }
}

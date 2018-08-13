using EosSharp;

namespace EosSharp
{
    public class Eos
    {
  //      const network = config.httpEndpoint != null ? EosApi(config) : null
  //config.network = network

  //const abiCache = AbiCache(network, config)

  //if(!config.chainId) {
  //  config.chainId = 'cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f'
  //}

  //if(network) {
  //  checkChainId(network, config.chainId, config.logger)
  //}

        public Eos(EosConfigurator config = null)
        {
            if (config == null)
                config = new EosConfigurator();

            if (string.IsNullOrWhiteSpace(config.ChainId))
            {
                config.ChainId = "cf057bbfb72640471fd910bcb67639c22df9f92470936cddc1ade0e2f2e7dc4f";
            }
        }
    }
}

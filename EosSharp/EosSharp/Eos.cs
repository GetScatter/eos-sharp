using EosSharp;
using EosSharp.Api.v1;
using EosSharp.Providers;

namespace EosSharp
{
    public class Eos
    {
        private EosConfigurator EosConfig { get; set; }
        private EosApi Api { get; set; }
        private AbiSerializationProvider AbiSerializer { get; set; }

        public Eos(EosConfigurator config = null)
        {
            EosConfig = config ?? new EosConfigurator();
            Api = new EosApi(EosConfig);
            AbiSerializer = new AbiSerializationProvider(Api);
        }
    }
}

using EosSharp.Core;

namespace EosSharp.Unity3D
{
    /// <summary>
    /// EOSIO client wrapper using general purpose HttpHandler
    /// </summary>
    public class Eos : EosBase
    {
        /// <summary>
        /// EOSIO Client wrapper constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        public Eos(EosConfigurator configuratior) : 
            base(configuratior, new HttpHandler())
        {
        }
    }
}

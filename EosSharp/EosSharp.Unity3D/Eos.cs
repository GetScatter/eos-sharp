using EosSharp.Core;

namespace EosSharp.Unity3D
{
    public class Eos : EosBase
    {
        public Eos(EosConfigurator configuratior) : 
            base(configuratior, new HttpHandler())
        {
        }
    }
}

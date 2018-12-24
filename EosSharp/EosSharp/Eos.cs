using EosSharp.Core;

namespace EosSharp
{
    public class Eos : EosBase
    {
        public Eos(EosConfigurator configuratior) : 
            base(configuratior, new HttpHelper())
        {
        }
    }
}

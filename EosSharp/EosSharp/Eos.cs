using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Helpers;
using EosSharp.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

using System;
using System.Collections.Generic;
using System.Text;

namespace EosSharp
{
    public class EosConfigurator
    {
        public string HttpEndpoint { get; set; }
        public string ChainId { get; set; }
        public bool Debug { get; set; }
        public bool Verbose { get; set; }
        public bool Broadcast { get; set; }
        public bool Sign { get; set; }

        public EosConfigurator()
        {
            HttpEndpoint = "http://127.0.0.1:8888";
            Debug = false;
            Verbose = false;
            Broadcast = true;
            Sign = true;
        }
    }
}

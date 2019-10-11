using System;
using System.Collections.Generic;
using System.Text;

namespace EosSharp.Core
{
    public class SignedTransaction
    {
        public IEnumerable<string> Signatures { get; set; }
        public byte[] PackedTransaction { get; set; }
    }
}

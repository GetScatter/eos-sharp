using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EosSharp
{
    public interface ISignProvider
    {
        Task<byte[]> SignTransaction(byte[] trx);
    }
}

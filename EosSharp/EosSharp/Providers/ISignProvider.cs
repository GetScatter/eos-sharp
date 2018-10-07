using EosSharp.Api.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EosSharp
{
    public interface ISignProvider
    {
        Task<IEnumerable<string>> GetAvailableKeys();
        Task<IEnumerable<string>> Sign(string chainId, IEnumerable<string> requiredKeys, byte[] signBytes, IEnumerable<string> abiNames = null);
    }
}

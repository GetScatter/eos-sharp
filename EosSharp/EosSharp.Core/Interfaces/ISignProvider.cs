using System.Collections.Generic;
using System.Threading.Tasks;

namespace EosSharp.Core.Interfaces
{
    /// <summary>
    /// Signature provider Interface to delegate multiple signing implementations
    /// </summary>
    public interface ISignProvider
    {
        /// <summary>
        /// Get available public keys from signature provider
        /// </summary>
        /// <returns>List of public keys</returns>
        Task<IEnumerable<string>> GetAvailableKeys();

        /// <summary>
        /// Sign bytes using the signature provider
        /// </summary>
        /// <param name="chainId">EOSIO Chain id</param>
        /// <param name="requiredKeys">required public keys for signing this bytes</param>
        /// <param name="signBytes">signature bytes</param>
        /// <param name="abiNames">abi contract names to get abi information from</param>
        /// <returns>List of signatures per required keys</returns>
        Task<IEnumerable<string>> Sign(string chainId, IEnumerable<string> requiredKeys, byte[] signBytes, IEnumerable<string> abiNames = null);
    }
}

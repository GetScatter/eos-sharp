using EosSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace EosSharp.Core.Providers
{
    /// <summary>
    /// Signature provider that combine multiple signature providers to complete all the signatures for a transaction
    /// </summary>
    public class CombinedSignersProvider : ISignProvider
    {
        private List<ISignProvider> Signers { get; set; }
        
        /// <summary>
        /// Creates the provider with a list of signature providers
        /// </summary>
        /// <param name="signers"></param>
        public CombinedSignersProvider(List<ISignProvider> signers)
        {
            if (signers == null || signers.Count == 0)
                throw new ArgumentNullException("Required atleast one signer.");

            Signers = signers;
        }

        /// <summary>
        /// Get available public keys from the list of signature providers
        /// </summary>
        /// <returns>List of public keys</returns>
        public async Task<IEnumerable<string>> GetAvailableKeys()
        {
            var availableKeysListTasks = Signers.Select(s => s.GetAvailableKeys());
            var availableKeysList = await Task.WhenAll(availableKeysListTasks);
            return availableKeysList.SelectMany(k => k).Distinct();
        }

        /// <summary>
        /// Sign bytes using the list of signature providers
        /// </summary>
        /// <param name="chainId">EOSIO Chain id</param>
        /// <param name="requiredKeys">required public keys for signing this bytes</param>
        /// <param name="signBytes">signature bytes</param>
        /// <param name="abiNames">abi contract names to get abi information from</param>
        /// <returns>List of signatures per required keys</returns>
        public async Task<IEnumerable<string>> Sign(string chainId, IEnumerable<string> requiredKeys, byte[] signBytes, IEnumerable<string> abiNames = null)
        {
            var signatureTasks = Signers.Select(s => s.Sign(chainId, requiredKeys, signBytes, abiNames));
            var signatures = await Task.WhenAll(signatureTasks);
            return signatures.SelectMany(k => k).Distinct();
        }
    }
}

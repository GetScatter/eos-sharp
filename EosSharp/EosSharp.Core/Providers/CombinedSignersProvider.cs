using EosSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace EosSharp.Core.Providers
{
    public class CombinedSignersProvider : ISignProvider
    {
        private List<ISignProvider> Signers { get; set; }
        
        public CombinedSignersProvider(List<ISignProvider> signers)
        {
            if (signers == null || signers.Count == 0)
                throw new ArgumentNullException("Required atleast one signer.");

            Signers = signers;
        }

        public async Task<IEnumerable<string>> GetAvailableKeys()
        {
            var availableKeysListTasks = Signers.Select(s => s.GetAvailableKeys());
            var availableKeysList = await Task.WhenAll(availableKeysListTasks);
            return availableKeysList.SelectMany(k => k).Distinct();
        }

        public async Task<IEnumerable<string>> Sign(string chainId, IEnumerable<string> requiredKeys, byte[] signBytes, IEnumerable<string> abiNames = null)
        {
            var signatureTasks = Signers.Select(s => s.Sign(chainId, requiredKeys, signBytes, abiNames));
            var signatures = await Task.WhenAll(signatureTasks);
            return signatures.SelectMany(k => k).Distinct();
        }
    }
}

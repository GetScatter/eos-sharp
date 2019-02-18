using EosSharp.Core.Interfaces;

namespace EosSharp.Core
{
    /// <summary>
    /// Aggregates all properties to configure Eos client
    /// </summary>
    public class EosConfigurator
    {
        /// <summary>
        /// http or https location of a nodeosd server providing a chain API.
        /// </summary>
        public string HttpEndpoint { get; set; } = "http://127.0.0.1:8888";
        /// <summary>
        /// unique ID for the blockchain you're connecting to. If no ChainId is provided it will get from the get_info API call.
        /// </summary>
        public string ChainId { get; set; }
        /// <summary>
        /// number of seconds before the transaction will expire. The time is based on the nodeosd's clock. 
        /// An unexpired transaction that may have had an error is a liability until the expiration is reached, this time should be brief.
        /// </summary>
        public double ExpireSeconds { get; set; } = 60;
        /// <summary>
        /// signature implementation to handle available keys and signing transactions. Use the DefaultSignProvider with a privateKey to sign transactions inside the lib.
        /// </summary>
        public ISignProvider SignProvider { get; set; }
    }
}

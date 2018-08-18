using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EosSharp.Helpers
{
    public class ChainHelper
    {
        public static bool CheckChainId(/*network, chainId, logger*/)
        {
            //        network.getInfo({ }).then(info => {
            //        if (info.chain_id !== chainId)
            //        {
            //            if (logger.log)
            //            {
            //                logger.log(
            //                  'chainId mismatch, signatures will not match transaction authority. ' +
            //                  `expected ${ chainId} !== actual ${ info.chain_id}`

            //            )
            //  }
            //}
            //}).catch(error => {
            //    if(logger.error) {
            //      logger.error('Warning, unable to validate chainId: ' + error.message)
            //    }
            //  })
            return true;
        }

        //TODO optimize
        public static string TransactionToHexString(object trx)
        {
            return ByteArrayToHexString(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(trx)));
        }
        
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}

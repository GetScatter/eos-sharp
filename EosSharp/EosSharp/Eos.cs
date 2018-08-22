using EosSharp;
using EosSharp.Api.v1;
using EosSharp.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using EosSharp.Helpers;

namespace EosSharp
{
    public class Eos
    {
        private EosConfigurator EosConfig { get; set; }
        private EosApi Api { get; set; }
        private AbiSerializationProvider AbiSerializer { get; set; }

        public Eos(EosConfigurator config = null)
        {
            EosConfig = config ?? new EosConfigurator();
            Api = new EosApi(EosConfig);
            AbiSerializer = new AbiSerializationProvider(Api);
        }

        public async Task<string> CreateTransaction(Transaction trx)
        {
            if(trx.Expiration == DateTime.MinValue ||
               trx.RefBlockNum == 0 ||
               trx.RefBlockPrefix == 0)
            {
                var getInfoResult = await Api.GetInfo();
                var getBlockResult = await Api.GetBlock(new GetBlockRequest()
                {
                    BlockNumOrId = getInfoResult.LastIrreversibleBlockNum.GetValueOrDefault().ToString()
                });

                trx.Expiration = getInfoResult.HeadBlockTime.Value.AddSeconds(EosConfig.ExpireSeconds);
                trx.RefBlockNum = (UInt16)(getInfoResult.LastIrreversibleBlockNum.Value & 0xFFFF);
                trx.RefBlockPrefix = getBlockResult.RefBlockPrefix;
            }

            var packedTrx = await AbiSerializer.SerializePackedTransaction(trx);
            var signProvider = new DefaultSignProvider();

            //TODO get required keys from trx
            var requiredKeys = new List<string>() { "EOS8Q8CJqwnSsV4A6HDBEqmQCqpQcBnhGME1RUvydDRnswNngpqfr" };

            var signatures = await signProvider.Sign(Api.Config.ChainId, requiredKeys, packedTrx);

            var result = await Api.PushTransaction(new PushTransactionRequest()
            {
                Signatures = signatures.ToArray(),
                Compression = 0,
                PackedContextFreeData = "",
                PackedTrx = SerializationHelper.ByteArrayToHexString(packedTrx)
            });

            return result.TransactionId;
        }
    }
}

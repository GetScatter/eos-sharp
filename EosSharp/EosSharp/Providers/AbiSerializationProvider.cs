using EosSharp.Api.v1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EosSharp.Providers
{
    public class AbiSerializationProvider
    {
        public byte[] SerializePackedTransaction(Transaction trx)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WriteUint32(ms, DateToTimePointSec(trx.Expiration));
                WriteUint16(ms, trx.RefBlockNum);
                WriteUint32(ms, trx.RefBlockPrefix);
                WriteUint32(ms, trx.MaxNetUsageWords);
                WriteUint8(ms, trx.MaxCpuUsageMs);
                WriteUint32(ms, trx.DelaySec);

                WriteUint32(ms, (UInt32)trx.ContextFreeActions.Count);
                WriteUint32(ms, (UInt32)trx.Actions.Count);
                WriteUint32(ms, (UInt32)trx.TransactionExtensions.Count);

                return ms.ToArray();
            }
        }

        private static UInt64 DateToTimePoint(DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt64)span.TotalMilliseconds;
        }

        private static UInt32 DateToTimePointSec(DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1));
            return (UInt32)span.TotalSeconds;
        }

        private static void WriteUint8(MemoryStream ms, byte value)
        {
            ms.Write(new byte[] { value }, 0, 1);
        }

        private static void WriteUint16(MemoryStream ms, UInt16 value)
        {
            ms.Write(BitConverter.GetBytes(value), 0, 2);
        }

        private static void WriteUint32(MemoryStream ms, UInt32 value)
        {
            ms.Write(BitConverter.GetBytes(value), 0, 4);
        }

        private static void WriteUint64(MemoryStream ms, UInt64 value)
        {
            ms.Write(BitConverter.GetBytes(value), 0, 8);
        }
    }
}

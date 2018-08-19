using EosSharp.Api.v1;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using EosSharp.Helpers;

namespace EosSharp.Providers
{
    public class AbiSerializationProvider
    {
        private EosApi Api { get; set; }
        private Dictionary<string, Action<MemoryStream, object>> TypeWriters { get; set; }

        public AbiSerializationProvider(EosApi api)
        {
            this.Api = api;

            TypeWriters = new Dictionary<string, Action<MemoryStream, object>>()
            {
                { "string", WriteString },
                { "uint8",  WriteUint8 },
                { "uint16", WriteUint16 },
                { "uint32", WriteUint32 },
                { "uint64", WriteUint8 },
                { "account_name", WriteName },
                { "asset", WriteAsset }
            };
        }

        public async Task<byte[]> SerializePackedTransaction(Transaction trx)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //trx headers
                WriteUint32(ms, SerializationHelper.DateToTimePointSec(trx.Expiration));
                WriteUint16(ms, trx.RefBlockNum);
                WriteUint32(ms, trx.RefBlockPrefix);

                //trx info
                WriteVarUint32(ms, trx.MaxNetUsageWords);
                WriteUint8(ms, trx.MaxCpuUsageMs);
                WriteVarUint32(ms, trx.DelaySec);

                WriteVarUint32(ms, (UInt32)trx.ContextFreeActions.Count);
                foreach (var action in trx.ContextFreeActions)
                {
                    await WriteAction(ms, action);
                }

                WriteVarUint32(ms, (UInt32)trx.Actions.Count);
                foreach (var action in trx.Actions)
                {
                    await WriteAction(ms, action);
                }

                WriteVarUint32(ms, (UInt32)trx.TransactionExtensions.Count);
                foreach (var extension in trx.TransactionExtensions)
                {
                    WriteExtension(ms, extension);
                }

                return ms.ToArray();
            }
        }

        private void WriteExtension(MemoryStream ms, Api.v1.Extension extension)
        {
            WriteUint16(ms, extension.Type);
            //TODO abi data?
            //public object Data { get; set; }
        }

        private async Task WriteAction(MemoryStream ms, Api.v1.Action action)
        {
            WriteName(ms, action.Account);
            WriteName(ms, action.Name);

            WriteVarUint32(ms, (UInt32)action.Authorization.Count);
            foreach (var perm in action.Authorization)
            {
                WritePermissionLevel(ms, perm);
            }

            await WriteAbiData(ms, action);
        }

        private async Task WriteAbiData(MemoryStream ms, Api.v1.Action action)
        {
            var abiResult = await Api.GetAbi(new GetAbiRequest()
            {
                AccountName = action.Account
            });

            var abiAction = abiResult.Abi.Actions.First(aa => aa.Name == action.Name);
            var abiStruct = abiResult.Abi.Structs.First(s => s.Name == abiAction.Type);

            Type dataType = action.Data.GetType();
            foreach (var field in abiStruct.Fields)
            {
                var value = dataType.GetProperty(field.Name).GetValue(action.Data, null);
                WriteAbiType(ms, value, field.Type, abiResult.Abi);
            }
        }

        private void WriteAbiType(MemoryStream ms, object value, string type, Abi abi)
        {
            TypeWriters[type](ms, value);
        }

        private static void WritePermissionLevel(MemoryStream ms, PermissionLevel perm)
        {
            WriteName(ms, perm.Actor);
            WriteName(ms, perm.Permission);
        }

        private static void WriteName(MemoryStream ms, object value)
        {
            var a = new byte[8];
            Int32 bit = 63;
            string s = (string)value;
            for (int i = 0; i < s.Length; ++i)
            {
                var c = SerializationHelper.CharToSymbol(s[i]);
                if (bit < 5)
                    c = (byte)(c << 1);
                for (int j = 4; j >= 0; --j)
                {
                    if (bit >= 0)
                    {
                        a[(int)Math.Floor((decimal)(bit / 8))] |= (byte)(((c >> j) & 1) << (bit % 8));
                        --bit;
                    }
                }
            }
            ms.Write(a, 0, 8);
        }

        private static void WriteAsset(MemoryStream ms, object value)
        {
            var s = ((string)value).Trim();
            Int32 pos = 0;
            string amount = "";
            UInt16 precision = 0;

            if (s[pos] == '-')
            {
                amount += '-';
                ++pos;
            }

            bool foundDigit = false;
            while (pos < s.Length && s[pos] >= '0' && s[pos] <= '9')
            {
                foundDigit = true;
                amount += s[pos];
                ++pos;
            }

            if (!foundDigit)
                throw new Exception("Asset must begin with a number");

            if (s[pos] == '.')
            {
                ++pos;
                while (pos < s.Length && s[pos] >= '0' && s[pos] <= '9')
                {
                    amount += s[pos];
                    ++precision;
                    ++pos;
                }
            }

            string name = s.Substring(pos).Trim();

            var decimalBytes = SerializationHelper.SignedDecimalToBinary(8, amount);
            ms.Write(decimalBytes, 0, decimalBytes.Length);
            WriteSymbol(ms, new Symbol() { Name = name, Precision = precision });
        }

        private static void WriteSymbol(MemoryStream ms, object value)
        {
            var symbol = (Symbol)value;

            WriteUint16(ms, symbol.Precision);

            if (symbol.Name.Length > 4)
                ms.Write(Encoding.UTF8.GetBytes(symbol.Name.Substring(0, 4)), 0, 4);
            else
            {

                ms.Write(Encoding.UTF8.GetBytes(symbol.Name), 0, symbol.Name.Length);

                if (symbol.Name.Length < 4)
                {
                    var fill = new byte[4 - symbol.Name.Length];
                    for (int i = 0; i < fill.Length; i++)
                        fill[i] = 0;
                    ms.Write(fill, 0, fill.Length);
                }
            }
        }

        private static void WriteString(MemoryStream ms, object value)
        {
            string s = (string)value;
            WriteUint32(ms, (UInt32)s.Length);
            ms.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
        }

        public static void WriteUint8(MemoryStream ms, object value)
        {
            ms.Write(new byte[] { (byte)value }, 0, 1);
        }

        private static void WriteUint16(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((UInt16)value), 0, 2);
        }

        private static void WriteUint32(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((UInt32)value), 0, 4);
        }

        private static void WriteVarUint32(MemoryStream ms, object value)
        {
            var v = (UInt32)value;
            while (true)
            {
                if ((v >> 7) != 0)
                {
                    ms.Write(new byte[] { (byte)(0x80 | (v & 0x7f)) }, 0, 1);
                    v >>= 7;
                }
                else
                {
                    ms.Write(new byte[] { (byte)(v) }, 0, 1);
                    break;
                }
            }
        }

        private static void WriteUint64(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((UInt64)value), 0, 8);
        }
    }
}

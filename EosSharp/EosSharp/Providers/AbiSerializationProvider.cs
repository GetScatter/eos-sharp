using EosSharp.Api.v1;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using EosSharp.Helpers;
using FastMember;

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
                {"int8",                 WriteInt8               },
                {"uint8",                WriteUint8              },
                {"int16",                WriteInt16              },
                {"uint16",               WriteUint16             },
                {"int32",                WriteInt32              },
                {"uint32",               WriteUint32             },
                {"int64",                WriteInt64              },
                {"uint64",               WriteUint64             },
                {"int128",               WriteInt128             },
                {"uint128",              WriteUInt128            },
                {"varuint32",            WriteVarUint32          },
                {"varint32",             WriteVarInt32           },
                {"float32",              WriteFloat32            },
                {"float64",              WriteFloat64            },
                {"float128",             WriteFloat128           },
                {"bytes",                WriteBytes              },
                {"bool",                 WriteBool               },
                {"string",               WriteString             },
                {"name",                 WriteName               },
                {"asset",                WriteAsset              },
                {"time_point",           WriteTimePoint          },
                {"time_point_sec",       WriteTimePointSec       },
                {"block_timestamp_type", WriteBlockTimestampType },
                {"symbol_code",          WriteSymbolCode         },
                {"symbol",               WriteSymbol             },
                {"checksum160",          WriteChecksum160        },
                {"checksum256",          WriteChecksum256        },
                {"checksum512",          WriteChecksum512        },
                {"public_key",           WritePublicKey          },
                {"private_key",          WritePrivateKey         },
                {"signature",            WriteSignature          },
                {"extended_asset",       WriteExtendedAsset      }
            };
        }      

        public async Task<byte[]> SerializePackedTransaction(Transaction trx)
        {
            int actionIndex = 0;
            var abiResponses = await GetTransactionAbis(trx);

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
                    WriteAction(ms, action, abiResponses[actionIndex++].Abi);
                }

                WriteVarUint32(ms, (UInt32)trx.Actions.Count);
                foreach (var action in trx.Actions)
                {
                    WriteAction(ms, action, abiResponses[actionIndex++].Abi);
                }

                WriteVarUint32(ms, (UInt32)trx.TransactionExtensions.Count);
                foreach (var extension in trx.TransactionExtensions)
                {
                    WriteExtension(ms, extension);
                }

                return ms.ToArray();
            }
        }

        public byte[] SerializeActionData(Api.v1.Action action, Abi abi)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var abiAction = abi.Actions.First(aa => aa.Name == action.Name);
                var abiStruct = abi.Structs.First(s => s.Name == abiAction.Type);

                var accessor = ObjectAccessor.Create(action.Data);
                foreach (var field in abiStruct.Fields)
                {
                    WriteAbiType(ms, accessor[field.Name], field.Type, abi);
                }
                
                return ms.ToArray();
            }
        }

        public Task<GetAbiResponse[]> GetTransactionAbis(Transaction trx)
        {
            var abiTasks = new List<Task<GetAbiResponse>>();

            foreach (var action in trx.ContextFreeActions)
            {
                abiTasks.Add(Api.GetAbi(new GetAbiRequest()
                {
                    AccountName = action.Account
                }));
            }

            foreach (var action in trx.Actions)
            {
                abiTasks.Add(Api.GetAbi(new GetAbiRequest()
                {
                    AccountName = action.Account
                }));
            }

            return Task.WhenAll(abiTasks);
        }

        #region Writer Functions

        private static void WriteInt8(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteUint8(MemoryStream ms, object value)
        {
            ms.Write(new byte[] { (byte)value }, 0, 1);
        }

        private static void WriteInt16(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteUint16(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((UInt16)value), 0, 2);
        }

        private static void WriteInt32(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteUint32(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((UInt32)value), 0, 4);
        }

        private static void WriteInt64(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteUint64(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((UInt64)value), 0, 8);
        }

        private static void WriteUInt128(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteInt128(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
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

        private static void WriteVarInt32(MemoryStream ms, object value)
        {
            var n = (Int32)value;
            WriteVarUint32(ms, (UInt32)((n << 1) ^ (n >> 31)));
        }

        private static void WriteFloat32(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((float)value), 0, 4);
        }

        private static void WriteFloat64(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((double)value), 0, 8);
        }

        private static void WriteFloat128(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteBytes(MemoryStream ms, object value)
        {
            var bytes = (byte[])value;

            WriteVarUint32(ms, (UInt32)bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
        }

        private static void WriteBool(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteString(MemoryStream ms, object value)
        {
            string s = (string)value;
            WriteVarUint32(ms, (UInt32)s.Length);
            if (s.Length > 0)
                ms.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
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
            byte precision = 0;

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

        private static void WriteTimePoint(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteTimePointSec(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteBlockTimestampType(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteSymbol(MemoryStream ms, object value)
        {
            var symbol = (Symbol)value;

            WriteUint8(ms, symbol.Precision);

            if (symbol.Name.Length > 7)
                ms.Write(Encoding.UTF8.GetBytes(symbol.Name.Substring(0, 7)), 0, 7);
            else
            {
                ms.Write(Encoding.UTF8.GetBytes(symbol.Name), 0, symbol.Name.Length);

                if (symbol.Name.Length < 7)
                {
                    var fill = new byte[7 - symbol.Name.Length];
                    for (int i = 0; i < fill.Length; i++)
                        fill[i] = 0;
                    ms.Write(fill, 0, fill.Length);
                }
            }
        }

        private static void WriteSymbolCode(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteChecksum160(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteChecksum256(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteChecksum512(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }
        
        private static void WritePublicKey(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WritePrivateKey(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteSignature(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private static void WriteExtendedAsset(MemoryStream ms, object value)
        {
            throw new NotImplementedException();
        }

        private void WriteExtension(MemoryStream ms, Api.v1.Extension extension)
        {
            WriteUint16(ms, extension.Type);
            //TODO abi data?
            //public object Data { get; set; }
        }

        private void WriteAction(MemoryStream ms, Api.v1.Action action, Abi abi)
        {
            WriteName(ms, action.Account);
            WriteName(ms, action.Name);

            WriteVarUint32(ms, (UInt32)action.Authorization.Count);
            foreach (var perm in action.Authorization)
            {
                WritePermissionLevel(ms, perm);
            }

            WriteBytes(ms, SerializeActionData(action, abi));
        }

        private void WriteAbiType(MemoryStream ms, object value, string type, Abi abi)
        {


            TypeWriters[type](ms, value);
        }

        //    function serializeStruct(buffer: SerialBuffer, data: any)
        //    {
        //        if (this.base)
        //this.base.serialize(buffer, data);
        //        for (let field of this.fields)
        //        {
        //            if (!(field.name in data))
        //  throw new Error('missing ' + this.name + '.' + field.name + ' (type=' + field.type.name + ')');
        //        field.type.serialize(buffer, data[field.name]);
        //    }
        //}

        //function serializeArray(buffer: SerialBuffer, data: any[])
        //{
        //    buffer.pushVaruint32(data.length);
        //    for (let item of data)
        //        this.arrayOf.serialize(buffer, item);
        //}

        //function serializeOptional(buffer: SerialBuffer, data: any)
        //{
        //    if (data === null || data === undefined)
        //    {
        //        buffer.push(0);
        //    }
        //    else
        //    {
        //        buffer.push(1);
        //        this.optionalOf.serialize(buffer, data);
        //    }
        //}

        private static void WritePermissionLevel(MemoryStream ms, PermissionLevel perm)
        {
            WriteName(ms, perm.Actor);
            WriteName(ms, perm.Permission);
        }

        #endregion
    }
}

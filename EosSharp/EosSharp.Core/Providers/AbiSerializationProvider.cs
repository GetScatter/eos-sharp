using EosSharp.Core.Api.v1;
using EosSharp.Core.DataAttributes;
using EosSharp.Core.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EosSharp.Core.Providers
{
    public class AbiSerializationProvider
    {
        private enum KeyType
        {
            k1 = 0,
            r1 = 1,
        };
        private delegate object ReaderDelegate(byte[] data, ref int dataIndex);

        private EosApi Api { get; set; }
        private Dictionary<string, Action<MemoryStream, object>> TypeWriters { get; set; }
        private Dictionary<string, ReaderDelegate> TypeReaders { get; set; }

        public AbiSerializationProvider(EosApi api)
        {
            this.Api = api;

            TypeWriters = new Dictionary<string, Action<MemoryStream, object>>()
            {     
                {"int8",                 WriteByte               },
                {"uint8",                WriteByte               },
                {"int16",                WriteUint16             },
                {"uint16",               WriteUint16             },
                {"int32",                WriteUint32             },
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
                {"symbol",               WriteSymbolString       },
                {"checksum160",          WriteChecksum160        },
                {"checksum256",          WriteChecksum256        },
                {"checksum512",          WriteChecksum512        },
                {"public_key",           WritePublicKey          },
                {"private_key",          WritePrivateKey         },
                {"signature",            WriteSignature          },
                {"extended_asset",       WriteExtendedAsset      }
            };

            TypeReaders = new Dictionary<string, ReaderDelegate>()
            {
                {"int8",                 ReadByte               },
                {"uint8",                ReadByte               },
                {"int16",                ReadUint16             },
                {"uint16",               ReadUint16             },
                {"int32",                ReadUint32             },
                {"uint32",               ReadUint32             },
                {"int64",                ReadInt64              },
                {"uint64",               ReadUint64             },
                {"int128",               ReadInt128             },
                {"uint128",              ReadUInt128            },
                {"varuint32",            ReadVarUint32          },
                {"varint32",             ReadVarInt32           },
                {"float32",              ReadFloat32            },
                {"float64",              ReadFloat64            },
                {"float128",             ReadFloat128           },
                {"bytes",                ReadBytes              },
                {"bool",                 ReadBool               },
                {"string",               ReadString             },
                {"name",                 ReadName               },
                {"asset",                ReadAsset              },
                {"time_point",           ReadTimePoint          },
                {"time_point_sec",       ReadTimePointSec       },
                {"block_timestamp_type", ReadBlockTimestampType },
                {"symbol_code",          ReadSymbolCode         },
                {"symbol",               ReadSymbolString       },
                {"checksum160",          ReadChecksum160        },
                {"checksum256",          ReadChecksum256        },
                {"checksum512",          ReadChecksum512        },
                {"public_key",           ReadPublicKey          },
                {"private_key",          ReadPrivateKey         },
                {"signature",            ReadSignature          },
                {"extended_asset",       ReadExtendedAsset      }
            };
        }      

        public async Task<byte[]> SerializePackedTransaction(Transaction trx)
        {
            int actionIndex = 0;
            var abiResponses = await GetTransactionAbis(trx);

            using (MemoryStream ms = new MemoryStream())
            {
                //trx headers
                WriteUint32(ms, SerializationHelper.DateToTimePointSec(trx.expiration));
                WriteUint16(ms, trx.ref_block_num);
                WriteUint32(ms, trx.ref_block_prefix);

                //trx info
                WriteVarUint32(ms, trx.max_net_usage_words);
                WriteByte(ms, trx.max_cpu_usage_ms);
                WriteVarUint32(ms, trx.delay_sec);

                WriteVarUint32(ms, (UInt32)trx.context_free_actions.Count);
                foreach (var action in trx.context_free_actions)
                {
                    WriteAction(ms, action, abiResponses[actionIndex++]);
                }

                WriteVarUint32(ms, (UInt32)trx.actions.Count);
                foreach (var action in trx.actions)
                {
                    WriteAction(ms, action, abiResponses[actionIndex++]);
                }

                WriteVarUint32(ms, (UInt32)trx.transaction_extensions.Count);
                foreach (var extension in trx.transaction_extensions)
                {
                    WriteExtension(ms, extension);
                }

                return ms.ToArray();
            }
        }

        public async Task<Transaction> DeserializePackedTransaction(string packtrx)
        {
            var data = SerializationHelper.HexStringToByteArray(packtrx);
            int readIndex = 0;
            var trx = new Transaction()
            {
                expiration = (DateTime)ReadTimePointSec(data, ref readIndex),
                ref_block_num = (UInt16)ReadUint16(data, ref readIndex),
                ref_block_prefix = (UInt32)ReadUint32(data, ref readIndex),
                max_net_usage_words = (UInt32)ReadVarUint32(data, ref readIndex),
                max_cpu_usage_ms = (byte)ReadByte(data, ref readIndex),
                delay_sec = (UInt32)ReadVarUint32(data, ref readIndex),
            };

            var contextFreeActionsSize = Convert.ToInt32(ReadVarUint32(data, ref readIndex));
            trx.context_free_actions = new List<Core.Api.v1.Action>(contextFreeActionsSize);

            for (int i = 0; i < contextFreeActionsSize; i++)
            {
                var action = (Core.Api.v1.Action)ReadActionHeader(data, ref readIndex);
                Abi abi = await GetAbi(action.account);

                trx.context_free_actions.Add((Core.Api.v1.Action)ReadAction(data, ref readIndex, action, abi));
            }

            var actionsSize = Convert.ToInt32(ReadVarUint32(data, ref readIndex));
            trx.actions = new List<Core.Api.v1.Action>(actionsSize);

            for (int i = 0; i < actionsSize; i++)
            {
                var action = (Core.Api.v1.Action)ReadActionHeader(data, ref readIndex);
                Abi abi = await GetAbi(action.account);

                trx.actions.Add((Core.Api.v1.Action)ReadAction(data, ref readIndex, action, abi));
            }

            return trx;
        }

        public Abi DeserializePackedAbi(string packabi)
        {
            var data = SerializationHelper.Base64FcStringToByteArray(packabi);
            int readIndex = 0;

            return new Abi()
            {
                version = (string)ReadString(data, ref readIndex),
                types = ReadType<List<AbiType>>(data, ref readIndex),
                structs = ReadType<List<AbiStruct>>(data, ref readIndex),
                actions = ReadType<List<AbiAction>>(data, ref readIndex),
                tables = ReadType<List<AbiTable>>(data, ref readIndex),
                ricardian_clauses = ReadType<List<AbiRicardianClause>>(data, ref readIndex),
                error_messages = ReadType<List<string>>(data, ref readIndex),
                abi_extensions = ReadType<List<Extension>>(data, ref readIndex)
            };
        }

        public byte[] SerializeActionData(Core.Api.v1.Action action, Abi abi)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var abiAction = abi.actions.First(aa => aa.name == action.name);
                var abiStruct = abi.structs.First(s => s.name == abiAction.type);
                WriteAbiStruct(ms, action.data, abiStruct, abi);

                return ms.ToArray();
            }
        }

        public object DeserializeStructData(string structType, string dataHex, Abi abi)
        {
            return DeserializeStructData<object>(structType, dataHex, abi);
        }

        public TStructData DeserializeStructData<TStructData>(string structType, string dataHex, Abi abi)
        {
            var data = SerializationHelper.HexStringToByteArray(dataHex);
            var abiStruct = abi.structs.First(s => s.name == structType);
            int readIndex = 0;
            return ReadAbiStruct<TStructData>(data, ref readIndex, abiStruct, abi);
        }

        public Task<Abi[]> GetTransactionAbis(Transaction trx)
        {
            var abiTasks = new List<Task<Abi>>();

            foreach (var action in trx.context_free_actions)
            {
                abiTasks.Add(GetAbi(action.account));
            }

            foreach (var action in trx.actions)
            {
                abiTasks.Add(GetAbi(action.account));
            }

            return Task.WhenAll(abiTasks);
        }

        public async Task<Abi> GetAbi(string accountName)
        {
            var result = await Api.GetRawAbi(new GetRawAbiRequest()
            {
                account_name = accountName
            });

            return DeserializePackedAbi(result.abi);
        }

        public T DeserializeType<T>(string dataHex)
        {
            return DeserializeType<T>(SerializationHelper.HexStringToByteArray(dataHex));
        }

        public T DeserializeType<T>(byte[] data)
        {
            int readIndex = 0;
            return ReadType<T>(data, ref readIndex);
        }

        #region Writer Functions

        private static void WriteByte(MemoryStream ms, object value)
        {
            ms.Write(new byte[] { Convert.ToByte(value) }, 0, 1);
        }
        
        private static void WriteUint16(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes(Convert.ToUInt16(value)), 0, 2);
        }

        private static void WriteUint32(MemoryStream ms, object value)
        {

            ms.Write(BitConverter.GetBytes(Convert.ToUInt32(value)), 0, 4);
        }

        private static void WriteInt64(MemoryStream ms, object value)
        {
            var decimalBytes = SerializationHelper.SignedDecimalToBinary(8, value.ToString());
            ms.Write(decimalBytes, 0, decimalBytes.Length);
        }

        private static void WriteUint64(MemoryStream ms, object value)
        {
            var decimalBytes = SerializationHelper.DecimalToBinary(8, value.ToString());
            ms.Write(decimalBytes, 0, decimalBytes.Length);
        }

        private static void WriteInt128(MemoryStream ms, object value)
        {
            var decimalBytes = SerializationHelper.SignedDecimalToBinary(16, value.ToString());
            ms.Write(decimalBytes, 0, decimalBytes.Length);
        }

        private static void WriteUInt128(MemoryStream ms, object value)
        {
            var decimalBytes = SerializationHelper.DecimalToBinary(16, value.ToString());
            ms.Write(decimalBytes, 0, decimalBytes.Length);
        }

        private static void WriteVarUint32(MemoryStream ms, object value)
        {
            var v = Convert.ToUInt32(value);
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
            var n = Convert.ToInt32(value);
            WriteVarUint32(ms, (UInt32)((n << 1) ^ (n >> 31)));
        }

        private static void WriteFloat32(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes((float)value), 0, 4);
        }

        private static void WriteFloat64(MemoryStream ms, object value)
        {
            ms.Write(BitConverter.GetBytes(Convert.ToDouble(value)), 0, 8);
        }

        private static void WriteFloat128(MemoryStream ms, object value)
        {
            Int32[] bits = decimal.GetBits((decimal)value);
            List<byte> bytes = new List<byte>();
            foreach (Int32 i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            ms.Write(bytes.ToArray(), 0, 16);
        }

        private static void WriteBytes(MemoryStream ms, object value)
        {
            var bytes = (byte[])value;

            WriteVarUint32(ms, (UInt32)bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
        }

        private static void WriteBool(MemoryStream ms, object value)
        {
            WriteByte(ms, (bool)value ? 1 : 0);
        }

        private static void WriteString(MemoryStream ms, object value)
        {
            var strBytes = Encoding.UTF8.GetBytes((string)value);
            WriteVarUint32(ms, (UInt32)strBytes.Length);
            if (strBytes.Length > 0)
                ms.Write(strBytes, 0, strBytes.Length); 
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
            WriteSymbol(ms, new Symbol() { name = name, precision = precision });
        }

        private static void WriteTimePoint(MemoryStream ms, object value)
        {
            var ticks = SerializationHelper.DateToTimePoint((DateTime)value);
            WriteUint32(ms, (UInt32)ticks >> 0);
            WriteUint32(ms, (UInt32)Math.Floor((double)ticks / 0x100000000) >> 0);
        }

        private static void WriteTimePointSec(MemoryStream ms, object value)
        {
            WriteUint32(ms, SerializationHelper.DateToTimePointSec((DateTime)value));
        }

        private static void WriteBlockTimestampType(MemoryStream ms, object value)
        {
            WriteUint32(ms, SerializationHelper.DateToBlockTimestamp((DateTime)value));
        }

        private static void WriteSymbolString(MemoryStream ms, object value)
        {
            Regex r = new Regex("^([0-9]+),([A-Z]+)$", RegexOptions.IgnoreCase);
            Match m = r.Match((string)value);

            if (!m.Success)
                throw new Exception("Invalid symbol.");

            WriteSymbol(ms, new Symbol() { name = m.Groups[2].ToString(), precision = byte.Parse(m.Groups[1].ToString()) });
        }

        private static void WriteSymbolCode(MemoryStream ms, object value)
        {
            var name = (string)value;

            if (name.Length > 8)
                ms.Write(Encoding.UTF8.GetBytes(name.Substring(0, 8)), 0, 8);
            else
            {
                ms.Write(Encoding.UTF8.GetBytes(name), 0, name.Length);

                if (name.Length < 8)
                {
                    var fill = new byte[8 - name.Length];
                    for (int i = 0; i < fill.Length; i++)
                        fill[i] = 0;
                    ms.Write(fill, 0, fill.Length);
                }
            }
        }

        private static void WriteChecksum160(MemoryStream ms, object value)
        {
            var bytes = SerializationHelper.HexStringToByteArray((string)value);

            if (bytes.Length != 20)
                throw new Exception("Binary data has incorrect size");

            ms.Write(bytes, 0, bytes.Length);
        }

        private static void WriteChecksum256(MemoryStream ms, object value)
        {
            var bytes = SerializationHelper.HexStringToByteArray((string)value);

            if (bytes.Length != 32)
                throw new Exception("Binary data has incorrect size");

            ms.Write(bytes, 0, bytes.Length);
        }

        private static void WriteChecksum512(MemoryStream ms, object value)
        {
            var bytes = SerializationHelper.HexStringToByteArray((string)value);

            if (bytes.Length != 64)
                throw new Exception("Binary data has incorrect size");

            ms.Write(bytes, 0, bytes.Length);
        }
        
        private static void WritePublicKey(MemoryStream ms, object value)
        {
            var s = (string)value;
            var keyBytes = CryptoHelper.PubKeyStringToBytes(s);

            WriteByte(ms, s.StartsWith("PUB_R1_") ? KeyType.r1 : KeyType.k1);
            ms.Write(keyBytes, 0, CryptoHelper.PUB_KEY_DATA_SIZE);
        }

        private static void WritePrivateKey(MemoryStream ms, object value)
        {
            var s = (string)value;
            var keyBytes = CryptoHelper.PrivKeyStringToBytes(s);
            WriteByte(ms, KeyType.r1);
            ms.Write(keyBytes, 0, CryptoHelper.PRIV_KEY_DATA_SIZE);
        }

        private static void WriteSignature(MemoryStream ms, object value)
        {
            var s = (string)value;
            var signBytes = CryptoHelper.SignStringToBytes(s);
            
            if (s.StartsWith("SIG_K1_"))
                WriteByte(ms, KeyType.k1);
            else if (s.StartsWith("SIG_R1_"))
                WriteByte(ms, KeyType.r1);

            ms.Write(signBytes, 0, CryptoHelper.SIGN_KEY_DATA_SIZE);
        }

        private static void WriteExtendedAsset(MemoryStream ms, object value)
        {
            var extAsset = (ExtendedAsset)value;
            WriteAsset(ms, extAsset.quantity);
            WriteName(ms, extAsset.contract);
        }

        private static void WriteSymbol(MemoryStream ms, object value)
        {
            var symbol = (Symbol)value;

            WriteByte(ms, symbol.precision);

            if (symbol.name.Length > 7)
                ms.Write(Encoding.UTF8.GetBytes(symbol.name.Substring(0, 7)), 0, 7);
            else
            {
                ms.Write(Encoding.UTF8.GetBytes(symbol.name), 0, symbol.name.Length);

                if (symbol.name.Length < 7)
                {
                    var fill = new byte[7 - symbol.name.Length];
                    for (int i = 0; i < fill.Length; i++)
                        fill[i] = 0;
                    ms.Write(fill, 0, fill.Length);
                }
            }
        }

        private static void WriteExtension(MemoryStream ms, Core.Api.v1.Extension extension)
        {
            if (extension.data == null)
                return;

            WriteUint16(ms, extension.type);
            WriteBytes(ms, extension.data);
        }

        private static void WritePermissionLevel(MemoryStream ms, PermissionLevel perm)
        {
            WriteName(ms, perm.actor);
            WriteName(ms, perm.permission);
        }

        private void WriteAction(MemoryStream ms, Core.Api.v1.Action action, Abi abi)
        {
            WriteName(ms, action.account);
            WriteName(ms, action.name);

            WriteVarUint32(ms, (UInt32)action.authorization.Count);
            foreach (var perm in action.authorization)
            {
                WritePermissionLevel(ms, perm);
            }

            WriteBytes(ms, SerializeActionData(action, abi));
        }

        private void WriteAbiType(MemoryStream ms, object value, string type, Abi abi)
        {
            //optional type
            if(type.EndsWith("?"))
            {
                WriteByte(ms, value != null ? 1 : 0);
                if(value != null)
                {
                    WriteByte(ms, 1);
                    type.Substring(0, type.Length - 1);
                }
                else
                {
                    WriteByte(ms, 0);
                    return;
                }
            }

            // array type
            if(type.EndsWith("[]"))
            {
                var items = (IEnumerable<object>)value;
                var arrayType = type.Substring(0, type.Length - 2);

                WriteVarUint32(ms, items.Count());
                foreach (var item in items)
                    WriteAbiType(ms, item, arrayType, abi);

                return;
            }

            var writer = GetTypeSerializerAndCache(type, TypeWriters, abi);

            if (writer != null)
            {
                writer(ms, value);
            }
            else
            {
                var abiStruct = abi.structs.FirstOrDefault(s => s.name == type);
                if (abiStruct != null)
                {
                    WriteAbiStruct(ms, value, abiStruct, abi);
                }
                else
                {
                    throw new Exception("Type supported writer not found.");
                }
            }
        }

        private void WriteAbiStruct(MemoryStream ms, object value, AbiStruct abiStruct, Abi abi)
        {
            if (value == null)
                return;

            if(!string.IsNullOrWhiteSpace(abiStruct.@base))
            {
                WriteAbiType(ms, value, abiStruct.@base, abi);
            }

            if(value is System.Collections.IDictionary)
            {
                var valueDict = value as System.Collections.IDictionary;
                foreach (var field in abiStruct.fields)
                {
                    var fieldName = FindObjectFieldName(field.name, valueDict);

                    if (string.IsNullOrWhiteSpace(fieldName))
                        throw new Exception("Missing " + abiStruct.name + "." + field.name + " (type=" + field.type + ")");

                    WriteAbiType(ms, valueDict[fieldName], field.type, abi);
                }
            }
            else
            {
                var valueType = value.GetType();
                foreach (var field in abiStruct.fields)
                {
                    var fieldName = FindObjectFieldName(field.name, value.GetType());

                    if (string.IsNullOrWhiteSpace(fieldName))
                        throw new Exception("Missing " + abiStruct.name + "." + field.name + " (type=" + field.type + ")");

                    WriteAbiType(ms, valueType.GetProperty(fieldName).GetValue(value, null), field.type, abi);
                }
            }
        }

        private TSerializer GetTypeSerializerAndCache<TSerializer>(string type, Dictionary<string, TSerializer> typeSerializers, Abi abi)
        {
            TSerializer nativeSerializer;
            if (typeSerializers.TryGetValue(type, out nativeSerializer))
            {
                return nativeSerializer;
            }

            var abiType = abi.types.FirstOrDefault(t => t.new_type_name == type);

            if(abiType != null)
            {
                var serializer = GetTypeSerializerAndCache(abiType.type, typeSerializers, abi);

                if(serializer != null)
                {
                    typeSerializers.Add(type, serializer);
                    return serializer;
                }
            }

            return default(TSerializer);
        }

        #endregion

        #region Reader Functions
        private static object ReadByte(byte[] data, ref Int32 readIndex)
        {
            return data[readIndex++];
        }

        private static object ReadUint16(byte[] data, ref Int32 readIndex)
        {
            var value = BitConverter.ToUInt16(data, readIndex);
            readIndex += 2;
            return value;
        }

        private static object ReadUint32(byte[] data, ref Int32 readIndex)
        {
            var value = BitConverter.ToUInt32(data, readIndex);
            readIndex += 4;
            return value;
        }

        private static object ReadInt64(byte[] data, ref Int32 readIndex)
        {
            var value = (Int64)BitConverter.ToUInt64(data, readIndex);
            readIndex += 8;
            return value;
        }

        private static object ReadUint64(byte[] data, ref Int32 readIndex)
        {
            var value = BitConverter.ToUInt64(data, readIndex);
            readIndex += 8;
            return value;
        }

        private static object ReadInt128(byte[] data, ref Int32 readIndex)
        {
            byte[] amount = data.Skip(readIndex).Take(16).ToArray();
            readIndex += 16;
            return SerializationHelper.SignedBinaryToDecimal(amount);
        }

        private static object ReadUInt128(byte[] data, ref Int32 readIndex)
        {
            byte[] amount = data.Skip(readIndex).Take(16).ToArray();
            readIndex += 16;
            return SerializationHelper.BinaryToDecimal(amount);
        }

        private static object ReadVarUint32(byte[] data, ref Int32 readIndex)
        {
            uint v = 0;
            int bit = 0;
            while (true)
            {
                byte b = data[readIndex++];
                v |= (uint)((b & 0x7f) << bit);
                bit += 7;
                if ((b & 0x80) == 0)
                    break;
            }
            return v >> 0;
        }

        private static object ReadVarInt32(byte[] data, ref Int32 readIndex)
        {
            var v = (UInt32)ReadVarUint32(data, ref readIndex);

            if ((v & 1) != 0)
                return ((~v) >> 1) | 0x80000000;
            else
                return v >> 1;
        }

        private static object ReadFloat32(byte[] data, ref Int32 readIndex)
        {
            var value = BitConverter.ToSingle(data, readIndex);
            readIndex += 4;
            return value;
        }

        private static object ReadFloat64(byte[] data, ref Int32 readIndex)
        {
            var value = BitConverter.ToDouble(data, readIndex);
            readIndex += 8;
            return value;
        }

        private static object ReadFloat128(byte[] data, ref Int32 readIndex)
        {
            var a = data.Skip(readIndex).Take(16).ToArray();
            var value = SerializationHelper.ByteArrayToHexString(a);
            readIndex += 16;
            return value;
        }

        private static object ReadBytes(byte[] data, ref Int32 readIndex)
        {
            var size = Convert.ToInt32(ReadVarUint32(data, ref readIndex));
            var value = data.Skip(readIndex).Take(size).ToArray();
            readIndex += size;
            return value;
        }

        private static object ReadBool(byte[] data, ref Int32 readIndex)
        {
            return (byte)ReadByte(data, ref readIndex) == 1;
        }

        private static object ReadString(byte[] data, ref Int32 readIndex)
        {
            var size = Convert.ToInt32(ReadVarUint32(data, ref readIndex));
            string value = null;
            if (size > 0)
            {
                value = Encoding.UTF8.GetString(data.Skip(readIndex).Take(size).ToArray());
                readIndex += size;
            }
            return value;
        }

        private static object ReadName(byte[] data, ref Int32 readIndex)
        {
            byte[] a = data.Skip(readIndex).Take(8).ToArray();
            string result = "";

            readIndex += 8;

            for (int bit = 63; bit >= 0;)
            {
                int c = 0;
                for (int i = 0; i < 5; ++i)
                {
                    if (bit >= 0)
                    {
                        c = (c << 1) | ((a[(int)Math.Floor((double)bit / 8)] >> (bit % 8)) & 1);
                        --bit;
                    }
                }
                if (c >= 6)
                    result += (char)(c + 'a' - 6);
                else if (c >= 1)
                    result += (char)(c + '1' - 1);
                else
                    result += '.';
            }

            if (result == ".............")
                return result;

            while (result.EndsWith("."))
                result = result.Substring(0, result.Length - 1);

            return result;
        }

        private static object ReadAsset(byte[] data, ref Int32 readIndex)
        {
            byte[] amount = data.Skip(readIndex).Take(8).ToArray();

            readIndex += 8;

            var symbol = (Symbol)ReadSymbol(data, ref readIndex);
            string s = SerializationHelper.SignedBinaryToDecimal(amount, symbol.precision + 1);

            if (symbol.precision > 0)
                s = s.Substring(0, s.Length - symbol.precision) + '.' + s.Substring(s.Length - symbol.precision);

            return s + ' ' + symbol.name;
        }

        private static object ReadTimePoint(byte[] data, ref Int32 readIndex)
        {
            var low = (UInt32)ReadUint32(data, ref readIndex);
            var high = (UInt32)ReadUint32(data, ref readIndex);
            return SerializationHelper.TimePointToDate((high >> 0) * 0x100000000 + (low >> 0));
        }

        private static object ReadTimePointSec(byte[] data, ref Int32 readIndex)
        {
            var secs = (UInt32)ReadUint32(data, ref readIndex);
            return SerializationHelper.TimePointSecToDate(secs);
        }

        private static object ReadBlockTimestampType(byte[] data, ref Int32 readIndex)
        {
            var slot = (UInt32)ReadUint32(data, ref readIndex);
            return SerializationHelper.BlockTimestampToDate(slot);
        }

        private static object ReadSymbolString(byte[] data, ref Int32 readIndex)
        {
            var value = (Symbol)ReadSymbol(data, ref readIndex);
            return value.precision + ',' + value.name;
        }

        private static object ReadSymbolCode(byte[] data, ref Int32 readIndex)
        {
            byte[] a = data.Skip(readIndex).Take(8).ToArray();

            readIndex += 8;

            int len;
            for (len = 0; len < a.Length; ++len)
                if (a[len] == 0)
                    break;

            return string.Join("", a.Take(len));
        }

        private static object ReadChecksum160(byte[] data, ref Int32 readIndex)
        {
            var a = data.Skip(readIndex).Take(20).ToArray();
            var value = SerializationHelper.ByteArrayToHexString(a);
            readIndex += 20;
            return value;
        }

        private static object ReadChecksum256(byte[] data, ref Int32 readIndex)
        {
            var a = data.Skip(readIndex).Take(32).ToArray();
            var value = SerializationHelper.ByteArrayToHexString(a);
            readIndex += 32;
            return value;
        }

        private static object ReadChecksum512(byte[] data, ref Int32 readIndex)
        {
            var a = data.Skip(readIndex).Take(64).ToArray();
            var value = SerializationHelper.ByteArrayToHexString(a);
            readIndex += 64;
            return value;
        }

        private static object ReadPublicKey(byte[] data, ref Int32 readIndex)
        {
            var type = (byte)ReadByte(data, ref readIndex);
            var keyBytes = data.Skip(readIndex).Take(CryptoHelper.PUB_KEY_DATA_SIZE).ToArray();

            readIndex += CryptoHelper.PUB_KEY_DATA_SIZE;

            if(type == (int)KeyType.k1)
            {
                return CryptoHelper.PubKeyBytesToString(keyBytes, "K1");
            }
            if (type == (int)KeyType.r1)
            {
                return CryptoHelper.PubKeyBytesToString(keyBytes, "R1", "PUB_R1_");
            }
            else
            {
                throw new Exception("public key type not supported.");
            }
        }

        private static object ReadPrivateKey(byte[] data, ref Int32 readIndex)
        {
            var type = (byte)ReadByte(data, ref readIndex);
            var keyBytes = data.Skip(readIndex).Take(CryptoHelper.PRIV_KEY_DATA_SIZE).ToArray();

            readIndex += CryptoHelper.PRIV_KEY_DATA_SIZE;

            if (type == (int)KeyType.r1)
            {
                return CryptoHelper.PrivKeyBytesToString(keyBytes, "R1", "PVT_R1_");
            }
            else
            {
                throw new Exception("private key type not supported.");
            }
        }

        private static object ReadSignature(byte[] data, ref Int32 readIndex)
        {
            var type = (byte)ReadByte(data, ref readIndex);
            var signBytes = data.Skip(readIndex).Take(CryptoHelper.SIGN_KEY_DATA_SIZE).ToArray();

            readIndex += CryptoHelper.SIGN_KEY_DATA_SIZE;

            if (type == (int)KeyType.r1)
            {
                return CryptoHelper.SignBytesToString(signBytes, "R1", "SIG_R1_");
            }
            else if (type == (int)KeyType.k1)
            {
                return CryptoHelper.SignBytesToString(signBytes, "K1", "SIG_K1_");
            }
            else
            {
                throw new Exception("signature type not supported.");
            }
        }

        private static object ReadExtendedAsset(byte[] data, ref Int32 readIndex)
        {
            return new ExtendedAsset()
            {
                quantity = (string)ReadAsset(data, ref readIndex),
                contract = (string)ReadName(data, ref readIndex)
            };
        }

        private static object ReadSymbol(byte[] data, ref Int32 readIndex)
        {
            var value = new Symbol
            {
                precision = (byte)ReadByte(data, ref readIndex)
            };

            byte[] a = data.Skip(readIndex).Take(7).ToArray();

            readIndex += 7;

            int len;
            for (len = 0; len < a.Length; ++len)
                if (a[len] == 0)
                    break;

            value.name = string.Join("", a.Take(len).Select(b => (char)b));

            return value;
        }

        private static object ReadPermissionLevel(byte[] data, ref Int32 readIndex)
        {
            var value = new PermissionLevel()
            {
                actor = (string)ReadName(data, ref readIndex),
                permission = (string)ReadName(data, ref readIndex),
            };
            return value;
        }

        private static object ReadActionHeader(byte[] data, ref Int32 readIndex)
        {
            return new Core.Api.v1.Action()
            {
                account = (string)ReadName(data, ref readIndex),
                name = (string)ReadName(data, ref readIndex)
            };
        }

        private object ReadAction(byte[] data, ref Int32 readIndex, Core.Api.v1.Action action, Abi abi)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var size = Convert.ToInt32(ReadVarUint32(data, ref readIndex));

            action.authorization = new List<PermissionLevel>(size);
            for (var i = 0; i < size ; i++)
            {
                action.authorization.Add((PermissionLevel)ReadPermissionLevel(data, ref readIndex));
            }

            var abiAction = abi.actions.First(aa => aa.name == action.name);
            var abiStruct = abi.structs.First(s => s.name == abiAction.type);

            var dataSize = Convert.ToInt32(ReadVarUint32(data, ref readIndex));

            action.data = ReadAbiStruct(data, ref readIndex, abiStruct, abi);

            action.hex_data = (string)ReadString(data, ref readIndex);

            return action;
        }

        private object ReadAbiType(byte[] data, ref Int32 readIndex, string type, Abi abi)
        {
            object value = null;

            //optional type
            if (type.EndsWith("?"))
            {
                var opt = (byte)ReadByte(data, ref readIndex);

                if (opt == 0)
                {
                    return value;
                }
            }

            // array type
            if (type.EndsWith("[]"))
            {
                var arrayType = type.Substring(0, type.Length - 2);
                var size = Convert.ToInt32(ReadVarUint32(data, ref readIndex));
                var items = new List<object>(size);

                for (int i = 0; i < size; i++)
                {
                    items.Add(ReadAbiType(data, ref readIndex, arrayType, abi));
                }

                return items;
            }

            var reader = GetTypeSerializerAndCache(type, TypeReaders, abi);

            if (reader != null)
            {
                value = reader(data, ref readIndex);
            }
            else
            {
                var abiStruct = abi.structs.FirstOrDefault(s => s.name == type);
                if (abiStruct != null)
                {
                    value = ReadAbiStruct(data, ref readIndex, abiStruct, abi);
                }
                else
                {
                    throw new Exception("Type supported writer not found.");
                }
            }
            
            return value;
        }

        private object ReadAbiStruct(byte[] data, ref Int32 readIndex, AbiStruct abiStruct, Abi abi)
        {
            return ReadAbiStruct<object>(data, ref readIndex, abiStruct, abi);
        }

        private T ReadAbiStruct<T>(byte[] data, ref Int32 readIndex, AbiStruct abiStruct, Abi abi)
        {
            object value = default(T);

            if (!string.IsNullOrWhiteSpace(abiStruct.@base))
            {
                value = (T)ReadAbiType(data, ref readIndex, abiStruct.@base, abi);
            }
            else if(typeof(T) == typeof(object))
            {
                value = new ExpandoObject();
            }
            else
            {
                value = Activator.CreateInstance(typeof(T));
            }

            var valueType = value.GetType();
            foreach (var field in abiStruct.fields)
            {
                var abiValue = ReadAbiType(data, ref readIndex, field.type, abi);
                var fieldName = FindObjectFieldName(field.name, value.GetType());

                if(string.IsNullOrWhiteSpace(fieldName))
                {
                    if (valueType == typeof(ExpandoObject))
                    {
                        (value as IDictionary<string, Object>).Add(field.name, abiValue);
                    }                    
                    else if (typeof(T) == typeof(object))
                        valueType.GetProperty(field.name).SetValue(value, abiValue);

                    continue;
                }

                valueType.GetProperty(fieldName).SetValue(value, abiValue);
            }

            return (T)value;
        }

        private T ReadType<T>(byte[] data, ref int readIndex)
        {
            return (T)ReadType(data, typeof(T), ref readIndex);
        }

        private object ReadType(byte[] data, Type objectType, ref int readIndex)
        {
            if (IsCollection(objectType))
            {
                return ReadCollectionType(data, objectType, ref readIndex);
            }
            else if (IsOptional(objectType))
            {
                var opt = (byte)ReadByte(data, ref readIndex);
                if (opt == 1)
                {
                    var optionalType = GetFirstGenericType(objectType);
                    return ReadType(data, optionalType, ref readIndex);
                }
            }
            else if (IsPrimitive(objectType))
            {
                var readerName = GetNormalizedReaderName(objectType);
                return TypeReaders[readerName](data, ref readIndex);
            }

            var value = Activator.CreateInstance(objectType);

            foreach (var member in objectType.GetFields())
            {
                if(IsCollection(member.FieldType))
                {
                    objectType.GetField(member.Name).SetValue(value, ReadCollectionType(data, member.FieldType, ref readIndex));
                }
                else if(IsOptional(member.FieldType))
                {
                    var opt = (byte)ReadByte(data, ref readIndex);
                    if (opt == 1)
                    {
                        var optionalType = GetFirstGenericType(member.FieldType);
                        objectType.GetField(member.Name).SetValue(value, ReadType(data, optionalType, ref readIndex));
                    }
                }
                else if (IsPrimitive(member.FieldType))
                {
                    var readerName = GetNormalizedReaderName(member.FieldType, member.GetCustomAttributes());
                    objectType.GetField(member.Name).SetValue(value, TypeReaders[readerName](data, ref readIndex));
                }
                else
                {
                    objectType.GetField(member.Name).SetValue(value, ReadType(data, member.FieldType, ref readIndex));
                }
            }

            return value;
        }

        private IList ReadCollectionType(byte[] data, Type objectType, ref int readIndex)
        {
            var collectionType = GetFirstGenericType(objectType);
            var size = Convert.ToInt32(ReadVarUint32(data, ref readIndex));
            IList items = (IList)Activator.CreateInstance(objectType);

            for (int i = 0; i < size; i++)
            {
                items.Add(ReadType(data, collectionType, ref readIndex));
            }

            return items;
        }

        private static bool IsCollection(Type type)
        {
            return type.Name.StartsWith("List");
        }

        private static bool IsOptional(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static Type GetFirstGenericType(Type type)
        {
            return type.GetGenericArguments().First();
        }

        private static bool IsPrimitive(Type type)
        {
            return type.IsPrimitive ||                   
                   type.Name.ToLower() == "string" ||
                   type.Name.ToLower() == "byte[]";
        }

        private static string GetNormalizedReaderName(Type type, IEnumerable<Attribute> customAttributes = null)
        {
            if(customAttributes != null)
            {
                var abiFieldAttr = (AbiFieldTypeAttribute)customAttributes.FirstOrDefault(attr => attr.GetType() == typeof(AbiFieldTypeAttribute));
                if (abiFieldAttr != null)
                    return abiFieldAttr.AbiType;
            }

            var typeName = type.Name.ToLower();

            if (typeName == "byte[]")
                return "bytes";
            else if (typeName == "boolean")
                return "bool";

            return typeName;
        }

        private string FindObjectFieldName(string name, System.Collections.IDictionary value)
        {
            if (value.Contains(name))
                return name;

            name = SerializationHelper.SnakeCaseToPascalCase(name);

            if (value.Contains(name))
                return name;

            name = SerializationHelper.PascalCaseToSnakeCase(name);

            if (value.Contains(name))
                return name;

            return null;
        }

        private string FindObjectFieldName(string name, Type objectType)
        {
            if (objectType.GetProperties().Any(p => p.Name == name))
                return name;

            name = SerializationHelper.SnakeCaseToPascalCase(name);

            if (objectType.GetProperties().Any(p => p.Name == name))
                return name;

            name = SerializationHelper.PascalCaseToSnakeCase(name);

            if (objectType.GetProperties().Any(p => p.Name == name))
                return name;

            return null;
        }
        #endregion
    }
}

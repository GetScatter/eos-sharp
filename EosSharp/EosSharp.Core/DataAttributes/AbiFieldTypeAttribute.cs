using System;

namespace EosSharp.Core.DataAttributes
{
    public class AbiFieldTypeAttribute : Attribute
    {
        public string AbiType { get; set; }

        public AbiFieldTypeAttribute(string abiType)
        {
            AbiType = abiType;
        }
    }
}

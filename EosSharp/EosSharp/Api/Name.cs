using System;
using System.Collections.Generic;
using System.Text;

namespace EosSharp.Api
{
    public class Name
    {
        readonly string _value;
        public Name(string value)
        {
            this._value = value;
        }
        public static implicit operator String(Name d)
        {
            return d._value;
        }
        public static implicit operator Name(string d)
        {
            return new Name(d);
        }

        public override string ToString()
        {
            return this._value;
        }
    }
}

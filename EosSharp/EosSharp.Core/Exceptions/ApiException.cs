using System;

namespace EosSharp.Core.Exceptions
{   
    /// <summary>
    /// Generic Api exception
    /// </summary>
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }
    }
}

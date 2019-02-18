using System;
using System.Collections.Generic;

namespace EosSharp.Core.Exceptions
{
    /// <summary>
    /// Wrapper exception for EOSIO api error
    /// </summary>
    public class ApiErrorException : Exception
    {
        public int Code { get; set; }
        public ApiError Error { get; set; }
    }

    /// <summary>
    /// EOSIO Api Error
    /// </summary>
    public class ApiError
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string What { get; set; }
        public List<ApiErrorDetail> Details { get; set; }
    }

    /// <summary>
    /// EOSIO Api Error detail
    /// </summary>
    public class ApiErrorDetail
    {
        public string Message { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }
        public string Method { get; set; }
    }
}

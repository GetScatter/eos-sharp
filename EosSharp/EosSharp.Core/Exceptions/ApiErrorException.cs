using System;
using System.Collections.Generic;

namespace EosSharp.Core.Exceptions
{
    public class ApiErrorException : Exception
    {
        public int Code { get; set; }
        public ApiError Error { get; set; }
    }

    public class ApiError
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string What { get; set; }
        public List<ApiErrorDetail> Details { get; set; }
    }

    public class ApiErrorDetail
    {
        public string Message { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }
        public string Method { get; set; }
    }
}

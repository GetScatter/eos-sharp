using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EosSharp.Exceptions
{
    public class ApiErrorException : Exception
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("error")]
        public ApiError Error { get; set; }
    }

    public class ApiError
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("what")]
        public string What { get; set; }
        [JsonProperty("details")]
        public List<ApiErrorDetail> Details { get; set; }
    }

    public class ApiErrorDetail
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("file")]
        public string File { get; set; }
        [JsonProperty("line_number")]
        public int LineNumber { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}

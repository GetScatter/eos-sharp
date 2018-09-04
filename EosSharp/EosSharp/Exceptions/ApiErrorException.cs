using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EosSharp.Exceptions
{
    public class ApiErrorException : Exception
    {
        public int Code { get; set; }
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
        public List<string> Details { get; set; }
    }
}

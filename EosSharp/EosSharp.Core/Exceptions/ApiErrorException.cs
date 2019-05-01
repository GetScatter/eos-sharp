using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EosSharp.Core.Exceptions
{
    /// <summary>
    /// Wrapper exception for EOSIO api error
    /// </summary>
    [Serializable]
    public class ApiErrorException : Exception
    {
        public int code;
        public string message;
        public ApiError error;

        public ApiErrorException(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                return;

            code = info.GetInt32("code");
            message = info.GetString("message");
            error = (ApiError)info.GetValue("error", typeof(ApiError));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                return;

            base.GetObjectData(info, context);
            info.AddValue("code", code);
            info.AddValue("message", message);
            info.AddValue("error", error);
        }
    }

    /// <summary>
    /// EOSIO Api Error
    /// </summary>
    [Serializable]
    public class ApiError
    {
        public int code;
        public string name;
        public string what;
        public List<ApiErrorDetail> details;
    }

    /// <summary>
    /// EOSIO Api Error detail
    /// </summary>
    [Serializable]
    public class ApiErrorDetail
    {
        public string message;
        public string file;
        public int line_number;
        public string method;
    }
}

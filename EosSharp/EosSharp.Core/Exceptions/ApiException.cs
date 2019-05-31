using System;
using System.Runtime.Serialization;

namespace EosSharp.Core.Exceptions
{
    /// <summary>
    /// Generic Api exception
    /// </summary>
    [Serializable]
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }

        public ApiException()
        {

        }

        public ApiException(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                return;

            StatusCode = info.GetInt32("StatusCode");
            Content = info.GetString("Content");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                return;

            base.GetObjectData(info, context);
            info.AddValue("StatusCode", StatusCode);
            info.AddValue("Content", Content);
        }
    }
}

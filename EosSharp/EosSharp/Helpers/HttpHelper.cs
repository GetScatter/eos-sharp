using Cryptography.ECDSA;
using EosSharp.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EosSharp.Helpers
{
    public class HttpHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static Dictionary<string, object> ResponseCache { get; set; } = new Dictionary<string, object>();

        public static void ClearResponseCache()
        {
            ResponseCache.Clear();
        }

        public static async Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, JsonSerializerSettings jsonSettings = null)
        {
            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync(request);
            return DeserializeJsonFromStream<TResponseData>(result);
        }

        public static async Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, JsonSerializerSettings jsonSettings = null)
        {
            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync(request, cancellationToken);
            return DeserializeJsonFromStream<TResponseData>(result);
        }

        public static async Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, bool reload = false, JsonSerializerSettings jsonSettings = null)
        {
            string hashKey = GetRequestHashKey(url, data);

            if (!reload)
            {
                if (ResponseCache.TryGetValue(hashKey, out object value))
                    return (TResponseData)value;
            }

            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync(request);
            var responseData = DeserializeJsonFromStream<TResponseData>(result);
            UpdateResponseDataCache(hashKey, responseData);

            return responseData;
        }

        public static async Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, bool reload = false, JsonSerializerSettings jsonSettings = null)
        {
            string hashKey = GetRequestHashKey(url, data);

            if (!reload)
            {
                if (ResponseCache.TryGetValue(hashKey, out object value))
                    return (TResponseData)value;
            }

            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync(request, cancellationToken);
            var responseData = DeserializeJsonFromStream<TResponseData>(result);
            UpdateResponseDataCache(hashKey, responseData);

            return responseData;
        }

        public static async Task<TResponseData> GetJsonAsync<TResponseData>(string url, JsonSerializerSettings jsonSettings = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await SendAsync(request);
            return DeserializeJsonFromStream<TResponseData>(result, jsonSettings);
        }

        public static async Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken, JsonSerializerSettings jsonSettings = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await SendAsync(request, cancellationToken);
            return DeserializeJsonFromStream<TResponseData>(result, jsonSettings);
        }

        public static async Task<Stream> SendAsync(HttpRequestMessage request)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await BuildSendResponse(response);
        }

        public static async Task<Stream> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return await BuildSendResponse(response);
        }

        public static TData DeserializeJsonFromStream<TData>(Stream stream, JsonSerializerSettings jsonSettings = null)
        {
            if (stream == null || stream.CanRead == false)
                return default(TData);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                return JsonSerializer.Create(jsonSettings).Deserialize<TData>(jtr);
            }
        }

        public static HttpRequestMessage BuildJsonRequestMessage(string url, object data)
        {
            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
        }


        private static void UpdateResponseDataCache<TResponseData>(string hashKey, TResponseData responseData)
        {
            if (ResponseCache.ContainsKey(hashKey))
            {
                ResponseCache[hashKey] = responseData;
            }
            else
            {
                ResponseCache.Add(hashKey, responseData);
            }
        }

        private static string GetRequestHashKey(string url, object data)
        {
            var keyBytes = new List<byte[]>()
            {
                Encoding.UTF8.GetBytes(url),
                SerializationHelper.ObjectToByteArray(data)
            };
            return Encoding.Default.GetString(Sha256Manager.GetHash(SerializationHelper.Combine(keyBytes)));
        }

        private static async Task<Stream> BuildSendResponse(HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return stream;

            var content = await StreamToStringAsync(stream);

            ApiErrorException apiError = null;
            try
            {
                apiError = JsonConvert.DeserializeObject<ApiErrorException>(content);
            }
            catch(Exception)
            {
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }

            throw apiError;
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }
    }
}

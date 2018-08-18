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

        public static async Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data)
        {
            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync<TResponseData>(request);
            return DeserializeJsonFromStream<TResponseData>(result);
        }

        public static async Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync<TResponseData>(request, cancellationToken);
            return DeserializeJsonFromStream<TResponseData>(result);
        }

        public static async Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, bool reload = false)
        {
            string hashKey = GetRequestHashKey(url, data);

            if (!reload)
            {
                if (ResponseCache.TryGetValue(hashKey, out object value))
                    return (TResponseData)value;
            }

            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync<TResponseData>(request);
            var responseData = DeserializeJsonFromStream<TResponseData>(result);
            UpdateResponseDataCache(hashKey, responseData);

            return responseData;
        }

        public static async Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, bool reload = false)
        {
            string hashKey = GetRequestHashKey(url, data);

            if (!reload)
            {
                if (ResponseCache.TryGetValue(hashKey, out object value))
                    return (TResponseData)value;
            }

            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync<TResponseData>(request, cancellationToken);
            var responseData = DeserializeJsonFromStream<TResponseData>(result);
            UpdateResponseDataCache(hashKey, responseData);

            return responseData;
        }

        public static async Task<TResponseData> GetJsonAsync<TResponseData>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await SendAsync<TResponseData>(request);
            return DeserializeJsonFromStream<TResponseData>(result);
        }

        public static async Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await SendAsync<TResponseData>(request, cancellationToken);
            return DeserializeJsonFromStream<TResponseData>(result);
        }

        public static async Task<Stream> SendAsync<TResponseData>(HttpRequestMessage request)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await BuildSendResponse(response);
        }

        public static async Task<Stream> SendAsync<TResponseData>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return await BuildSendResponse(response);
        }

        public static TData DeserializeJsonFromStream<TData>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(TData);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                return new JsonSerializer().Deserialize<TData>(jtr);
            }
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
            var keyBytes = new List<byte>(Encoding.UTF8.GetBytes(url));
            keyBytes.AddRange(ChainHelper.ObjectToByteArray(data));
            var hashKey = Encoding.Default.GetString(Sha256Manager.GetHash(keyBytes.ToArray()));
            return hashKey;
        }

        private static async Task<Stream> BuildSendResponse(HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return stream;

            var content = await StreamToStringAsync(stream);
            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
        }

        private static HttpRequestMessage BuildJsonRequestMessage(string url, object data)
        {
            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
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

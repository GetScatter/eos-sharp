using Cryptography.ECDSA;
using EosSharp.Core.Exceptions;
using EosSharp.Core.Helpers;
using EosSharp.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace EosSharp.Unity3D
{
    public class HttpHelper : IHttpHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static Dictionary<string, object> ResponseCache { get; set; } = new Dictionary<string, object>();

        public void ClearResponseCache()
        {
            ResponseCache.Clear();
        }

        public async Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data)
        {
            UnityWebRequest uwr = BuildUnityWebRequest(url, UnityWebRequest.kHttpVerbPOST, data);

            await uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Console.WriteLine("Error While Sending: " + uwr.error);
            }

            return JsonConvert.DeserializeObject<TResponseData>(uwr.downloadHandler.text);
        }

        public Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, CancellationToken cancellationToken)
        {
            return PostJsonAsync<TResponseData>(url, data);
        }

        public async Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, bool reload = false)
        {
            string hashKey = GetRequestHashKey(url, data);

            if (!reload)
            {
                object value;
                if (ResponseCache.TryGetValue(hashKey, out value))
                    return (TResponseData)value;
            }

            UnityWebRequest uwr = BuildUnityWebRequest(url, UnityWebRequest.kHttpVerbPOST, data);

            await uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Console.WriteLine("Error While Sending: " + uwr.error);
            }
            else
            {
                Console.WriteLine("Received: " + uwr.downloadHandler.text);
            }

            return JsonConvert.DeserializeObject<TResponseData>(uwr.downloadHandler.text);
        }

        public Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, bool reload = false)
        {
            return PostJsonWithCacheAsync<TResponseData>(url, data, reload);
        }

        public async Task<TResponseData> GetJsonAsync<TResponseData>(string url)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);

            await uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Console.WriteLine("Error While Sending: " + uwr.error);
            }
            else
            {
                Console.WriteLine("Received: " + uwr.downloadHandler.text);
            }

            return JsonConvert.DeserializeObject<TResponseData>(uwr.downloadHandler.text);
        }

        public Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken)
        {
            return GetJsonAsync<TResponseData>(url);
        }

        public async Task<Stream> SendAsync(HttpRequestMessage request)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await BuildSendResponse(response);
        }

        public async Task<Stream> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return await BuildSendResponse(response);
        }

        public void UpdateResponseDataCache<TResponseData>(string hashKey, TResponseData responseData)
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

        public string GetRequestHashKey(string url, object data)
        {
            var keyBytes = new List<byte[]>()
            {
                Encoding.UTF8.GetBytes(url),
                SerializationHelper.ObjectToByteArray(data)
            };
            return Encoding.Default.GetString(Sha256Manager.GetHash(SerializationHelper.Combine(keyBytes)));
        }

        public async Task<Stream> BuildSendResponse(HttpResponseMessage response)
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

        public async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }

        private static UnityWebRequest BuildUnityWebRequest(string url, string verb, object data)
        {
            var uwr = new UnityWebRequest(url, verb)
            {
                uploadHandler = (UploadHandler)new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data))),
                downloadHandler = (DownloadHandler)new DownloadHandlerBuffer()
            };

            uwr.SetRequestHeader("Content-Type", "application/json");
            return uwr;
        }
    }
}

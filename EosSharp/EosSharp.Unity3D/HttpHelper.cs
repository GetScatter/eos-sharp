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
    public class HttpHandler : IHttpHandler
    {
        private static readonly HttpClient client = new HttpClient();
        private static Dictionary<string, object> ResponseCache { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Clear cached responses from requests called with Post/GetWithCacheAsync
        /// </summary>
        public void ClearResponseCache()
        {
            ResponseCache.Clear();
        }

        /// <summary>
        /// Make post request with data converted to json asynchronously
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">data sent in the body</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
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

        /// <summary>
        /// Make post request with data converted to json asynchronously
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">data sent in the body</param>
        /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        public Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, CancellationToken cancellationToken)
        {
            return PostJsonAsync<TResponseData>(url, data);
        }

        /// <summary>
        /// Make post request with data converted to json asynchronously.
        /// Response is cached based on input (url, data)
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">data sent in the body</param>
        /// <param name="reload">ignore cached value and make a request caching the result</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
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

            //TODO handle network errors
            if (uwr.isNetworkError)
            {
                Console.WriteLine("Error While Sending: " + uwr.error);
            }

            return JsonConvert.DeserializeObject<TResponseData>(uwr.downloadHandler.text);
        }

        /// <summary>
        /// Make post request with data converted to json asynchronously.
        /// Response is cached based on input (url, data)
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">data sent in the body</param>
        /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <param name="reload">ignore cached value and make a request caching the result</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        public Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, bool reload = false)
        {
            return PostJsonWithCacheAsync<TResponseData>(url, data, reload);
        }

        /// <summary>
        /// Make get request asynchronously.
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        public async Task<TResponseData> GetJsonAsync<TResponseData>(string url)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);

            await uwr.SendWebRequest();

            //TODO handle network errors
            if (uwr.isNetworkError)
            {
                Console.WriteLine("Error While Sending: " + uwr.error);
            }

            return JsonConvert.DeserializeObject<TResponseData>(uwr.downloadHandler.text);
        }

        /// <summary>
        /// Make get request asynchronously.
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        public Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken)
        {
            return GetJsonAsync<TResponseData>(url);
        }

        /// <summary>
        /// Generic http request sent asynchronously
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Stream with response</returns>
        public async Task<Stream> SendAsync(HttpRequestMessage request)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await BuildSendResponse(response);
        }

        /// <summary>
        /// Generic http request sent asynchronously
        /// </summary>
        /// <param name="request">request body</param>
        /// /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <returns>Stream with response</returns>
        public async Task<Stream> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return await BuildSendResponse(response);
        }

        /// <summary>
        /// Upsert response data in the data store
        /// </summary>
        /// <typeparam name="TResponseData">response data type</typeparam>
        /// <param name="hashKey">data key</param>
        /// <param name="responseData">response data</param>
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

        /// <summary>
        /// Calculate request unique hash key
        /// </summary>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">data sent in the body</param>
        /// <returns></returns>
        public string GetRequestHashKey(string url, object data)
        {
            var keyBytes = new List<byte[]>()
            {
                Encoding.UTF8.GetBytes(url),
                SerializationHelper.ObjectToByteArray(data)
            };
            return Encoding.Default.GetString(Sha256Manager.GetHash(SerializationHelper.Combine(keyBytes)));
        }

        /// <summary>
        /// Convert response to stream
        /// </summary>
        /// <param name="response">response object</param>
        /// <returns>Stream with response</returns>
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

        /// <summary>
        /// Convert stream to a string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }

        /// <summary>
        /// Build unity web request
        /// </summary>
        /// <param name="url">Url to send the request</param>
        /// <param name="verb">Http verb</param>
        /// <param name="data">data sent in the body</param>
        /// <returns></returns>
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

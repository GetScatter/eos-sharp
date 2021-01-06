﻿using Cryptography.ECDSA;
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

namespace EosSharp
{
    /// <summary>
    /// Http Handler implementation using System.Net.Http.HttpClient
    /// </summary>
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
            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync(request);
            return DeserializeJsonFromStream<TResponseData>(result);
        }

        /// <summary>
        /// Make post request with data converted to json asynchronously
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">data sent in the body</param>
        /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        public async Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync(request, cancellationToken);
            return DeserializeJsonFromStream<TResponseData>(result);
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

            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync(request);
            var responseData = DeserializeJsonFromStream<TResponseData>(result);
            UpdateResponseDataCache(hashKey, responseData);

            return responseData;
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
        public async Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, bool reload = false)
        {
            string hashKey = GetRequestHashKey(url, data);

            if (!reload)
            {
                object value;
                if (ResponseCache.TryGetValue(hashKey, out value))
                    return (TResponseData)value;
            }

            HttpRequestMessage request = BuildJsonRequestMessage(url, data);
            var result = await SendAsync(request, cancellationToken);
            var responseData = DeserializeJsonFromStream<TResponseData>(result);
            UpdateResponseDataCache(hashKey, responseData);

            return responseData;
        }

        /// <summary>
        /// Make get request asynchronously.
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        public async Task<TResponseData> GetJsonAsync<TResponseData>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await SendAsync(request);
            return DeserializeJsonFromStream<TResponseData>(result);
        }

        /// <summary>
        /// Make get request asynchronously.
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        public async Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await SendAsync(request, cancellationToken);
            return DeserializeJsonFromStream<TResponseData>(result);
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
            if(response == null)
            {
                throw new ApplicationException("Respionse is null!");    
            }

            if(response.Content == null)
            {
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = "Content is null"
                };
            }

            var stream = await response.Content.ReadAsStreamAsync();
            if (response.IsSuccessStatusCode) return stream;

            string content = null;
            try
            {
                content = await StreamToStringAsync(stream);
            }
            catch (System.Exception ex)
            {
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = $"Couldn't parse stream data. exception: {ex.ToString()}"
                };
            }

            if(string.IsNullOrEmpty(content)) 
            {
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = $"Couldn't parse stream data."
                };
            }

            try
            {
                ApiErrorException apiError = JsonConvert.DeserializeObject<ApiErrorException>(content);

                if(apiError == null) 
                {
                    throw new ApiException
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = $"Api error is null! Status code: {response.StatusCode}, content: {content}"
                    };
                }

                throw apiError;
            }
            catch(Exception ex)
            {
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = $"Couldn't deserialized api error, exception: {ex.ToString()}, content: {content}"
                };
            }
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
        /// Generic method to convert a stream with json data to any type
        /// </summary>
        /// <typeparam name="TData">type to convert</typeparam>
        /// <param name="stream">stream with json content</param>
        /// <returns>TData object</returns>
        public TData DeserializeJsonFromStream<TData>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(TData);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                return JsonSerializer.Create().Deserialize<TData>(jtr);
            }
        }

        /// <summary>
        /// Build json request data from object data
        /// </summary>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">object data</param>
        /// <returns>request message</returns>
        public HttpRequestMessage BuildJsonRequestMessage(string url, object data)
        {
            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
        }
    }
}

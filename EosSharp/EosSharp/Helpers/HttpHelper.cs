using EosSharp.Exceptions;
using Newtonsoft.Json;
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


        public static async Task<TResponseData> PostAsync<TResponseData>(string url, object data, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };

            return await SendAsync<TResponseData>(request, cancellationToken);
        }

        public static async Task<TResponseData> GetAsync<TResponseData>(string url, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await SendAsync<TResponseData>(request, cancellationToken);
        }

        public static async Task<TResponseData> SendAsync<TResponseData>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return DeserializeJsonFromStream<TResponseData>(stream);

            var content = await StreamToStringAsync(stream);
            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
        }

        public static TData DeserializeJsonFromStream<TData>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(TData);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<TData>(jtr);
                return searchResult;
            }
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

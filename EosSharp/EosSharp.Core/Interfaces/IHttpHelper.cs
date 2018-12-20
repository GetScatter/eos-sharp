using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EosSharp.Core.Interfaces
{
    public interface IHttpHelper
    {
        void ClearResponseCache();
        Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data);
        Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, CancellationToken cancellationToken);
        Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, bool reload = false);
        Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, bool reload = false);
        Task<TResponseData> GetJsonAsync<TResponseData>(string url);
        Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken);
        Task<Stream> SendAsync(HttpRequestMessage request);
        Task<Stream> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
        TData DeserializeJsonFromStream<TData>(Stream stream);
        HttpRequestMessage BuildJsonRequestMessage(string url, object data);
        void UpdateResponseDataCache<TResponseData>(string hashKey, TResponseData responseData);
        string GetRequestHashKey(string url, object data);
        Task<Stream> BuildSendResponse(HttpResponseMessage response);
        Task<string> StreamToStringAsync(Stream stream);
    }
}

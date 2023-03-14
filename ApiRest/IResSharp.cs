using ApiRest.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiRest
{
    public interface IResSharp
    {
        Apiendpoint CurrentEndpoint { get; set; }

        Task<T> RequestAsync<T>(Apiendpoint currentendpoint, object data, HttpMethod typepost, string Apptoken = "", Dictionary<string, string> headerAttributes = null, bool checkToken = true);
        Task<T> Upload<T>(byte[] image, string imagename, HttpClient client, string endpoint, string Apptoken = "");
    }
}
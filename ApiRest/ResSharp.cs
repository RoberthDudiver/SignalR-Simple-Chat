using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serilog;
using ApiRest.Models;
using Generic.Cryptography;

using Generic;
namespace ApiRest
{
    public class ResSharp : IResSharp
    {
        private IEncryption _encryption;
        public ResSharp( )
        {
            _encryption = new Encryption();
        }
        private string UrlBase
        {
            get { return CurrentEndpoint.apiBaseUrl; }
        }
        private string Rootbase
        {
            get { return $"{ CurrentEndpoint.apiversion}{CurrentEndpoint.endpointName}/{CurrentEndpoint.endpoint}"; }
        }

        public Apiendpoint CurrentEndpoint
        {
            get; set;
        }
        private static HttpClient client
        {
            get;
            set;
        }

        private Uri GetUri(string route)
        {
            if (!string.IsNullOrEmpty(UrlBase))
                return new Uri(new Uri(UrlBase), $"{Rootbase}{route}");

            return new Uri(route);
        }
        private HttpClient Cliente(HttpClientHandler handler, string token = null)
        {
            var authValue = new AuthenticationHeaderValue("Bearer", token);
            if (!string.IsNullOrEmpty(token))
            {
                client = new HttpClient(handler)
                {
                    DefaultRequestHeaders = { Authorization = authValue },

                };
            }
            else
            {
                client = new HttpClient()
                {

                };
            }
            return client;
        }


        public async Task<T> RequestAsync<T>(Apiendpoint currentendpoint, object data, HttpMethod typepost, string Apptoken = "", Dictionary<string, string> headerAttributes = null,  bool checkToken = true)
        {
            var ApptokenDecode = Apptoken;
                //Decode(Apptoken);
            if (headerAttributes!=null && headerAttributes.ContainsKey("USERTOKEN"))
            {
                headerAttributes["USERTOKEN"] = Decode(headerAttributes["USERTOKEN"]);
            }

            CurrentEndpoint = currentendpoint;
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var df = data;


            string tokenData = string.Empty;

            try
            {
                var route = CurrentEndpoint.Actualroute().realroute;


                //var r = await GetUri(route).PostJsonAsync(JsonConvert.SerializeObject(data)).ReceiveJson<T>();
                //return r;
                if (checkToken)
                {
                    return await SendRequestAsync<T>(route, content, typepost, new AuthenticationHeaderValue("Bearer", ApptokenDecode), headerAttributes);
                }
                else
                {
                    return await SendRequestAsync<T>(route, content, typepost, null, headerAttributes);

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string Decode(string Apptoken)
        {
            return _encryption.decrypt(_encryption.Base64Decode(Apptoken));
        }

        private async Task<T> SendRequestAsync<T>(string route, HttpContent content, HttpMethod httpMethod, AuthenticationHeaderValue authHeader, Dictionary<string, string> headerAttributes = null)
        {
            string contentRes = string.Empty;
            string requestContent = content?.ReadAsStringAsync()?.Result;

            try
            {
                using (var handler = new HttpClientHandler())
                {

                    using (Cliente(handler,""))
                    {

                        var request = new HttpRequestMessage();
                        if (httpMethod == HttpMethod.Get)
                        {
                            request = new HttpRequestMessage() { RequestUri = GetUri(route), Method = httpMethod };

                        }
                        else
                        {
                            request = new HttpRequestMessage() { RequestUri = GetUri(route), Method = httpMethod, Content = content };
                        }
                        using (request)
                        {
                            if (authHeader != null)
                            {
                                request.Headers.Authorization = authHeader;
                            }

                            request.Headers.Add("Accept", "*/*");

                            if (headerAttributes != null)
                            {
                                foreach (var attr in headerAttributes)
                                {
                                    request.Headers.Add(attr.Key, attr.Value);
                                }
                            }

                            using (var res = await client.SendAsync(request))
                            {

                                string json = await res.Content.ReadAsStringAsync();
                                contentRes = json;
                                if (json != null)
                                {
                                    return JsonConvert.DeserializeObject<T>(json);
                                }
                            }

                            content.Dispose();
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<T> Upload<T>(byte[] image, string imagename, HttpClient client, string endpoint, string Apptoken = "")
        {
            using (var handler = new HttpClientHandler())
            {
                using (Cliente(handler, Apptoken))
                {
                    using (var content =
                        new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                    {
                        content.Add(new StreamContent(new MemoryStream(image)), "file", imagename);

                        using (
                           var message =
                               await client.PostAsync(endpoint, content))
                        {

                            string json = await message.Content.ReadAsStringAsync();
                            if (json != null)
                            {
                                return JsonConvert.DeserializeObject<T>(json);
                            }

                            return default(T);
                        }
                    }
                }
            }
        }
    }
}

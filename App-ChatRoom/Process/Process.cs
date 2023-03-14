
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRest;
using System.Net.Http;
using App_Chat_Common.ViewModels;
using App_Chat_Common.Responses;

namespace App_ChatRoom.Process
{
    public class EndpointsProcess : IEndpointsProcess
    {
        private IConfigurationsEndpoints _configurations;
        private ResSharp _resSharp;

        public EndpointsProcess()
        {

            _resSharp = new ResSharp();
            _configurations = new ConfigurationsEndpoints();
        }

        public async Task<LoginResponse> Login(LoginViewModel request)
        {
            var data = new Dictionary<string, object>();
            var endpoint = _configurations.GetEndpoint("ApiChat");
            endpoint.routenameName = "login";
            var result = await _resSharp.RequestAsync<LoginResponse>(endpoint, request, HttpMethod.Post, null, null, false);

            return result;
        }


    }
}

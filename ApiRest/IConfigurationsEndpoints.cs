using ApiRest.Models;
using System.Collections.Generic;

namespace ApiRest
{
    public interface IConfigurationsEndpoints
    {
        Apiendpoint GetEndpoint(string Name);
        List<Apiendpoint> GetEndpoints();
    }
}
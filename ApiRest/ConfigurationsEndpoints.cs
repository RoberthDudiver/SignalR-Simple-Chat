
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRest.Models;
using Configuration;

namespace ApiRest
{
    public class ConfigurationsEndpoints : IConfigurationsEndpoints
    {
        public List<Apiendpoint> GetEndpoints() => AppConfiguration.GetSection<List<Apiendpoint>>(AppKeys.Endpoints);

        public Apiendpoint GetEndpoint(string Name) => GetEndpoints().SingleOrDefault(x => x.name == Name);

    }
}

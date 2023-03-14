using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRest.Models
{
    public class EndpointsConfigsModel
    {
        public Configurations configurations { get; set; }
    }


    public class Configurations
    {
        public string appKey { get; set; }
        public List<Apiendpoint> apiendpoints { get; set; }
    }

    public class Apiendpoint
    {
        public string name { get; set; }
 public string token { get; set; }
        public string endpointName { get; set; }
        public string routenameName { get; set; }

        public string apiBaseUrl { get; set; }
        public string apiversion { get; set; }
        public string endpoint { get; set; }
        public List<Route> routes { get; set; }

        List<string> _parameters;
        public List<string> parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new List<string>();
                }
                return _parameters;
            }
            set { _parameters = value; }
        }
        public Route Actualroute()
        {
            //if (!string.IsNullOrEmpty(name))
            //{
            //    endpointName = name;
            //}
            if (!string.IsNullOrEmpty(endpointName))
            {
                var rtn= this.routes.FirstOrDefault(x => x.route == routenameName);
                rtn.parameters = parameters;
                return rtn;
            }

            return null;
        }
    }

    public class Route
    {
        public string query { get; set; }
       
        public string realroute {
            get
            {
                if (this != null && !string.IsNullOrEmpty(query) && parameters.Count != 0)
                {
                    return  Fullquery();
                }
                else
                {
                    return route;

                }
            }
        }
        public string route { get; set; }
        List<string> _parameters;
        public List<string> parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new List<string>();
                }
                return _parameters;
            }
            set { _parameters = value; }
        }
        public string Fullquery()
        {

            StringBuilder text = new StringBuilder();
            var full = text.AppendFormat($"{route}{query}", parameters.ToArray());
            return full.ToString();
        }
    }
}

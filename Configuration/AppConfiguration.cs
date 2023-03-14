
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections;
using System;
using Serilog;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;
using Generic;
using Newtonsoft.Json.Linq;

namespace Configuration
{

    public static class AppConfiguration
    {

        static JObject Configuration { get; set; }

        static AppConfiguration()
        {
            NewConfig();
        }


        static JObject loadJson(string jsonPath)
        {
            using (StreamReader reader = new StreamReader(jsonPath))
            {
                string json = reader.ReadToEnd();
                //   var objectSetings = JsonConvert.DeserializeObject<Dictionary<string,object>>(json);
                var objectSetings = JObject.Parse(json);

                return objectSetings;
            }


        }
        private static void NewConfig()
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AppConfiguration)).Locati‌​on);
            var file = $"{currentDirectory}/Configuration/appsettings.json";

            Configuration = loadJson(file);


        }

        public static T GetConfiguration<T>(string key)
        {
            var originalValue = Configuration.SelectToken(key.Replace(":", ".")).ToString();
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var result = (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, originalValue);
            return result;
        }

        public static T GetSection<T>(string key) where T : class, new()
        {
            var entity = new T();

            return Configuration.SelectToken(key.Replace(":", ".")).ToConvertObjects<T>();
        }

    }
}
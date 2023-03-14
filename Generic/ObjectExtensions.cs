using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Generic
{
    public static class ObjectExtensions
    {
        private static object ConvertIfNonAvroType(object obj)
        {
            return obj is DateTime ? ((DateTime)obj).ToString("o") : obj;
        }
        public static JObject GetJsonObjetc(this Dictionary<string, object> document)
        {
            return JObject.Parse(JsonConvert.SerializeObject(document));
        }

        public static bool IfBoolean(this object defaultValue)
        {
            try
            {
                var bolea = ToObject<bool>(defaultValue.ToString());
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static T MapXMLToObject<T>(this XmlDocument doc, string tableName)
        {
            var xml = doc.GetElementsByTagName(tableName)[0].OuterXml;
            DataSet acompaniantesDataSet = new DataSet();
            acompaniantesDataSet.ReadXml(new XmlTextReader(new StringReader(xml)));
            var table = acompaniantesDataSet.Tables["item"];
            var json = JsonConvert.SerializeObject(table);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static JObject GetJsonObjetc(this object document)
        {
            return JObject.Parse(JsonConvert.SerializeObject(document));
        }
        public static T ToObject<T>(this object obj)
        {
            Type TypeOfClass = typeof(T);
            return (T)Convert.ChangeType(obj, TypeOfClass);
        }
     public static Dictionary<string,object> ToDictionary(this object obj)
        {
          var dic=  obj.GetType()
      .GetProperties(BindingFlags.Instance | BindingFlags.Public)
           .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(obj, null));

            return dic;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
  
  public static string GetFilePath(this string path)
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ObjectExtensions)).Locati‌​on);

            var json = Path.Combine(currentDirectory, string.Format(@"{0}", path));
            return json;
        }
        public static string GetFile(this string path)
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ObjectExtensions)).Locati‌​on);

            var json = File.ReadAllText(Path.Combine(currentDirectory, string.Format(@"{0}", path)));
            return json;
        }
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string ProcessStringCharacter(this string str)
        {
            return (RemoveDiacritics(str.Replace(",", " ").Replace("-", " ").Replace("_", " ").Replace(";", " ").Replace("&", " ")));
        }
        public static T ToConvertObjects<T>(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T ToConvertObjects<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static XmlDocument GetEntityXml<T>(List<T> Lista)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XPathNavigator nav = xmlDoc.CreateNavigator();
            using (XmlWriter writer = nav.AppendChild())
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("Root"));

                ser.Serialize(writer, Lista);
            }
            return xmlDoc;
        }
        public static DataSet ToDataset(this XmlDocument XML)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlTextReader(new StringReader(XML.OuterXml)));
            return ds;
        }



        public static string GetSerializer<T>(List<T> Lista)
        {
            string RequestContentXML = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                settings.IndentChars = "\t";
                settings.NewLineChars = Environment.NewLine;
                settings.ConformanceLevel = ConformanceLevel.Document;
                using (XmlWriter writer = XmlTextWriter.Create(ms, settings))
                {
                    serializer.Serialize(writer, Lista);
                }
                RequestContentXML = Encoding.UTF8.GetString(ms.ToArray());
            }

            return RequestContentXML;
        }

        public static DataSet ToDataset(this XmlNode XML)
        {

            DataSet ds = new DataSet();
            ds.ReadXml(new XmlTextReader(new StringReader(XML.OuterXml)));


            return ds;
        }

        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        public static List<T> ToLista<T>(this DataTable dt)
        {
            List<T> Lista = new System.Collections.Generic.List<T>();
            Type TypeOfClass = typeof(T);
            PropertyInfo[] pClass = TypeOfClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow datarow in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(TypeOfClass);
                foreach (PropertyInfo pc in pClass)
                {
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName == pc.Name);
                        if (d != null)
                            // pc.SetValue(cn, datarow[pc.Name], null);
                            pc.SetValue(cn, Convert.ChangeType(datarow[pc.Name], pc.PropertyType), null);
                    }
                    catch
                    {
                    }
                }
                Lista.Add(cn);
            }
            return Lista;
        }

        public static string ToStringJson(this object obj)
        {

            return JsonConvert.SerializeObject(obj);


        }

    }
}

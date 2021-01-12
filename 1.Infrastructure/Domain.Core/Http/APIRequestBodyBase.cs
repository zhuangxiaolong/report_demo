using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Domain.Core.Cache;
using Newtonsoft.Json;

namespace Domain.Core.Http
{
    public abstract class APIRequestBodyBase : IValueObject
    {

    }

    public abstract class APIRequestBodyBase<T> : APIRequestBodyBase
        where T : APIRequestBodyBase<T>
    {
        public APIRequestBodyBase()
        {
            ContentType = "application/x-www-form-urlencoded";
            Charset = "utf8";
            TimeOut = 100000;
            Method = "Post";
            UseSecurity = false;
            SecurityProtocol = SecurityProtocolType.Ssl3;
        }

        [JsonIgnore]
        public string Url { get; set; }

        [JsonIgnore]
        public string ContentType { get; set; }

        [JsonIgnore]
        public string Charset { get; set; }

        [JsonIgnore]
        public int TimeOut { get; set; }

        [JsonIgnore]
        public string Method { get; set; }

        [JsonIgnore]
        public SecurityProtocolType SecurityProtocol { get; set; }

        [JsonIgnore]
        public bool UseSecurity { get; set; }

        [JsonIgnore]
        public IDictionary<string, string> Headers { get; set; }

        public override string ToString()
        {
            if (Method.ToLower() == "post" &&
                ContentType == "application/json")
                return getJsonData();
            return getFormData();
        }

        public string GetCacheKey()
        {
            return string.Format("Url:{0}&Data:{1}", Url, getJsonData());
        }


        private string getJsonData()
        {
            return JsonConvert.SerializeObject(this);
        }

        private string getFormData()
        {
            var type = typeof(T);
            var keyValue = type.GetProperties()
                               .Where(e => !e.IsDefined(typeof(JsonIgnoreAttribute), true))
                               .Select(e => new Tuple<string, string>(e.Name, e.GetValue(this)?.ToString()))
                               .Where(e => e.Item2 != null)
                               .Select(e => string.Format("{0}={1}", e.Item1, e.Item2))
                               .ToArray();
            var param = string.Join("&", keyValue);
            return param;
        }
    }
}
using System;
using Domain.Core.Cache;
using Newtonsoft.Json;

namespace Domain.Core.Http
{
    public abstract class APIResponseBodyBase : CacheableObjectBase
    {

    }
    public abstract class APIResponseBodyBase<T> : APIResponseBodyBase
        where T : APIResponseBodyBase<T>
    {
        public static T CreateResponse()
        {
            return Activator.CreateInstance<T>();
        }
        public static T Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
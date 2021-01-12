using System;
using Newtonsoft.Json;

namespace Domain.Core.Cache
{
    public abstract class CacheableObjectBase : ICacheableObject
    {
        [JsonIgnore]
        public string CacheId { get; set; }

        [JsonIgnore]
        public TimeSpan? ExpireTime
        {
            get; set;
        }
    }
}
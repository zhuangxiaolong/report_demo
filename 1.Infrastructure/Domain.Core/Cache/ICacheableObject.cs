using System;

namespace Domain.Core.Cache
{
    public interface ICacheableObject : IValueObject
    {
        string CacheId { get; set; }
        TimeSpan? ExpireTime { get; set; }
    }
}
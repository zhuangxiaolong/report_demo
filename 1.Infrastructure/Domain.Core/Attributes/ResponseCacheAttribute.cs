using System;

namespace Domain.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ResponseCacheAttribute : Attribute
    {
        public ResponseCacheAttribute()
        {
            ExpiredTime = TimeSpan.FromSeconds(30);
        }

        public ResponseCacheAttribute(int expired)
        {
            ExpiredTime = TimeSpan.FromSeconds(expired);
        }

        public TimeSpan ExpiredTime { get; }
    }
}
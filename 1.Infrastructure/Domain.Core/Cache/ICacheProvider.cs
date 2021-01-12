using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Domain.Core.Cache
{
    public interface ICacheProvider : IDisposable, IDependency
    {
        void SetDb(IDatabase db);

        void AddCache<T>(T obj) where T : ICacheableObject;

        void AddCache(object obj);

        Task AddCacheAsync<T>(T obj) where T : ICacheableObject;

        void RemoveCache<T>(T obj) where T : ICacheableObject;

        Task RemoveCacheAsync<T>(T obj) where T : ICacheableObject;

        void UpdateCache<T>(T obj) where T : ICacheableObject;

        Task UpdateCacheAsync<T>(T obj) where T : ICacheableObject;

        T GetCache<T>(string key) where T : ICacheableObject;

        object Get(Type type, string key);

        Task<T> GetCacheAsync<T>(string key) where T : ICacheableObject;
    }
}
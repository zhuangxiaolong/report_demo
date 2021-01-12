using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Domain.Core.Cache
{
     public class RedisCacheProvider : ICacheProvider
    {
        private IDatabase _db;

        public void SetDb(IDatabase db)
        {
            _db = db;
        }

        public void AddCache<T>(T obj)
            where T : ICacheableObject
        {
            var val = JsonConvert.SerializeObject(obj);
            var result = _db.StringSet(obj.CacheId, val);
            if (result)
            {
                _db.KeyExpire(obj.CacheId, obj.ExpireTime);
            }
        }

        public async Task AddCacheAsync<T>(T obj)
            where T : ICacheableObject
        {
            var val = JsonConvert.SerializeObject(obj);
            var result = await _db.StringSetAsync(obj.CacheId, val);
            if (result)
            {
                await _db.KeyExpireAsync(obj.CacheId, obj.ExpireTime);
            }
        }

        public void RemoveCache<T>(T obj)
            where T : ICacheableObject
        {
            _db.KeyDelete(obj.CacheId);
        }

        public async Task RemoveCacheAsync<T>(T obj)
            where T : ICacheableObject
        {
            await _db.KeyDeleteAsync(obj.CacheId);
        }

        public void UpdateCache<T>(T obj)
            where T : ICacheableObject
        {
            var value = JsonConvert.SerializeObject(obj);
            _db.StringSet(obj.CacheId, value,obj.ExpireTime);
        }

        public async Task UpdateCacheAsync<T>(T obj)
            where T : ICacheableObject
        {
            var value = JsonConvert.SerializeObject(obj);
            await _db.StringSetAsync(obj.CacheId, value);
        }

        public T GetCache<T>(string key) where T : ICacheableObject
        {
            var value = _db.StringGet(key);
            if (!value.HasValue) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> GetCacheAsync<T>(string key) where T : ICacheableObject
        {
            var value = await _db.StringGetAsync(key);
            if (!value.HasValue) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public void Dispose()
        {
            _db = null;
        }

        public void AddCache(object obj)
        {
            var cacheObj = obj as ICacheableObject;
            var val = JsonConvert.SerializeObject(obj);
            var result = _db.StringSet(cacheObj.CacheId, val);
            if (result)
            {
                _db.KeyExpire(cacheObj.CacheId, cacheObj.ExpireTime);
            }
        }

        public object Get(Type type, string key)
        {
            var value = _db.StringGet(key);
            if (!value.HasValue) return null;
            return JsonConvert.DeserializeObject(value, type);
        }
    }
}
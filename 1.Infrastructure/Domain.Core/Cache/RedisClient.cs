using System;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Domain.Core.Cache
{
    public class RedisClient : ICacheClient
    {
        private static ConnectionMultiplexer _redis;
        private static string _connStr;
        private int _dbIndex;
        private object _asyncState;
        private IDatabase _db;
        private ICachePkProvider _pkProvider;
        public ICacheProvider _cacheProvider { get; }
        public RedisClient(string connStr,
            ICacheProvider provider,
            ICachePkProvider pkProvider,
            int dbIndex = -1,
            object asyncState = null)
        {
            _connStr = connStr;
            _dbIndex = dbIndex;
            _asyncState = asyncState;
            _cacheProvider = provider;
            _pkProvider = pkProvider;
        }
        private static Lazy<ConnectionMultiplexer>
            lazyConnection = new Lazy<ConnectionMultiplexer>
            (() =>
            {
                var configuration = ConfigurationOptions.Parse(_connStr);
                configuration.AbortOnConnectFail = false;
                var multiplexer = ConnectionMultiplexer.Connect(configuration);
                return multiplexer;
            });
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public void Initialize()
        {
            _db = Connection.GetDatabase(_dbIndex, _asyncState);
            _cacheProvider.SetDb(_db);
        }


        public void Add<T>(T obj) where T : ICacheableObject
        {
            var cacheId = string.Format("class:{0},key:{1}", typeof(T).FullName, obj.CacheId);
            obj.CacheId = _pkProvider.GenerateKey(cacheId);
            _cacheProvider.AddCache(obj);
        }

        public async Task AddAsync<T>(T obj) where T : ICacheableObject
        {
            var cacheId = string.Format("class:{0},key:{1}", typeof(T).FullName, obj.CacheId);
            obj.CacheId = _pkProvider.GenerateKey(cacheId);
            await _cacheProvider.AddCacheAsync(obj);
        }

        public T Get<T>(string key) where T : ICacheableObject
        {
            var cacheId = string.Format("class:{0},key:{1}", typeof(T).FullName, key);
            key = _pkProvider.GenerateKey(cacheId);
            return _cacheProvider.GetCache<T>(key);
        }

        public async Task<T> GetAsynce<T>(string key) where T : ICacheableObject
        {
            var cacheId = string.Format("class:{0},key:{1}", typeof(T).FullName, key);
            key = _pkProvider.GenerateKey(cacheId);
            return await _cacheProvider.GetCacheAsync<T>(key);
        }


        public void Remove<T>(T obj) where T : ICacheableObject
        {
            var cacheId = string.Format("class:{0},key:{1}", typeof(T).FullName, obj.CacheId);
            obj.CacheId = _pkProvider.GenerateKey(cacheId);
            _cacheProvider.RemoveCache(obj);
        }

        public async Task RemoveAsync<T>(T obj) where T : ICacheableObject
        {
            var cacheId = string.Format("class:{0},key:{1}", typeof(T).FullName, obj.CacheId);
            obj.CacheId = _pkProvider.GenerateKey(cacheId);
            await _cacheProvider.RemoveCacheAsync(obj);
        }

        public string GenerateKey(object id)
        {
            return _pkProvider.GenerateKey(id);
        }

        public void Dispose()
        {
            _db = null;
        }

        public void Add(object obj)
        {
            var type = obj.GetType();
            if (!type.GetInterfaces().Any(e => e == typeof(ICacheableObject)))
                return;
            var cacheObj = obj as ICacheableObject;
            var cacheId = string.Format("class:{0},key:{1}", type.FullName, cacheObj.CacheId);
            cacheObj.CacheId = _pkProvider.GenerateKey(cacheObj.CacheId);
            type.GetProperty("CacheId").SetValue(obj, cacheObj.CacheId);
            _cacheProvider.AddCache(obj);
        }

        public object Get(Type type, string key)
        {
            var cacheId = string.Format("class:{0},key:{1}", type.FullName, key);
            key = _pkProvider.GenerateKey(key);
            return _cacheProvider.Get(type, key);
        }

        public void Update<T>(T obj) where T : ICacheableObject
        {
            var cacheId = string.Format("class:{0},key:{1}", typeof(T).FullName, obj.CacheId);
            obj.CacheId = _pkProvider.GenerateKey(cacheId);
            _cacheProvider.UpdateCache(obj);
        }

        public async Task UpdateAsync<T>(T obj) where T : ICacheableObject
        {
            var cacheId = string.Format("class:{0},key:{1}", typeof(T).FullName, obj.CacheId);
            obj.CacheId = _pkProvider.GenerateKey(cacheId);
            await _cacheProvider.UpdateCacheAsync(obj);
        }
    }
}
using System;
using System.Threading.Tasks;

namespace Domain.Core.Cache
{
    public interface ICacheClient : IInitializable, IDisposable, IDependency
    {
        void Add<T>(T obj) where T : ICacheableObject;
        void Add(object obj);
        Task AddAsync<T>(T obj) where T : ICacheableObject;
        void Remove<T>(T obj) where T : ICacheableObject;
        Task RemoveAsync<T>(T obj) where T : ICacheableObject;
        void Update<T>(T obj) where T : ICacheableObject;
        Task UpdateAsync<T>(T obj) where T : ICacheableObject;
        T Get<T>(string key) where T : ICacheableObject;
        Task<T> GetAsynce<T>(string key) where T : ICacheableObject;
        object Get(Type type, string key);
        string GenerateKey(object id);
    }
}
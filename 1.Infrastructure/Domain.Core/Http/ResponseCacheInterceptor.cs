using System;
using System.Linq;
using Castle.DynamicProxy;
using Domain.Core.Attributes;
using Domain.Core.Cache;

namespace Domain.Core.Http
{
    public class ResponseCacheInterceptor : IInterceptor
    {
        private const string CACHEIDMETHOD = "GetCacheKey";
        private const string EXPIREDTIME = "ExpireTime";
        private const string CACHEID = "CacheId";
        private readonly Lazy<ICacheClient> _cacheClient;
        public ResponseCacheInterceptor(Lazy<ICacheClient> cacheClient)
        {
            _cacheClient = cacheClient;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            return;
            if (!invocation.Method.IsDefined(typeof(ResponseCacheAttribute), true))
            {
                invocation.Proceed();
                return;
            }
            var parameters = invocation.Arguments
                                       .Where(e => e.GetType()
                                                    .IsSubclassOf(typeof(APIRequestBodyBase)));
            if (parameters.Count() == 0)
            {
                invocation.Proceed();
                return;
            }
            if (!invocation.Method.ReturnType.IsSubclassOf(typeof(APIResponseBodyBase)))
            {
                invocation.Proceed();
                return;
            }
            var request = parameters.FirstOrDefault();
            var key = request.GetType()
                             .GetMethod(CACHEIDMETHOD)
                             .Invoke(request, null);
            var response = _cacheClient.Value.Get(invocation.Method.ReturnType, key.ToString());
            if (response != null)
            {
                invocation.ReturnValue = response;
                return;
            }
            invocation.Proceed();
            var cacheAttr = invocation.Method.GetCustomAttributes(typeof(ResponseCacheAttribute), true)[0] as ResponseCacheAttribute;
            response = invocation.ReturnValue;
            var returnType = invocation.Method.ReturnType;
            returnType.GetProperty(EXPIREDTIME).SetValue(response, cacheAttr.ExpiredTime);
            returnType.GetProperty(CACHEID).SetValue(response, key);
            _cacheClient.Value.Add(response);
        }
    }
}
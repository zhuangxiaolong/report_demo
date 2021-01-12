using System;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Core.APIClient;
using Domain.Core.Handlers;
using Domain.Core.Log;
using Polly.Timeout;

namespace Domain.Core.Http
{
    public abstract class HttpRequestClientBase : LogBase
    {
        private RestfulClient _restfulClient;
        private ICircuitBreakingHandler<string> _handler;
        protected HttpRequestClientBase(string logName)
            : base(logName)
        {
            _restfulClient = new RestfulClient();
        }

        protected HttpRequestClientBase(string logName,
                                        ICircuitBreakingHandler<string> handler)
            : this(logName)
        {
            _handler = handler;
            _handler.Handle<HttpRequestException>()
                    .Handle<TimeoutException>()
                    .Handle<TimeoutRejectedException>()
                    .Build();
        }

        protected async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
          where TRequest : APIRequestBodyBase<TRequest>
          where TResponse : APIResponseBodyBase<TResponse>
        {
            if (request.Method.ToUpper() == "GET") return await GetAsync<TRequest, TResponse>(request);
            return await PostAsync<TRequest, TResponse>(request);
        }

        protected void Download(string url, string fileName)
        {
            _handler.Handle(() =>
            {
                _restfulClient = _restfulClient ?? new RestfulClient();
                return _restfulClient.Download(url, fileName);
            });
        }

        protected TResponse Request<TRequest, TResponse>(TRequest request)
          where TRequest : APIRequestBodyBase<TRequest>
          where TResponse : APIResponseBodyBase<TResponse>
        {
            if (request.Method.ToUpper() == "GET") return Get<TRequest, TResponse>(request);
            return Post<TRequest, TResponse>(request);
        }

        protected async Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
          where TRequest : APIRequestBodyBase<TRequest>
          where TResponse : APIResponseBodyBase<TResponse>
        {
            var result = await _handler.HandleAsync(() =>
            {
                _restfulClient = _restfulClient ?? new RestfulClient();
                return _restfulClient.DoGetAsync(request.Url,
                                                       request.ToString(),
                                                       request.Headers);
            });
            var response = APIResponseBodyBase<TResponse>.Deserialize(result);
            return response;
        }

        protected TResponse Get<TRequest, TResponse>(TRequest request)
                  where TRequest : APIRequestBodyBase<TRequest>
                  where TResponse : APIResponseBodyBase<TResponse>
        {
            var result = _handler.Handle(() =>
            {
                _restfulClient = _restfulClient ?? new RestfulClient();
                return _restfulClient.DoGet(request.Url,
                                                  request.ToString(),
                                                  request.Headers);
            });
            var response = APIResponseBodyBase<TResponse>.Deserialize(result);
            return response;
        }

        protected string Post<TRequest>(TRequest request)
            where TRequest : APIRequestBodyBase<TRequest>
        {
            var result = _handler.Handle(() =>
            {
                return _restfulClient.DoPost(request.Url,
                                             request.ContentType,
                                             request.Headers,
                                             request.ToString(),
                                             request.SecurityProtocol,
                                             request.TimeOut,
                                             request.UseSecurity);
            });
            return result;
        }

        protected TResponse Post<TRequest, TResponse>(TRequest request)
                  where TRequest : APIRequestBodyBase<TRequest>
                  where TResponse : APIResponseBodyBase<TResponse>
        {
            _log.DebugFormat("Request:{0}", request.ToString());
            var result = _handler.Handle(() =>
            {
                _restfulClient = _restfulClient ?? new RestfulClient();
                return _restfulClient.DoPost(request.Url,
                                                   request.ContentType,
                                                   request.Headers,
                                                   request.ToString(),
                                                   request.SecurityProtocol,
                                                   request.TimeOut,
                                                   request.UseSecurity);
            });
            _log.DebugFormat("Response:{0}", result);
            var response = APIResponseBodyBase<TResponse>.Deserialize(result);
            return response;
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
                  where TRequest : APIRequestBodyBase<TRequest>
                  where TResponse : APIResponseBodyBase<TResponse>
        {
            var result = await _handler.HandleAsync(() =>
            {
                _restfulClient = _restfulClient ?? new RestfulClient();
                return _restfulClient.PostAsync(request.Url,
                                      request.ContentType,
                                      request.Headers,
                                      request.ToString(),
                                      request.SecurityProtocol,
                                      request.TimeOut,
                                      request.UseSecurity);
            });
            var response = APIResponseBodyBase<TResponse>.Deserialize(result);
            return response;
        }

        public void Dispose()
        {
            _restfulClient = null;
        }
    }
}
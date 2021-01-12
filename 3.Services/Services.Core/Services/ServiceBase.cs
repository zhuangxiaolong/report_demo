using Domain.Core.Handlers;
using Domain.Core.Http;

namespace Services.Core.Services
{
    public abstract class ServiceBase : HttpRequestClientBase, IService
    {

        protected ServiceBase(string logName)
            : base(string.Format("Service:{0}", logName))
        {

        }

        protected ServiceBase(string logName,
            ICircuitBreakingHandler<string> handler)
            : base(logName, handler)
        {

        }
    }
}
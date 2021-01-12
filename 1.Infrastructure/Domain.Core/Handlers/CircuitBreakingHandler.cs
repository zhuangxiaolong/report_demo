using System;
using System.Collections.Generic;
using System.Text;
using Polly;
using Polly.Timeout;
using System;
using System.Threading.Tasks;
using Domain.Core.Log;

namespace Domain.Core.Handlers
{
   public abstract class CircuitBreakingHandler<T> : LogBase, ICircuitBreakingHandler<T>
    {
        private Policy _circuitBreakerPolicyAsync;
        private Policy _circuitBreakerPolicy;
        private TimeoutPolicy _timeoutBreakerPolicy;
        private int _exceptionsAllowedBeforeBreaking;
        private TimeSpan _durationOfBreak;
        private PolicyBuilder _policyBuilder;
        public CircuitBreakingHandler(int exceptionsAllowedBeforeBreaking,
                                      TimeSpan durationOfBreak,
                                      TimeSpan timeoutValue,
                                      TimeoutStrategy timeoutStrategy = TimeoutStrategy.Pessimistic)
            : base(string.Format("Handler:{0}", nameof(T)))
        {
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _durationOfBreak = durationOfBreak;
            _timeoutBreakerPolicy = Policy.Timeout(timeoutValue, timeoutStrategy);
        }

        public Policy Build()
        {
            _circuitBreakerPolicyAsync = _policyBuilder.CircuitBreakerAsync(_exceptionsAllowedBeforeBreaking,
                                                                       _durationOfBreak,
                                                                       (ex, breakDelay) =>
                                                                       {
                                                                           _log.Error("发生异常：", ex);
                                                                       },
                                                                       () =>
                                                                       {
                                                                           _log.Debug("正在重新启动任务");
                                                                       });
            _circuitBreakerPolicy = _policyBuilder.CircuitBreaker(_exceptionsAllowedBeforeBreaking,
                                                                  _durationOfBreak,
                                                                  (ex, breakDelay) =>
                                                                  {
                                                                      _log.Error("发生异常：", ex);
                                                                  },
                                                                  () =>
                                                                  {
                                                                      _log.Debug("正在重新启动任务");
                                                                  });
            return _circuitBreakerPolicyAsync;
        }

        public ICircuitBreakingHandler<T> Handle<TException>() where TException : Exception
        {
            if (_policyBuilder != null) _policyBuilder.Or<TException>();
            else _policyBuilder = Policy.Handle<TException>();
            return this;
        }

        public Task<T> HandleAsync(Func<Task<T>> funcAsync)
        {
            //var task = Policy.WrapAsync(_circuitBreakerPolicyAsync, _timeoutBreakerPolicy)
            //                    .ExecuteAsync(funcAsync);
            var task = funcAsync.Invoke();
            return task;
        }

        public T Handle(Func<T> func)
        {
            //var result = Policy.Wrap(_circuitBreakerPolicy, _timeoutBreakerPolicy)
            //                   .Execute(func);
            var result = func.Invoke();
            return result;
        }
    }
}
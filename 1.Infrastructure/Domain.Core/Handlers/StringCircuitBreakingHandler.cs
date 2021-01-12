using System;
using Polly.Timeout;

namespace Domain.Core.Handlers
{
    public class StringCircuitBreakingHandler : CircuitBreakingHandler<string>
    {
        public StringCircuitBreakingHandler(int exceptionsAllowedBeforeBreaking,
            TimeSpan durationOfBreak,
            TimeSpan timeoutValue,
            TimeoutStrategy timeoutStrategy = TimeoutStrategy.Pessimistic)
            : base(exceptionsAllowedBeforeBreaking, durationOfBreak, timeoutValue, timeoutStrategy)
        {
        }
    }
}
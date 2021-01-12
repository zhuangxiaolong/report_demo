using System;
using Polly;

namespace Domain.Core.Handlers
{
    public interface ICircuitBreakingHandler<T> : IHandler<T>
    {
        ICircuitBreakingHandler<T> Handle<TException>() where TException : Exception;
        Policy Build();
    }
}
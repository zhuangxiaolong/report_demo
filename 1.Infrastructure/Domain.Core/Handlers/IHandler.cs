using System;
using System.Threading.Tasks;

namespace Domain.Core.Handlers
{
    public interface IHandler : IDependency
    {

    }
    public interface IHandler<T> : IHandler
    {
        Task<T> HandleAsync(Func<Task<T>> funcAsync);
        T Handle(Func<T> func);
    }

    public interface IHandler<T1, T2> : IHandler
    {
        T2 Handle(Func<T1, T2> func);
    }

    public interface IHandler<T1, T2, T3> : IHandler
    {
        T3 Handle(Func<T1, T2, T3> func);
    }
}
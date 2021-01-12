using Domain.Core;
using System;

namespace Domain.Data
{
    public interface IUnitOfWork : System.IDisposable, IDependency
    {
        bool AutoCommit { get; set; }
        // DbContextTransaction UseTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted);
        //long GenerateEventNo();
        //void Rollback();
        int Commit();
        int ExecuteSqlCommand(string sql, params object[] args);
        // DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] args);
        // string GenerateNcId(string idHeader, int v);
    }
}
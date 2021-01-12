using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domain.Data
{
    public class DbContextBase : DbContext, IUnitOfWork
    {
        public DbContextBase(string connStr)
            : base()
        {
            AutoCommit = true;
        }

        private IDbContextTransaction _transaction;

        public bool AutoCommit { get; set; }
        public void Rollback()
        {
            if (_transaction != null)
                _transaction.Rollback();
        }

        public int Commit()
        {
            int result = 0;
            try
            {
                result = SaveChanges();
                if (_transaction != null)
                    _transaction.Commit();
            }
            catch (Exception ex)
            {
                //if (ObjectStateManager.GetObjectStateEntry(entity).State == EntityState.Deleted || ObjectStateManager.GetObjectStateEntry(entity).State == EntityState.Modified)
                //    this.Refresh(RefreshMode.StoreWins, entity);
                //else if (ObjectStateManager.GetObjectStateEntry(entity).State == EntityState.Added)
                //    Detach(entity);
                //AcceptAllChanges();
                //_transaction.Commit();
            }
            return result;
        }

        public IDbContextTransaction UseTransaction(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
        {
            AutoCommit = false;
            _transaction = Database.BeginTransaction(isolationLevel);
            return _transaction;
        }

        public int ExecuteSqlCommand(string sql, params object[] args)
        {
            return Database.ExecuteSqlCommand(sql, args);
        }
    }
}
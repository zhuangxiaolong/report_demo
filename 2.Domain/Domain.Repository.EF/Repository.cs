using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Core;
using Domain.Core.Attributes;
using Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repository.EF
{
    public abstract class Repository<T, Pk> : IRepository<T, Pk>
        where T : class, IEntity<Pk>
    {
        private IUnitOfWork _unitWork;
        private OpenApiContext _db;
        private DbSet<T> _dbSet;

        protected Repository(IUnitOfWork unitWork)
        {
            _unitWork = unitWork;
            _db = _unitWork as OpenApiContext;
            _dbSet = _db.Set<T>();
        }

        public virtual int Add(T entity)
        {
            _db.Entry(entity).State = EntityState.Added;
            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        public int AddRange(ICollection<T> entities)
        {
            _dbSet.AddRange(entities);
            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        public virtual void Dispose()
        {
            _unitWork.Dispose();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> findexpression)
        {
            return _dbSet.Where(findexpression).AsNoTracking();
        }

        public ICollection<T> FindAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T FindSingle(Pk primary)
        {
            return _dbSet.Find(primary);
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsNoTracking()
                .AsQueryable();
        }

        public int Remove(Pk primary)
        {
            var entity = _dbSet.Find(primary);
            _dbSet.Remove(entity);
            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        public int RemoveRange(Expression<Func<T, bool>> expression)
        {
            var list = Find(expression).ToList();
            foreach (var entity in list)
            {
                _db.Entry(entity).State = EntityState.Deleted;
            }

            // _dbSet.RemoveRange(list);
            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int RemoveRange(ICollection<T> entities)
        {
            _dbSet.RemoveRange(entities);
            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        public int Update(T entity)
        {
            var des = _db.Set<T>().Find(entity.Id);
            ValueCopy(entity, des, "Id");
            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        public int UpdateRange(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                var des = _db.Set<T>().Find(entity.Id);
                ValueCopy(entity, des, "Id");
            }

            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        public int Modify(T entity)
        {
            _dbSet.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        public int ModifyRange(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
            }

            if (_unitWork.AutoCommit == false) return 0;
            return _db.SaveChanges();
        }

        /// <summary>
        /// 属性复制
        /// </summary>
        /// <param name="source">源实例</param>
        /// <param name="destination">目标实例</param>
        /// <param name="ignore">需要忽略的属性名称</param>
        public void ValueCopy(T source, T destination, params string[] ignore)
        {
            var type = typeof(T);
            type.GetProperties()
                .Where(e => (ignore == null
                             || !ignore.Contains(e.Name))
                            && e.PropertyType != typeof(ICollection<>)
                            && !e.GetCustomAttributes(typeof(DbFKAttribute), true).Any())
                .ToList()
                .ForEach(e =>
                {
                    var value = e.GetValue(source);
                    e.SetValue(destination, value);
                });
        }
    }
}
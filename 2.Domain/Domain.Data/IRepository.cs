using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Core;

namespace Domain.Data
{
    public interface IRepository : System.IDisposable, IDependency
    {

    }

    public interface IRepository<T, Pk> : IRepository
        where T : IEntity<Pk>
    {
        /// <summary>
        /// 添加单个实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int Add(T entity);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        int AddRange(ICollection<T> entities);

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="primary">主键</param>
        int Remove(Pk primary);

        int RemoveRange(ICollection<T> entities);
        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        int RemoveRange(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        int Update(T entity);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        int UpdateRange(ICollection<T> entities);

        int Modify(T entity);

        int ModifyRange(ICollection<T> entities);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="primary">主键</param>
        /// <returns>实体对象</returns>
        T FindSingle(Pk primary);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体集合</returns>
        ICollection<T> FindAll();

        /// <summary>
        /// 根据条件查询实体集合
        /// </summary>
        /// <param name="findexpression">查询条件Lambda表达式</param>
        /// <returns>实体集合</returns>
        IQueryable<T> Find(Expression<Func<T, bool>> findexpression);

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <returns>查询对象</returns>
        IQueryable<T> GetQueryable();
    }
}
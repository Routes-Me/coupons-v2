using CouponService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CouponService.Abstraction
{
    public interface IGenericRepository<T> where T : class
    {
        void Delete(int id);
        void Delete(T entity);
        List<T> Get(Pagination pagination, Expression<Func<T, bool>> filter = null,
                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                    params Expression<Func<T, object>>[] includeProperties);
        T GetById(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);
        void Post(T entity);
        void Put(T entity);
        void Remove(T entity);
        T Where(Expression<Func<T, bool>> predicate);
    }
}

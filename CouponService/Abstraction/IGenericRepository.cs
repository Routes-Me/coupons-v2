using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CouponService.Abstraction
{
    public interface IGenericRepository<T> where T : class
    {
        void Delete(int id);

        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                    params Expression<Func<T, object>>[] includeProperties);

        List<T> GetReports(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);
        T GetById(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        void Post(T entity);
        void Put(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Remove(T entity);
        bool CheckExistance(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T Where(Expression<Func<T, bool>> predicate);
    }
}

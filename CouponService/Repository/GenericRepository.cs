using CouponService.Abstraction;
using CouponService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace CouponService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal readonly CouponContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(CouponContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }


        public void Delete(int id)
        {
            var entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        public void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public T Where(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate).FirstOrDefault();
        }

        public List<T> Get(Pagination pagination, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;
            if (pagination != null)
            {
                query = query.Skip((pagination.Offset - 1) * pagination.Limit).Take(pagination.Limit);
                pagination.Total = query.Count();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties!= null)
            {
                foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.ToList();
        }
        public T GetById(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault();
        }


        public void Post(T entity)
        {
            dbSet.Add(entity);
        }

        public void Put(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }
    }
}

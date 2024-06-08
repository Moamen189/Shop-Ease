using Microsoft.EntityFrameworkCore;
using shoppingCart.DataAcess.Data;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.DataAcess.Impementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity); 
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, string? IncludeWord)
        {
            IQueryable<T> query = context.Set<T>();

            if(predicate != null)
            {
                query = query.Where(predicate);
            }

            if(IncludeWord != null)
            {
                foreach(var item in IncludeWord.Split(new char[] {','} , StringSplitOptions.RemoveEmptyEntries))
                {
                           query = query.Include(item);    
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate, string? IncludeWord)
        {
            IQueryable<T> query = context.Set<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);

        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            context.Set<T>().RemoveRange(entities);

        }
    }
}

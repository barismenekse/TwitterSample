using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Twitter.Context;

namespace Twitter.Repository
{
    public class Repository<T> where T : class
    {
       
        private TwitterContext _context;
        protected DbSet<T> table;

        public Repository(TwitterContext context)
        {
            _context = context; 
            table = _context.Set<T>();
        }
       
        public List<T> GetAll()
        {
            return table.ToList();
        }
        public T GetById(params Object[] id)
        {
            return table.Find(id);

        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;

        }

        
        public void Delete(params Object[] id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        
    
        public T Select(Expression<Func<T, bool>> predicate)
        {
            T obj = table.Where(predicate).FirstOrDefault();
            return obj;

        }
        public List<T> SelectAll(Expression<Func<T, bool>> predicate)
        {
            List<T> obj = table.Where(predicate).ToList();
            return obj;
        }
    }
}

using AuthServer.Core.Repositories;
using AuthServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task CreateAsync(T t)
        {
            await _dbSet.AddAsync(t);
        }

        public void Delete(T t)
        {
            _dbSet.Remove(t);
        }

        public IQueryable<T> GetByFilter(Expression<Func<T, bool>> filter)
        {
            var value = _dbSet.Where(filter).AsQueryable();
            return value;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var findValue = await _dbSet.FindAsync(id);
            if (findValue != null)
            {
               _context.Entry(findValue).State = EntityState.Detached;
            }
            return findValue;
        }

        public  IQueryable<T> GetListAll()
        {
            var values = _dbSet.AsQueryable();
            return values;
        }
        public  T Update(T t)
        {
            _dbSet.Update(t);
            return t;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SchoolOf.Data.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolOf.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntityModel
    {
        private readonly DbSet<T> _dbSet;

        public Repository(DatabaseContext context)
        {
            this._dbSet = context.Set<T>();
        }



        public T Add(T entity)
        {
            entity.CreatedAt = DateTimeOffset.UtcNow;
            return this._dbSet.Add(entity).Entity;
        }

        public async Task<bool> DeleteByIdAsync(long id)
        {
            var entityToBeDeleted = await this.GetByIdAsync(id);
            entityToBeDeleted.IsDeleted = true;
            return this._dbSet.Update(entityToBeDeleted) is not null;
        }

        public IEnumerable<T> Find(Func<T, bool> searchCriteria)
        {
            return this._dbSet.Where(searchCriteria).ToList();
        }


        public IEnumerable<T> Find(Expression<Func<T, bool>> searchCriteria, string includePropertyByName)
        {
            var result = this._dbSet.Where(searchCriteria);
            if (!string.IsNullOrEmpty(includePropertyByName))
            {
                result = result.Include(includePropertyByName);
            }
            return result.ToList();
        }

        public IEnumerable<T> Find(Func<T, bool> searchCriteria, int skip, int take)
        {
            
            return this._dbSet.Where(searchCriteria).Skip(skip).Take(take).ToList();
        }


        public async Task<T> GetByIdAsync(long id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public T Update(T entity)
        {
            entity.ModifiedAt = DateTimeOffset.UtcNow;
            return this._dbSet.Update(entity).Entity;
        }
    }
}

using SchoolOf.Data.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolOf.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DatabaseContext _databaseContext;

        public UnitOfWork(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntityModel
        {
            return new Repository<T>(this._databaseContext);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this._databaseContext.SaveChangesAsync() > 0;
        }
    }

}


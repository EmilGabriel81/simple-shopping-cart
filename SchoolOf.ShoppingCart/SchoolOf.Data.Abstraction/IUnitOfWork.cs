﻿
using System.Threading.Tasks;

namespace SchoolOf.Data.Abstraction
{
    interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : BaseEntityModel;
        Task<bool> SaveChangesAsync();

    }

}

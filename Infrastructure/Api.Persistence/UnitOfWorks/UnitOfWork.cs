using Api.Application.Interface.Repositories;
using Api.Application.Interface.UnitOfWorks;
using Api.Persistence.Context;
using Api.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async ValueTask DisposeAsync() => await appDbContext.DisposeAsync();

        public int Save() => appDbContext.SaveChanges(); 

        public async Task<int> SaveAsync() => await appDbContext.SaveChangesAsync();

        IReadRepository<T> IUnitOfWork.GetReadRepository<T>() => new ReadRepository<T>(appDbContext);

        IWriteRepository<T> IUnitOfWork.GetWriteRepository<T>() => new WriteRepository<T>(appDbContext);
    }
}

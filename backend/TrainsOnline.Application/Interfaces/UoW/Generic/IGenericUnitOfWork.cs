namespace TrainsOnline.Application.Interfaces.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces.Repository.Generic;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseEntity;

        IGenericReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
           where TEntity : class, IBaseEntity;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

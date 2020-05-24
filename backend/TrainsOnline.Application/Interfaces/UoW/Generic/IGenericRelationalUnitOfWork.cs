namespace TrainsOnline.Application.Interfaces.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces.Repository.Generic;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericRelationalUnitOfWork
    {
        IGenericRelationalRepository<TEntity> GetRepositoryByName<TEntity>(string name)
            where TEntity : class, IBaseRelationalEntity;

        IGenericRelationalRepository<TEntity> GetRepository<TEntity>()
                    where TEntity : class, IBaseRelationalEntity;

        IGenericReadOnlyRepository<TEntity> GetReadOnlyRepositoryByName<TEntity>(string name)
            where TEntity : class, IBaseRelationalEntity;

        IGenericReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
           where TEntity : class, IBaseRelationalEntity;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

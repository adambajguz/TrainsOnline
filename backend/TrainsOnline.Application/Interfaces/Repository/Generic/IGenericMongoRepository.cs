namespace TrainsOnline.Application.Interfaces.Repository.Generic
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericMongoRepository<TEntity> : IGenericMongoReadOnlyRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(Guid id);
        Task RemoveAsync(TEntity entity);
    }
}
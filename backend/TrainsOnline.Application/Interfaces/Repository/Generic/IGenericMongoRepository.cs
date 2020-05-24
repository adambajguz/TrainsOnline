namespace TrainsOnline.Application.Interfaces.Repository.Generic
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericMongoRepository
    {

    }

    public interface IGenericMongoRepository<TEntity> : IGenericMongoRepository, IGenericMongoReadOnlyRepository<TEntity>
        where TEntity : class, IBaseMongoEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(Guid id);
        Task RemoveAsync(TEntity entity);
    }
}
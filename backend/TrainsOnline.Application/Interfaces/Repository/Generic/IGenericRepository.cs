namespace TrainsOnline.Application.Interfaces.Repository.Generic
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericRepository<TEntity> : IGenericReadOnlyRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        Task Remove(Guid id);

        void Remove(TEntity entity);
    }
}
namespace TrainsOnline.Application.Interfaces.Repository.Generic
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericRelationalRepository : IGenericRelationalReadOnlyRepository
    {
        IBaseRelationalEntity Add(IBaseRelationalEntity entity);

        void Update(IBaseRelationalEntity entity);

        Task Remove(Guid id);

        void Remove(IBaseRelationalEntity entity);
    }

    public interface IGenericRelationalRepository<TEntity> : IGenericRelationalRepository, IGenericRelationalReadOnlyRepository<TEntity>
        where TEntity : class, IBaseRelationalEntity
    {
        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);
    }
}
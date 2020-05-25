namespace TrainsOnline.Application.Interfaces.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericMongoReadOnlyRepository
    {
        Type GetEntityType();

        Task<IEnumerable<IBaseMongoEntity>> GetAll();

        Task<IBaseMongoEntity?> GetByIdAsync(Guid id);

        Task<int> GetCountAsync();
    }

    public interface IGenericMongoReadOnlyRepository<TEntity> : IGenericMongoReadOnlyRepository
        where TEntity : class, IBaseMongoEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

        Task<TEntity?> GetByIdAsync(Guid id);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<List<T>> ProjectToAsync<T>(Expression<Func<TEntity, bool>>? filter = null,
                                        CancellationToken cancellationToken = default);
    }
}

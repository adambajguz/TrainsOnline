namespace TrainsOnline.Application.Interfaces.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using MongoDB.Driver.Linq;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericMongoReadOnlyRepository
    {
        public Type EntityType { get; }
        public string EntityName { get; }

        Task<IEnumerable<IBaseMongoEntity>> All();

        Task<IBaseMongoEntity> SingleByIdAsync(Guid id);
        Task<IBaseMongoEntity?> SingleByIdOrDefaultAsync(Guid id);

        Task<int> GetCountAsync();
    }

    public interface IGenericMongoReadOnlyRepository<TEntity> : IGenericMongoReadOnlyRepository
        where TEntity : class, IBaseMongoEntity
    {
        Task<IEnumerable<TEntity>> AllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                            Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>>? orderBy = null);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? filter,
                                 CancellationToken cancellationToken = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter,
                                           CancellationToken cancellationToken = default);

        new Task<TEntity?> SingleByIdAsync(Guid id);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<List<T>> ProjectToAsync<T>(Expression<Func<TEntity, bool>>? filter = null);
    }
}

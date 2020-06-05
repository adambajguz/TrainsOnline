namespace TrainsOnline.Application.Interfaces.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericRelationalReadOnlyRepository
    {
        public Type EntityType { get; }
        public string EntityName { get; }

        Task<IEnumerable<IBaseRelationalEntity>> All();

        Task<IBaseRelationalEntity> SingleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IBaseRelationalEntity?> SingleByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IBaseRelationalEntity> NoTrackingSingleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IBaseRelationalEntity?> NoTrackingSingleByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync();
    }

    public interface IGenericRelationalReadOnlyRepository<TEntity> : IGenericRelationalReadOnlyRepository
        where TEntity : class, IBaseRelationalEntity
    {
        Task<IEnumerable<TEntity>> AllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                            CancellationToken cancellationToken = default);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> filter,
                                  CancellationToken cancellationToken = default);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                            CancellationToken cancellationToken = default);

        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                 CancellationToken cancellationToken = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                           CancellationToken cancellationToken = default);

        Task<TEntity> NoTrackigFirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                           CancellationToken cancellationToken = default);
        Task<TEntity?> NoTrackigFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                    CancellationToken cancellationToken = default);

        new Task<TEntity> SingleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        new Task<TEntity?> SingleByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

        new Task<TEntity> NoTrackingSingleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        new Task<TEntity?> NoTrackingSingleByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

        Task<TEntity?> SingleByIdWithRelatedAsync<TProperty0>(Guid id,
                                                              Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                              CancellationToken cancellationToken = default);
        Task<TEntity?> SingleByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                          Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                          Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                          CancellationToken cancellationToken = default);
        Task<TEntity?> SingleByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                          Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                          Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                          CancellationToken cancellationToken = default,
                                                                          params Expression<Func<TEntity, object>>[] relatedSelectors);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? filter = null);

        #region ProjectTo
        Task<List<T>> ProjectToAsync<T>(Expression<Func<TEntity, bool>>? filter = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                        CancellationToken cancellationToken = default);
        Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                               Expression<Func<TEntity, bool>>? filter = null,
                                                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                               CancellationToken cancellationToken = default);
        Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0, TProperty1>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                           Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                           Expression<Func<TEntity, bool>>? filter = null,
                                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                           CancellationToken cancellationToken = default);
        Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0, TProperty1>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                           Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                           Expression<Func<TEntity, bool>>? filter = null,
                                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                           CancellationToken cancellationToken = default,
                                                                           params Expression<Func<TEntity, object>>[] relatedSelectors);
        #endregion
    }
}

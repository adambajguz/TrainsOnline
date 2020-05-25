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
        Type GetEntityType();

        Task<IEnumerable<IBaseRelationalEntity>> GetAll();

        Task<IBaseRelationalEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IBaseRelationalEntity?> NoTrackingGetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync();
    }

    public interface IGenericRelationalReadOnlyRepository<TEntity> : IGenericRelationalReadOnlyRepository
        where TEntity : class, IBaseRelationalEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                               CancellationToken cancellationToken = default);

        Task<TEntity?> GetOneOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                   CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                           CancellationToken cancellationToken = default);
        Task<TEntity?> NoTrackigFirstOrDefaultAsync(CancellationToken cancellationToken = default);
        Task<TEntity?> NoTrackigFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                    CancellationToken cancellationToken = default);

        new Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        new Task<TEntity?> NoTrackingGetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<TEntity?> GetByIdWithRelatedAsync<TProperty0>(Guid id,
                                                           Expression<Func<TEntity, TProperty0>> relatedSelector0, 
                                                           CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                       Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                       Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                       CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                       Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                       Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                       CancellationToken cancellationToken = default,
                                                                       params Expression<Func<TEntity, object>>[] relatedSelectors);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>>? filter = null);

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

namespace TrainsOnline.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Interfaces.Repository.Generic;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using TrainsOnline.Application.Exceptions;
    using TrainsOnline.Domain.Abstractions.Base;

    public class GenericReadOnlyRelationalRepository<TEntity> : IGenericRelationalReadOnlyRepository<TEntity>
        where TEntity : class, IBaseRelationalEntity
    {
        protected readonly IGenericDatabaseContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public Type EntityType { get; }
        public string EntityName { get; }

        public GenericReadOnlyRelationalRepository(IGenericDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _mapper = mapper;

            Type type = typeof(TEntity);
            EntityType = type;
            EntityName = type.Name;
        }

        protected IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? filter = null,
                                                   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        #region IGenericRelationalReadOnlyRepository<TEntity>
        public async Task<IEnumerable<TEntity>> AllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                            CancellationToken cancellationToken = default)
        {
            return await GetQueryable(filter, orderBy).ToListAsync(cancellationToken);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> filter,
                                               CancellationToken cancellationToken = default)
        {
            return await _dbSet.SingleOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                         CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbSet.SingleOrDefaultAsync(filter, cancellationToken);

            return await _dbSet.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                              CancellationToken cancellationToken = default)
        {
            TEntity? entity;
            if (filter is null)
                entity = await _dbSet.FirstOrDefaultAsync(cancellationToken);
            else
                entity = await _dbSet.FirstOrDefaultAsync(filter, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                        CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbSet.FirstOrDefaultAsync(cancellationToken);

            return await _dbSet.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity> NoTrackigFirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                       CancellationToken cancellationToken = default)
        {
            TEntity? entity;
            if (filter is null)
                entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            else
                entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> NoTrackigFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                                 CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity> SingleByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await _dbSet.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName, id);
        }

        public async Task<TEntity?> SingleByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TEntity> NoTrackingSingleByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName, id);
        }

        public async Task<TEntity?> NoTrackingSingleByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TEntity?> SingleByIdWithRelatedAsync<TProperty0>(Guid id,
                                                                           Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                           CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(relatedSelector0)
                               .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TEntity?> SingleByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                                       Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                       Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                       CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(relatedSelector0)
                               .Include(relatedSelector1)
                               .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TEntity?> SingleByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                                       Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                       Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                       CancellationToken cancellationToken = default,
                                                                                       params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            IQueryable<TEntity> expr = _dbSet.Include(relatedSelector0)
                                             .Include(relatedSelector1);

            foreach (Expression<Func<TEntity, object>> relatedExpr in relatedSelectors)
            {
                expr = expr.Include(relatedExpr);
            }

            return await expr.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).CountAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).AnyAsync();
        }

        #region ProjectTo
        public async Task<List<T>> ProjectToAsync<T>(Expression<Func<TEntity, bool>>? filter = null,
                                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                     CancellationToken cancellationToken = default)
        {
            return await GetQueryable(filter, orderBy).ProjectTo<T>(_mapper.ConfigurationProvider)
                                                      .ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                            Expression<Func<TEntity, bool>>? filter = null,
                                                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                            CancellationToken cancellationToken = default)
        {
            return await GetQueryable(filter, orderBy).Include(relatedSelector0)
                                                      .ProjectTo<T>(_mapper.ConfigurationProvider)
                                                      .ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0, TProperty1>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                        Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                        Expression<Func<TEntity, bool>>? filter = null,
                                                                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                                        CancellationToken cancellationToken = default)
        {
            return await GetQueryable(filter, orderBy).Include(relatedSelector0)
                                                      .Include(relatedSelector1)
                                                      .ProjectTo<T>(_mapper.ConfigurationProvider)
                                                      .ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0, TProperty1>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                        Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                        Expression<Func<TEntity, bool>>? filter = null,
                                                                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                                        CancellationToken cancellationToken = default,
                                                                                        params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            IQueryable<TEntity> expr = GetQueryable(filter, orderBy).Include(relatedSelector0)
                                                                    .Include(relatedSelector1);

            foreach (Expression<Func<TEntity, object>> relatedExpr in relatedSelectors)
            {
                expr = expr.Include(relatedExpr);
            }

            return await expr.ProjectTo<T>(_mapper.ConfigurationProvider)
                             .ToListAsync(cancellationToken);
        }
        #endregion
        #endregion

        #region IGenericRelationalReadOnlyRepository
        public async Task<IEnumerable<IBaseRelationalEntity>> All()
        {
            return await _dbSet.AsQueryable().ToListAsync();
        }

        async Task<IBaseRelationalEntity> IGenericRelationalReadOnlyRepository.SingleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            TEntity? entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName, id);
        }

        async Task<IBaseRelationalEntity?> IGenericRelationalReadOnlyRepository.SingleByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        async Task<IBaseRelationalEntity> IGenericRelationalReadOnlyRepository.NoTrackingSingleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            TEntity? entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName, id);
        }

        async Task<IBaseRelationalEntity?> IGenericRelationalReadOnlyRepository.NoTrackingSingleByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<int> GetCountAsync()
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            return await query.CountAsync();
        }
        #endregion
    }
}

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
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using TrainsOnline.Domain.Abstractions.Base;

    public class GenericReadOnlyRepository<TEntity> : IGenericReadOnlyRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        protected readonly IGenericDatabaseContext _context;
        protected readonly IMongoCollection<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public GenericReadOnlyRepository(IGenericDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.GetCollection<TEntity>();
            _mapper = mapper;
        }

        protected virtual IMongoQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>>? filter = null)
        {
            IMongoQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await GetQueryable(null).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).ToListAsync();
        }

        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).SingleOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> NoTrackigFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> NoTrackigFirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity> GetByIdWithRelatedAsync<TProperty0>(Guid id, Expression<Func<TEntity, TProperty0>> relatedSelector0)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            //return await _dbSet.Include(relatedSelector0)
            //                   .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity> GetByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                                           Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                           Expression<Func<TEntity, TProperty1>> relatedSelector1)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            //return await _dbSet.Include(relatedSelector0)
            //                   .Include(relatedSelector1)
            //                   .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity> GetByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                                           Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                           Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                           params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            //IQueryable<TEntity> expr = _dbSet.Include(relatedSelector0)
            //                                 .Include(relatedSelector1);

            //foreach (Expression<Func<TEntity, object>> relatedExpr in relatedSelectors)
            //{
            //    expr = expr.Include(relatedExpr);
            //}

            //return await expr.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity> NoTrackigGetByIdAsync(Guid id)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
            //return _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).CountAsync();
        }

        public virtual async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).AnyAsync();
        }

        public virtual List<T> ProjectTo<T>(Expression<Func<TEntity, bool>>? filter = null,
                                                             CancellationToken cancellationToken = default)
        {
            return GetQueryable(filter).ProjectTo<T>(_mapper.ConfigurationProvider).ToList();
        }

        public virtual async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                    Expression<Func<TEntity, bool>>? filter = null,
                                                                                    CancellationToken cancellationToken = default)
        {
            return GetQueryable(filter).ProjectTo<T>(_mapper.ConfigurationProvider).ToList();

            //return await GetQueryable(filter).Include(relatedSelector0)
            //                                 .ProjectTo<T>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0, TProperty1>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                                Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                                Expression<Func<TEntity, bool>>? filter = null,
                                                                                                CancellationToken cancellationToken = default)
        {
            return GetQueryable(filter).ProjectTo<T>(_mapper.ConfigurationProvider).ToList();

            //return await GetQueryable(filter).Include(relatedSelector0)
            //                                          .Include(relatedSelector1)
            //                                          .ProjectTo<T>(_mapper.ConfigurationProvider).ToList(cancellationToken);
        }

        public virtual async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0, TProperty1>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                                Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                                Expression<Func<TEntity, bool>>? filter = null,
                                                                                                CancellationToken cancellationToken = default,
                                                                                                params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            return GetQueryable(filter).ProjectTo<T>(_mapper.ConfigurationProvider).ToList();

            //IMongoQueryable<TEntity> expr = GetQueryable(filter).Include(relatedSelector0)
            //                                                    .Include(relatedSelector1);

            //foreach (Expression<Func<TEntity, object>> relatedExpr in relatedSelectors)
            //{
            //    expr = expr.Include(relatedExpr);
            //}

            //return await expr.ProjectTo<T>(_mapper.ConfigurationProvider).ToList(cancellationToken);
        }
    }
}

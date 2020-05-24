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
    using TrainsOnline.Domain.Abstractions.Base;

    public class GenericReadOnlyRelationalRepository<TEntity> : IGenericReadOnlyRepository<TEntity>
        where TEntity : class, IBaseRelationalEntity
    {
        protected readonly IGenericDatabaseContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public GenericReadOnlyRelationalRepository(IGenericDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _mapper = mapper;
        }

        protected virtual IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            return await GetQueryable(null, orderBy).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            return await GetQueryable(filter, orderBy).ToListAsync();
        }


        public virtual async Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter, null).SingleOrDefaultAsync();
        }


        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            return await GetQueryable(filter, orderBy).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> NoTrackigFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> NoTrackigFirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
            //return _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity> GetByIdWithRelatedAsync<TProperty0>(Guid id, Expression<Func<TEntity, TProperty0>> relatedSelector0)
        {
            return await _dbSet.Include(relatedSelector0)
                               .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity> GetByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                                           Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                           Expression<Func<TEntity, TProperty1>> relatedSelector1)
        {
            return await _dbSet.Include(relatedSelector0)
                               .Include(relatedSelector1)
                               .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity> GetByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                                           Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                           Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                           params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            IQueryable<TEntity> expr = _dbSet.Include(relatedSelector0)
                                             .Include(relatedSelector1);

            foreach (Expression<Func<TEntity, object>> relatedExpr in relatedSelectors)
            {
                expr = expr.Include(relatedExpr);
            }

            return await expr.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity> NoTrackigGetByIdAsync(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
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

        public virtual async Task<List<T>> ProjectToAsync<T>(Expression<Func<TEntity, bool>>? filter = null,
                                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                              CancellationToken cancellationToken = default)
        {
            return await GetQueryable(filter, orderBy).ProjectTo<T>(_mapper.ConfigurationProvider)
                                                      .ToListAsync(cancellationToken);
        }

        public virtual async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                    Expression<Func<TEntity, bool>>? filter = null,
                                                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                                    CancellationToken cancellationToken = default)
        {
            return await GetQueryable(filter, orderBy).Include(relatedSelector0)
                                                      .ProjectTo<T>(_mapper.ConfigurationProvider)
                                                      .ToListAsync(cancellationToken);
        }

        public virtual async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0, TProperty1>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
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

        public virtual async Task<List<T>> ProjectToWithRelatedAsync<T, TProperty0, TProperty1>(Expression<Func<TEntity, TProperty0>> relatedSelector0,
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
    }
}

﻿namespace TrainsOnline.Infrastructure.Repository
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

    public class GenericReadOnlyRelationalRepository<TEntity> : IGenericRelationalReadOnlyRepository<TEntity>
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
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                            CancellationToken cancellationToken = default)
        {
            return await GetQueryable(filter, orderBy).ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetOneOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbSet.SingleOrDefaultAsync(filter, cancellationToken);

            return await _dbSet.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                        CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbSet.FirstOrDefaultAsync(cancellationToken);

            return await _dbSet.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity?> NoTrackigFirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity?> NoTrackigFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                                 CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            //return await _dbSet.FindAsync(id);
            return await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<TEntity?> NoTrackingGetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<TEntity?> GetByIdWithRelatedAsync<TProperty0>(Guid id,
                                                                        Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                        CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(relatedSelector0)
                               .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<TEntity?> GetByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                                    Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                    Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                    CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(relatedSelector0)
                               .Include(relatedSelector1)
                               .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<TEntity?> GetByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
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

            return await expr.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).CountAsync();
        }

        public async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>>? filter = null)
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
        public Type GetEntityType()
        {
            return typeof(TEntity);
        }

        public async Task<IEnumerable<IBaseRelationalEntity>> GetAll()
        {
            return await _dbSet.AsQueryable().ToListAsync();
        }

        async Task<IBaseRelationalEntity?> IGenericRelationalReadOnlyRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            //return await _dbSet.FindAsync(id);
            return await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        async Task<IBaseRelationalEntity?> IGenericRelationalReadOnlyRepository.NoTrackingGetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<int> GetCountAsync()
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            return await query.CountAsync();
        }
        #endregion
    }
}

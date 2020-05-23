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

    public class GenericMongoReadOnlyRepository<TEntity> : IGenericMongoReadOnlyRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        protected readonly IGenericMongoDatabaseContext _context;
        protected readonly IMongoCollection<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public GenericMongoReadOnlyRepository(IGenericMongoDatabaseContext context, IMapper mapper)
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

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
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
                                                       CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => GetQueryable(filter).ProjectTo<T>(_mapper.ConfigurationProvider).ToList());
        }
    }
}

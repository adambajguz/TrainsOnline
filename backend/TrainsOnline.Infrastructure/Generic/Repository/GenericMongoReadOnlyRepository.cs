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
        where TEntity : class, IBaseMongoEntity
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

        protected IMongoQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? filter = null,
                                                        Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>>? orderBy = null)
        {
            IMongoQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        #region IGenericMongoReadOnlyRepository<TEntity>
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                            Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>>? orderBy = null)
        {
            return await GetQueryable(filter, orderBy).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).ToListAsync();
        }

        public async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).SingleOrDefaultAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, 
                                                        CancellationToken cancellationToken = default)
        {
            if(predicate is null)
                return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(cancellationToken);

            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).CountAsync();
        }

        public async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).AnyAsync();
        }

        public async Task<List<T>> ProjectToAsync<T>(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await Task.Run(() => GetQueryable(filter).ProjectTo<T>(_mapper.ConfigurationProvider)
                                                            .ToList());
        }
        #endregion

        #region IGenericMongoReadOnlyRepository
        public Type GetEntityType()
        {
            return typeof(TEntity);
        }

        public async Task<IEnumerable<IBaseMongoEntity>> GetAll()
        {
            return await _dbSet.AsQueryable().ToListAsync();
        }

        async Task<IBaseMongoEntity?> IGenericMongoReadOnlyRepository.GetByIdAsync(Guid id)
        {
            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbSet.AsQueryable().CountAsync();
        }
        #endregion
    }
}

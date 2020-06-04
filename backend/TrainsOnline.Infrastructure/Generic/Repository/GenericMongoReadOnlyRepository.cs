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
    using TrainsOnline.Application.Exceptions;
    using TrainsOnline.Domain.Abstractions.Base;

    public class GenericMongoReadOnlyRepository<TEntity> : IGenericMongoReadOnlyRepository<TEntity>
        where TEntity : class, IBaseMongoEntity
    {
        protected readonly IGenericMongoDatabaseContext _context;
        protected readonly IMongoCollection<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public Type EntityType { get; }
        public string EntityName { get; }

        public GenericMongoReadOnlyRepository(IGenericMongoDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.GetCollection<TEntity>();
            _mapper = mapper;

            Type type = typeof(TEntity);
            EntityType = type;
            EntityName = type.Name;
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
        public async Task<IEnumerable<TEntity>> AllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                            Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>>? orderBy = null)
        {
            return await GetQueryable(filter, orderBy).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).ToListAsync();
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            TEntity? entity = await GetQueryable(filter).SingleOrDefaultAsync();

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).SingleOrDefaultAsync();
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                               CancellationToken cancellationToken = default)
        {
            TEntity? entity;
            if (filter is null)
                entity = await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(cancellationToken);
            else
                entity = await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(filter, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                        CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(cancellationToken);

            return await _dbSet.AsQueryable<TEntity>().FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity?> SingleByIdAsync(Guid id)
        {
            return await _dbSet.AsQueryable<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).CountAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? filter = null)
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

        public async Task<IEnumerable<IBaseMongoEntity>> All()
        {
            return await _dbSet.AsQueryable().ToListAsync();
        }

        async Task<IBaseMongoEntity> IGenericMongoReadOnlyRepository.SingleByIdAsync(Guid id)
        {
            TEntity? entity = await _dbSet.AsQueryable<TEntity>().SingleOrDefaultAsync(x => x.Id == id);

            return entity ?? throw new NotFoundException(EntityName, id);
        }

        async Task<IBaseMongoEntity?> IGenericMongoReadOnlyRepository.SingleByIdOrDefaultAsync(Guid id)
        {
            return await _dbSet.AsQueryable<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbSet.AsQueryable().CountAsync();
        }
        #endregion
    }
}

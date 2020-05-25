namespace TrainsOnline.Infrastructure.Repository
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Interfaces.Repository.Generic;
    using AutoMapper;
    using MongoDB.Driver;
    using TrainsOnline.Domain.Abstractions.Base;

    public class GenericMongoRepository<TEntity> : GenericMongoReadOnlyRepository<TEntity>, IGenericMongoRepository<TEntity>
        where TEntity : class, IBaseMongoEntity
    {
        private readonly ICurrentUserService _currentUser;

        public GenericMongoRepository(ICurrentUserService currentUserService, IGenericMongoDatabaseContext context, IMapper mapper) : base(context, mapper)
        {
            _currentUser = currentUserService;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            DateTime time = DateTime.UtcNow;
            Guid? userGuid = _currentUser.UserId;

            if (entity is IEntityCreation entityCreation)
            {
                entityCreation.CreatedOn = time;
                entityCreation.CreatedBy = userGuid;
            }

            if (entity is IEntityLastSaved entityModification)
            {
                entityModification.LastSavedOn = time;
                entityModification.LastSavedBy = userGuid;
            }

            await _dbSet.InsertOneAsync(entity);

            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity is IEntityLastSaved entityModification)
            {
                entityModification.LastSavedOn = DateTime.UtcNow;
                entityModification.LastSavedBy = _currentUser.UserId;
            }

            FilterDefinition<TEntity> idFilter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);

            await _dbSet.ReplaceOneAsync(idFilter, entity);
        }

        public virtual async Task RemoveAsync(Guid id)
        {
            FilterDefinition<TEntity> idFilter = Builders<TEntity>.Filter.Eq(x => x.Id, id);

            await _dbSet.DeleteOneAsync(idFilter);
        }

        public virtual async Task RemoveAsync(TEntity entity)
        {
            FilterDefinition<TEntity> idFilter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);

            await _dbSet.DeleteOneAsync(idFilter);
        }
    }
}

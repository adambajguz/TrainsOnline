namespace TrainsOnline.Infrastructure.Repository
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Interfaces.Repository.Generic;
    using AutoMapper;
    using TrainsOnline.Domain.Abstractions.Base;

    public class GenericRepository<TEntity> : GenericReadOnlyRepository<TEntity>, IGenericRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        private ICurrentUserService CurrentUser { get; }

        public GenericRepository(ICurrentUserService currentUserService, IGenericDatabaseContext context, IMapper mapper) : base(context, mapper)
        {
            CurrentUser = currentUserService;
        }

        public virtual TEntity Add(TEntity entity)
        {
            DateTime time = DateTime.UtcNow;
            Guid? userGuid = CurrentUser.UserId;

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

            EntityEntry<TEntity> createdEntity = _dbSet.Add(entity);

            return createdEntity.Entity;
        }

        public virtual void Update(TEntity entity)
        {
            if (entity is IEntityLastSaved entityModification)
            {
                entityModification.LastSavedOn = DateTime.UtcNow;
                entityModification.LastSavedBy = CurrentUser.UserId;
            }
            _dbSet.Attach(entity);
            //_context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task Remove(Guid id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            Remove(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
        }
    }
}

namespace TrainsOnline.Infrastructure.Repository
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Interfaces.Repository.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using TrainsOnline.Domain.Abstractions.Base;

    public class GenericRelationalRepository<TEntity> : GenericReadOnlyRelationalRepository<TEntity>, IGenericRelationalRepository<TEntity>
        where TEntity : class, IBaseRelationalEntity
    {
        private ICurrentUserService CurrentUser { get; }

        public GenericRelationalRepository(ICurrentUserService currentUserService, IGenericDatabaseContext context, IMapper mapper) : base(context, mapper)
        {
            CurrentUser = currentUserService;
        }

        #region IGenericRelationalRepository<TEntity>
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
        #endregion

        #region IGenericRelationalRepository
        public IBaseRelationalEntity Add(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
                return Add(e);

            throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
        }

        public void Update(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
                Update(e);
            else
                throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
        }

        public void Remove(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
                Remove(e);
            else
                throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
        }
        #endregion
    }
}

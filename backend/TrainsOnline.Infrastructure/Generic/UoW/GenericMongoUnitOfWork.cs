namespace TrainsOnline.Infrastructure.UoW
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces;
    using Application.Interfaces.Repository.Generic;
    using AutoMapper;
    using Infrastructure.Repository;
    using TrainsOnline.Application.Interfaces.UoW.Generic;
    using TrainsOnline.Domain.Abstractions.Base;

    public abstract class GenericMongoUnitOfWork : IGenericMongoUnitOfWork, IDisposable
    {
        protected ICurrentUserService CurrentUser { get; private set; }
        protected IGenericMongoDatabaseContext Context { get; private set; }
        protected IMapper Mapper { get; private set; }

        public bool IsDisposed { get; private set; }

        private Dictionary<Type, IGenericMongoReadOnlyRepository> Repositories { get; } = new Dictionary<Type, IGenericMongoReadOnlyRepository>();

        public GenericMongoUnitOfWork(ICurrentUserService currentUserService, IGenericMongoDatabaseContext context, IMapper mapper)
        {
            CurrentUser = currentUserService;
            Context = context;
            Mapper = mapper;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                //Context.Dispose();
                Repositories.Clear();
            }

            IsDisposed = true;
        }

        public IGenericMongoRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseMongoEntity
        {
            Type type = typeof(IGenericMongoRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericMongoReadOnlyRepository? value))
            {
                return (value as IGenericMongoRepository<TEntity>)!;
            }

            IGenericMongoRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericMongoRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericMongoRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        public IGenericMongoReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseMongoEntity
        {
            Type type = typeof(IGenericMongoReadOnlyRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericMongoReadOnlyRepository? value))
            {
                return (value as IGenericMongoReadOnlyRepository<TEntity>)!;
            }

            IGenericMongoReadOnlyRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericMongoReadOnlyRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericMongoReadOnlyRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        protected TSpecificRepositoryInterface GetSpecificRepository<TSpecificRepositoryInterface, TSpecificRepository>()
            where TSpecificRepositoryInterface : IGenericMongoReadOnlyRepository
            where TSpecificRepository : class, IGenericMongoReadOnlyRepository
        {
            Type type = typeof(TSpecificRepositoryInterface);
            if (Repositories.ContainsKey(type))
            {
                return (TSpecificRepositoryInterface)Repositories[type];
            }

            TSpecificRepositoryInterface repository = (TSpecificRepositoryInterface)Activator.CreateInstance(typeof(TSpecificRepository), CurrentUser, Context, Mapper)!;
            Repositories.Add(type, repository);
            return repository;
        }
    }
}
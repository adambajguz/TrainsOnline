namespace TrainsOnline.Infrastructure.UoW
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Interfaces.Repository.Generic;
    using Application.Interfaces.UoW;
    using AutoMapper;
    using Infrastructure.Repository;
    using TrainsOnline.Domain.Abstractions.Base;

    public abstract class GenericUnitOfWork : IGenericUnitOfWork, IDisposable
    {
        protected ICurrentUserService CurrentUser { get; private set; }
        protected IGenericDatabaseContext Context { get; private set; }
        protected IMapper Mapper { get; private set; }

        public bool IsDisposed { get; private set; }

        private Dictionary<Type, IGenericReadOnlyRepository> Repositories { get; } = new Dictionary<Type, IGenericReadOnlyRepository>();

        public GenericUnitOfWork(ICurrentUserService currentUserService, IGenericDatabaseContext context, IMapper mapper)
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
                Context.Dispose();
                Repositories.Clear();
            }

            IsDisposed = true;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseEntity
        {
            Type type = typeof(IGenericRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericReadOnlyRepository? value))
            {
                return (value as IGenericRepository<TEntity>)!;
            }

            IGenericRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        public IGenericReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseEntity
        {
            Type type = typeof(IGenericReadOnlyRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericReadOnlyRepository? value))
            {
                return (value as IGenericReadOnlyRepository<TEntity>)!;
            }

            IGenericReadOnlyRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericReadOnlyRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericReadOnlyRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        protected TSpecificRepositoryInterface GetSpecificRepository<TSpecificRepositoryInterface, TSpecificRepository>()
            where TSpecificRepositoryInterface : IGenericReadOnlyRepository
            where TSpecificRepository : class, IGenericReadOnlyRepository
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

        public virtual int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
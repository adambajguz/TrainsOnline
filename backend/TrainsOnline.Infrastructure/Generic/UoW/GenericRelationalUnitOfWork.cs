namespace TrainsOnline.Infrastructure.UoW
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Interfaces.Repository.Generic;
    using AutoMapper;
    using Infrastructure.Repository;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using TrainsOnline.Application.Interfaces.UoW.Generic;
    using TrainsOnline.Domain.Abstractions.Base;
    using TrainsOnline.Infrastructure.Extensions;

    public abstract class GenericRelationalUnitOfWork : IGenericRelationalUnitOfWork, IDisposable
    {
        protected ICurrentUserService CurrentUser { get; private set; }
        protected IGenericDatabaseContext Context { get; private set; }
        protected IMapper Mapper { get; private set; }

        public bool IsDisposed { get; private set; }

        private Dictionary<Type, IGenericRelationalReadOnlyRepository> Repositories { get; } = new Dictionary<Type, IGenericRelationalReadOnlyRepository>();
        private Dictionary<string, Type> TableNameToEntityLookup { get; } = new Dictionary<string, Type>();

        public GenericRelationalUnitOfWork(ICurrentUserService currentUserService, IGenericDatabaseContext context, IMapper mapper)
        {
            CurrentUser = currentUserService;
            Context = context;
            Mapper = mapper;

            TableNameToEntityLookup = BuildTableNameToEntityLookup(context, "*.Domain*");
        }

        private static Dictionary<string, Type> BuildTableNameToEntityLookup(IGenericDatabaseContext context, params string[] assemblyFilters)
        {
            var exportedTypes = ReflectionExtensions.GetExportedTypes(assemblyFilters);

            IEnumerable<Type> typesWithAttributes = exportedTypes.Where(type => !type.IsAbstract &&
                                                                                !type.IsGenericTypeDefinition &&
                                                                                type.IsAssignableFrom(typeof(IBaseRelationalEntity)) &&
                                                                                !type.IsAssignableFrom(typeof(IBaseMongoEntity)));

            Dictionary<string, Type> tableNameToEntityLookup = new Dictionary<string, Type>();
            foreach (Type type in typesWithAttributes)
            {
                IEntityType entityType = context.Model.FindEntityType(type);
                string schema = entityType.GetSchema();
                string tableName = entityType.GetTableName();

                tableNameToEntityLookup.Add(tableName, type);
            }

            return tableNameToEntityLookup;
        }

        protected Type GetEntityTypeFromTableName(string tableName)
        {
            bool found = TableNameToEntityLookup.TryGetValue(tableName, out Type? type);
            if (!found)
                throw new ArgumentException("Invalid table name", nameof(tableName));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return type;
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

        public IGenericRelationalRepository<TEntity> GetRepositoryByName<TEntity>(string name)
            where TEntity : class, IBaseRelationalEntity
        {
            Type type = typeof(IGenericRelationalRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericRelationalReadOnlyRepository? value))
            {
                return (value as IGenericRelationalRepository<TEntity>)!;
            }

            IGenericRelationalRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericRelationalRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericRelationalRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        public IGenericRelationalRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            Type type = typeof(IGenericRelationalRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericRelationalReadOnlyRepository? value))
            {
                return (value as IGenericRelationalRepository<TEntity>)!;
            }

            IGenericRelationalRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericRelationalRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericRelationalRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        public IGenericReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            Type type = typeof(IGenericReadOnlyRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericRelationalReadOnlyRepository? value))
            {
                return (value as IGenericReadOnlyRepository<TEntity>)!;
            }

            IGenericReadOnlyRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericReadOnlyRelationalRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericReadOnlyRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        public IGenericReadOnlyRepository<TEntity> GetReadOnlyRepositoryByName<TEntity>(string name)
            where TEntity : class, IBaseRelationalEntity
        {
            //TODO
            Type type = typeof(IGenericReadOnlyRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericRelationalReadOnlyRepository? value))
            {
                return (value as IGenericReadOnlyRepository<TEntity>)!;
            }

            IGenericReadOnlyRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericReadOnlyRelationalRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericReadOnlyRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        protected TSpecificRepositoryInterface GetSpecificRepository<TSpecificRepositoryInterface, TSpecificRepository>()
            where TSpecificRepositoryInterface : IGenericRelationalReadOnlyRepository
            where TSpecificRepository : class, IGenericRelationalReadOnlyRepository
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
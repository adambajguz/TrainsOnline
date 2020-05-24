namespace TrainsOnline.Infrastructure.UoW
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Interfaces.Repository;
    using AutoMapper;
    using Infrastructure.Repository;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Newtonsoft.Json;
    using TrainsOnline.Application.Interfaces.UoW.Generic;
    using TrainsOnline.Domain.Abstractions.Audit;
    using TrainsOnline.Domain.Abstractions.Enums;
    using TrainsOnline.Domain.Entities.Audit;

    public abstract class GenericAuditableUnitOfWork : GenericUnitOfWork, IGenericAuditableUnitOfWork
    {
        private IEntityAuditLogsRepository? _entityAuditLogsRepository;

        public IEntityAuditLogsRepository EntityAuditLogsRepository => _entityAuditLogsRepository ?? (_entityAuditLogsRepository = GetSpecificRepository<IEntityAuditLogsRepository, EntityAuditLogRepository>());


        public GenericAuditableUnitOfWork(ICurrentUserService currentUserService, IGenericDatabaseContext context, IMapper mapper) : base(currentUserService, context, mapper)
        {

        }

        public override int SaveChanges()
        {
            OnBeforeSaveChanges();

            return Context.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();

            return await Context.SaveChangesAsync(cancellationToken);
        }

        public int SaveChangesWithoutAudit()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesWithoutAuditAsync(CancellationToken cancellationToken = default)
        {
            return await Context.SaveChangesAsync(cancellationToken);
        }

        //https://www.meziantou.net/entity-framework-core-history-audit-table.htm
        private void OnBeforeSaveChanges()
        {
            Context.ChangeTracker.DetectChanges();
            IEnumerable<EntityEntry> entries = Context.ChangeTracker.Entries();

            List<EntityAuditLog> auditLogsToAdd = ProcessChanges(entries);

            foreach (EntityAuditLog log in auditLogsToAdd)
            {
                EntityAuditLogsRepository.Add(log);
            }

            static List<EntityAuditLog> ProcessChanges(IEnumerable<EntityEntry> entries)
            {
                List<EntityAuditLog> auditLogsToAdd = new List<EntityAuditLog>();

                foreach (EntityEntry entry in entries)
                {
                    object entity = entry.Entity;
                    if (entity is IAuditableEntitiy == false || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || entity is IAuditLog)
                        continue;

                    AuditEntry auditEntry = new AuditEntry(entry)
                    {
                        TableName = entry.Metadata.GetDefaultTableName()
                    };

                    auditEntry.Action = entry.State switch
                    {
                        EntityState.Added => AuditActions.Added,
                        EntityState.Deleted => AuditActions.Deleted,
                        EntityState.Modified => AuditActions.Modified,
                        _ => throw new NotImplementedException()
                    };

                    ProcessProperties(entry, auditEntry);

                    if (auditEntry.NewValues.Count > 0 || auditEntry.Action != AuditActions.Modified)
                    {
                        auditLogsToAdd.Add(auditEntry.ToAudit());
                    }
                }

                return auditLogsToAdd;
            }

            static void ProcessProperties(EntityEntry entry, AuditEntry auditEntry)
            {
                foreach (PropertyEntry property in entry.Properties)
                {
                    IProperty metadata = property.Metadata;
                    if (metadata.IsPrimaryKey())
                    {
                        auditEntry.Key = (Guid)property.CurrentValue;

                        if (entry.State == EntityState.Added)
                            auditEntry.NewValues[metadata.Name] = property.CurrentValue;

                        continue;
                    }

                    if (property.IsModified || entry.State == EntityState.Added)
                    {
                        object[] x = metadata.PropertyInfo.GetCustomAttributes(typeof(AuditIgnoreAttribute), false);
                        if (x.Length == 0)
                            auditEntry.NewValues[metadata.Name] = property.CurrentValue;
                    }
                }
            }
        }

        private class AuditEntry
        {
            public AuditEntry(EntityEntry entry)
            {
                Entry = entry;
            }

            public EntityEntry Entry { get; } = default!;
            public string TableName { get; set; } = default!;
            public Guid Key { get; set; } = default!;
            public AuditActions Action { get; set; }
            public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

            public EntityAuditLog ToAudit()
            {
                EntityAuditLog audit = new EntityAuditLog
                {
                    TableName = TableName,
                    Key = Key,
                    Action = Action,
                    Values = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)
                };
                return audit;
            }
        }
    }
}
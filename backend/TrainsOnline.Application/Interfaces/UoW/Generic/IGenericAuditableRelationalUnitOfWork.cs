﻿namespace TrainsOnline.Application.Interfaces.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces.Repository;

    public interface IGenericAuditableRelationalUnitOfWork : IGenericRelationalUnitOfWork
    {
        IEntityAuditLogsRepository EntityAuditLogs { get; }

        int SaveChangesWithoutAudit();
        Task<int> SaveChangesWithoutAuditAsync(CancellationToken cancellationToken = default);
    }
}

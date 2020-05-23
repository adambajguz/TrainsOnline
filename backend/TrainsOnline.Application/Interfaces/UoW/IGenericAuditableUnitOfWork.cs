namespace TrainsOnline.Application.Interfaces.UoW
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces.Repository;

    public interface IGenericAuditableUnitOfWork : IGenericUnitOfWork
    {
        IEntityAuditLogRepository EntityAuditLogRepository { get; }

        int SaveChangesWithoutAudit();
        Task<int> SaveChangesWithoutAuditAsync(CancellationToken cancellationToken = default);
    }
}

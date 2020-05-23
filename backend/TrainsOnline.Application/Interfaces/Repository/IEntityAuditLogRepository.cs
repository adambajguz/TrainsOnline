namespace TrainsOnline.Application.Interfaces.Repository
{
    using Application.Interfaces.Repository.Generic;
    using Domain.Entities.Audit;

    public interface IEntityAuditLogRepository : IGenericRepository<EntityAuditLog>
    {

    }
}

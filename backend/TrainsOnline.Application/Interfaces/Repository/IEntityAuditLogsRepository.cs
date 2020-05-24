namespace TrainsOnline.Application.Interfaces.Repository
{
    using Application.Interfaces.Repository.Generic;
    using Domain.Entities.Audit;
    using TrainsOnline.Domain.Entities;

    public interface IEntityAuditLogsRepository : IGenericRepository<EntityAuditLog>
    {

    }
}

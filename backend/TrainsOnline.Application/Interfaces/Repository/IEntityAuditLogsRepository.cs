namespace TrainsOnline.Application.Interfaces.Repository
{
    using Application.Interfaces.Repository.Generic;
    using TrainsOnline.Domain.Entities;

    public interface IEntityAuditLogsRepository : IGenericRelationalRepository<EntityAuditLog>
    {

    }
}

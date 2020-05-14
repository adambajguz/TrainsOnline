namespace TrainsOnline.Infrastructure.Repository
{
    using Application.Interfaces;
    using Application.Interfaces.Repository;
    using AutoMapper;
    using TrainsOnline.Domain.Entities.Audit;

    public class EntityAuditLogRepository : GenericRepository<EntityAuditLog>, IEntityAuditLogRepository
    {
        public EntityAuditLogRepository(ICurrentUserService currentUserService, IGenericDatabaseContext context, IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}

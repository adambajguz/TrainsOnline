namespace TrainsOnline.Infrastructure.Repository
{
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.DbContext;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Domain.Entities;

    public class EntityAuditLogsRepository : GenericRepository<EntityAuditLog>, IEntityAuditLogsRepository
    {
        public EntityAuditLogsRepository(ICurrentUserService currentUserService,
                                         IPKPAppDbContext context,
                                         IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}

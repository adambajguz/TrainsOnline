namespace TrainsOnline.Infrastructure.Repository
{
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Domain.Entities;

    public class RouteLogsRepository : GenericMongoRepository<RouteLog>, IRouteLogsRepository
    {
        public RouteLogsRepository(ICurrentUserService currentUserService,
                                   IGenericMongoDatabaseContext context,
                                   IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}

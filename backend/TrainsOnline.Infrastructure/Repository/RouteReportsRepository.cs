namespace TrainsOnline.Infrastructure.Repository
{
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Domain.Entities;

    public class RouteReportsRepository : GenericMongoRepository<RouteReport>, IRouteReportsRepository
    {
        public RouteReportsRepository(ICurrentUserService currentUserService,
                                      IGenericMongoDatabaseContext context,
                                      IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}

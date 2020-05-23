namespace TrainsOnline.Infrastructure.UoW
{
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Infrastructure.Repository;

    public class TrainsOnlineMongoUnitOfWork : GenericMongoUnitOfWork, ITrainsOnlineMongoUnitOfWork
    {
        private IRouteLogsRepository? _routeLogsRepository;
        public IRouteLogsRepository RouteLogsRepository => _routeLogsRepository ?? (_routeLogsRepository = GetSpecificRepository<IRouteLogsRepository, RouteLogsRepository>());

        public TrainsOnlineMongoUnitOfWork(ICurrentUserService currentUserService, ITrainsOnlineMongoDbContext context, IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}

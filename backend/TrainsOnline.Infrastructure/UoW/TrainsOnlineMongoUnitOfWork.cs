namespace TrainsOnline.Infrastructure.UoW
{
    using System;
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Infrastructure.Repository;

    public class TrainsOnlineMongoUnitOfWork : GenericMongoUnitOfWork, ITrainsOnlineMongoUnitOfWork
    {
        private readonly Lazy<IRouteLogsRepository> _routeLogs;
        public IRouteLogsRepository RouteLogs => _routeLogs.Value;

        private readonly Lazy<IRouteReportsRepository> _routeReports;
        public IRouteReportsRepository RouteReports => _routeReports.Value;

        public TrainsOnlineMongoUnitOfWork(ICurrentUserService currentUserService, ITrainsOnlineMongoDbContext context, IMapper mapper) : base(currentUserService, context, mapper)
        {
            _routeLogs = new Lazy<IRouteLogsRepository>(() => GetSpecificRepository<IRouteLogsRepository, RouteLogsRepository>());
            _routeReports = new Lazy<IRouteReportsRepository>(() => GetSpecificRepository<IRouteReportsRepository, RouteReportsRepository>());
        }
    }
}

namespace TrainsOnline.Infrastructure.UoW
{
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Infrastructure.Repository;

    public class TrainsOnlineUnitOfWork : GenericUnitOfWork, ITrainsOnlineUnitOfWork
    {
        private IRoutesRepository? _routesRepository;
        public IRoutesRepository RoutesRepository => _routesRepository ?? (_routesRepository = GetSpecificRepository<IRoutesRepository, RoutesRepository>());

        private IStationsRepository? _stationsRepository;
        public IStationsRepository StationsRepository => _stationsRepository ?? (_stationsRepository = GetSpecificRepository<IStationsRepository, StationsRepository>());

        private ITicketsRepository? _ticketsRepository;
        public ITicketsRepository TicketsRepository => _ticketsRepository ?? (_ticketsRepository = GetSpecificRepository<ITicketsRepository, TicketsRepository>());

        private IUsersRepository? _usersRepository;
        public IUsersRepository UsersRepository => _usersRepository ?? (_usersRepository = GetSpecificRepository<IUsersRepository, UsersRepository>());

        public TrainsOnlineUnitOfWork(ICurrentUserService currentUserService, ITrainsOnlineDbContext context, IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}

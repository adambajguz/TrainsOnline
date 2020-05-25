namespace TrainsOnline.Infrastructure.UoW
{
    using System;
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.DbContext;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Infrastructure.Repository;

    public class TrainsOnlineSQLUnitOfWork : GenericAuditableRelationalUnitOfWork, ITrainsOnlineSQLUnitOfWork
    {
        private readonly Lazy<IRoutesRepository> _routes;
        public IRoutesRepository Routes => _routes.Value;

        private readonly Lazy<IStationsRepository> _stations;
        public IStationsRepository Stations => _stations.Value;

        private readonly Lazy<ITicketsRepository> _tickets;
        public ITicketsRepository Tickets => _tickets.Value;

        private readonly Lazy<IUsersRepository> _users;
        public IUsersRepository Users => _users.Value;

        public TrainsOnlineSQLUnitOfWork(ICurrentUserService currentUserService, ITrainsOnlineRelationalDbContext context, IMapper mapper) : base(currentUserService, context, mapper)
        {
            _routes = new Lazy<IRoutesRepository>(() => GetSpecificRepository<IRoutesRepository, RoutesRepository>());
            _stations = new Lazy<IStationsRepository>(() => GetSpecificRepository<IStationsRepository, StationsRepository>());
            _tickets = new Lazy<ITicketsRepository>(() => GetSpecificRepository<ITicketsRepository, TicketsRepository>());
            _users = new Lazy<IUsersRepository>(() => GetSpecificRepository<IUsersRepository, UsersRepository>());
        }
    }
}

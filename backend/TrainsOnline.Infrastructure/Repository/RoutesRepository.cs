namespace TrainsOnline.Infrastructure.Repository
{
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Domain.Entities;

    public class RoutesRepository : GenericMonogRepository<Route>, IRoutesRepository
    {
        public RoutesRepository(ICurrentUserService currentUserService,
                                ITrainsOnlineDbContext context,
                                IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}

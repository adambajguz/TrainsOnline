namespace TrainsOnline.Infrastructure.Repository
{
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.DbContext;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Domain.Entities;

    public class TicketsRepository : GenericRelationalRepository<Ticket>, ITicketsRepository
    {
        public TicketsRepository(ICurrentUserService currentUserService,
                                 ITrainsOnlineRelationalDbContext context,
                                 IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}

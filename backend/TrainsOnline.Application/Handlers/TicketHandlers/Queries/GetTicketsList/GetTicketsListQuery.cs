namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class GetTicketsListQuery : IRequest<GetTicketsListResponse>
    {
        public GetTicketsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetTicketsListQuery, GetTicketsListResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetTicketsListResponse> Handle(GetTicketsListQuery request, CancellationToken cancellationToken)
            {
                return new GetTicketsListResponse
                {
                    Tickets = await _uow.TicketsRepository.ProjectToAsync<GetTicketsListResponse.TicketLookupModel>(cancellationToken: cancellationToken)
                };
            }
        }
    }
}


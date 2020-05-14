namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW.Generic;

    public class GetTicketsListQuery : IRequest<GetTicketsListResponse>
    {
        public GetTicketsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetTicketsListQuery, GetTicketsListResponse>
        {
            private readonly IPKPAppDbUnitOfWork _uow;

            public Handler(IPKPAppDbUnitOfWork uow)
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


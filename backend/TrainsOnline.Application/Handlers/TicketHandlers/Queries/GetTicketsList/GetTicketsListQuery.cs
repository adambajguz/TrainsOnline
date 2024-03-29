﻿namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketsList
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
            private readonly ITrainsOnlineSQLUnitOfWork _uow;

            public Handler(ITrainsOnlineSQLUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetTicketsListResponse> Handle(GetTicketsListQuery request, CancellationToken cancellationToken)
            {
                return new GetTicketsListResponse
                {
                    Tickets = await _uow.Tickets.ProjectToAsync<GetTicketsListResponse.TicketLookupModel>(cancellationToken: cancellationToken)
                };
            }
        }
    }
}


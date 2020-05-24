namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetUserTicketsList
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketsList;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class GetUserTicketsListQuery : IRequest<GetUserTicketsListResponse>
    {
        public IdRequest Data { get; }

        public GetUserTicketsListQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetUserTicketsListQuery, GetUserTicketsListResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IDataRightsService _drs;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IDataRightsService drs)
            {
                _uow = uow;
                _drs = drs;
            }

            public async Task<GetUserTicketsListResponse> Handle(GetUserTicketsListQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;
                Guid userId = data.Id;
                await _drs.ValidateUserId(data, x => x.Id);

                return new GetUserTicketsListResponse
                {
                    Tickets = await _uow.TicketsRepository.ProjectToWithRelatedAsync<GetUserTicketsListResponse.UserTicketLookupModel, Route>(x => x.Route, x => x.UserId == userId, cancellationToken: cancellationToken)
                };
            }
        }
    }
}


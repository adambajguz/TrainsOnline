﻿namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Queries.GetRouteLogsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Application.Interfaces.UoW.Generic;
    using TrainsOnline.Domain.Entities;

    public class GetRouteLogsListQuery : IRequest<GetRouteLogsListResponse>
    {
        public GetRouteLogsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetRouteLogsListQuery, GetRouteLogsListResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetRouteLogsListResponse> Handle(GetRouteLogsListQuery request, CancellationToken cancellationToken)
            {
                return new GetRouteLogsListResponse
                {
                    RouteLogs = await _uow.RouteLogsRepository.ProjectToAsync<GetRouteLogsListResponse.RouteLookupModel>(cancellationToken: cancellationToken)
                };
            }
        }
    }
}

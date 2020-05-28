namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Queries.GetRouteLogsListByRouteId
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class GetRouteLogsListByRouteIdQuery : IRequest<GetRouteLogsListByRouteIdResponse>
    {
        public IdRequest Data { get; }

        public GetRouteLogsListByRouteIdQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetRouteLogsListByRouteIdQuery, GetRouteLogsListByRouteIdResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetRouteLogsListByRouteIdResponse> Handle(GetRouteLogsListByRouteIdQuery request, CancellationToken cancellationToken)
            {
                return new GetRouteLogsListByRouteIdResponse
                {
                    RouteLogs = await _uow.RouteLogs.ProjectToAsync<GetRouteLogsListByRouteIdResponse.RouteLogByRouteIdLookupModel>(x => x.RouteId == request.Data.Id)
                };
            }
        }
    }
}


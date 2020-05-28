namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Queries.GetRouteReportsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class GetRouteReportsListQuery : IRequest<GetRouteReportsListResponse>
    {
        public GetRouteReportsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetRouteReportsListQuery, GetRouteReportsListResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetRouteReportsListResponse> Handle(GetRouteReportsListQuery request, CancellationToken cancellationToken)
            {
                return new GetRouteReportsListResponse
                {
                    RouteReports = await _uow.RouteLogs.ProjectToAsync<GetRouteReportsListResponse.RouteLogLookupModel>()
                };
            }
        }
    }
}


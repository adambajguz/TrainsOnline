namespace TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Queries.GetRouteReportsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class GetAnalyticsRecordsListQuery : IRequest<GetAnalyticsRecordsListResponse>
    {
        public GetAnalyticsRecordsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetAnalyticsRecordsListQuery, GetAnalyticsRecordsListResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetAnalyticsRecordsListResponse> Handle(GetAnalyticsRecordsListQuery request, CancellationToken cancellationToken)
            {
                return new GetAnalyticsRecordsListResponse
                {
                    AnalyticsRecords = await _uow.AnalyticsRecords.ProjectToAsync<GetAnalyticsRecordsListResponse.AnalyticsRecordLookupModel>()
                };
            }
        }
    }
}


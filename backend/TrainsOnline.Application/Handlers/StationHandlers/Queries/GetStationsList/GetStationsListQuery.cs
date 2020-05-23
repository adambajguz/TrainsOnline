namespace TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class GetStationsListQuery : IRequest<GetStationsListResponse>
    {
        public GetStationsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetStationsListQuery, GetStationsListResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetStationsListResponse> Handle(GetStationsListQuery request, CancellationToken cancellationToken)
            {
                return new GetStationsListResponse
                {
                    Stations = await _uow.StationsRepository.ProjectToAsync<GetStationsListResponse.StationLookupModel>(cancellationToken: cancellationToken)
                };
            }
        }
    }
}


namespace TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW.Generic;

    public class GetStationsListQuery : IRequest<GetStationsListResponse>
    {
        public GetStationsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetStationsListQuery, GetStationsListResponse>
        {
            private readonly IPKPAppDbUnitOfWork _uow;

            public Handler(IPKPAppDbUnitOfWork uow)
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


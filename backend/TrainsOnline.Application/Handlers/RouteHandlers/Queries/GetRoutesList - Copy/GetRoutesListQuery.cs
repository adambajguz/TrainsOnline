namespace TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRoutesList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class GetRoutesListQuery : IRequest<GetRoutesListResponse>
    {
        public GetRoutesListQuery()
        {

        }

        public class Handler : IRequestHandler<GetRoutesListQuery, GetRoutesListResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetRoutesListResponse> Handle(GetRoutesListQuery request, CancellationToken cancellationToken)
            {
                return new GetRoutesListResponse
                {
                    Routes = await _uow.RoutesRepository.ProjectToWithRelatedAsync<GetRoutesListResponse.RouteLookupModel, Station, Station>(relatedSelector0: x => x.From,
                                                                                                                                             relatedSelector1: x => x.To,
                                                                                                                                             cancellationToken: cancellationToken)
                };
            }
        }
    }
}


namespace TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetFilteredRoutesList
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRoutesList;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class GetFilteredRoutesListQuery : IRequest<GetRoutesListResponse>
    {
        public GetFilteredRoutesListRequest Data { get; }

        public GetFilteredRoutesListQuery(GetFilteredRoutesListRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetFilteredRoutesListQuery, GetRoutesListResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IStringSimilarityComparerService _strComparer;

            public Handler(ITrainsOnlineSQLUnitOfWork uow,
                           IStringSimilarityComparerService strComparer)
            {
                _uow = uow;
                _strComparer = strComparer;
            }

            public async Task<GetRoutesListResponse> Handle(GetFilteredRoutesListQuery request, CancellationToken cancellationToken)
            {
                GetFilteredRoutesListRequest data = request.Data;

                List<GetRoutesListResponse.RouteLookupModel> list = await _uow.Routes.ProjectToWithRelatedAsync<GetRoutesListResponse.RouteLookupModel, Station, Station>(relatedSelector0: x => x.From,
                                                                                                                                             relatedSelector1: x => x.To,
                                                                                                                                             //filter: (x) => data.MaximumTicketPrice == null || x.TicketPrice <= data.MaximumTicketPrice,
                                                                                                                                             cancellationToken: cancellationToken);

                if (data.MaximumTicketPrice is double max)
                    list.RemoveAll(x => x.TicketPrice > max);

                if (data.FromPattern is string from)
                    list.RemoveAll(x => _strComparer.AreNotSimilar(x.From.Name, from));

                if (data.ToPattern is string to)
                    list.RemoveAll(x => _strComparer.AreNotSimilar(x.To.Name, to));

                return new GetRoutesListResponse
                {
                    Routes = list
                };
            }
        }
    }
}


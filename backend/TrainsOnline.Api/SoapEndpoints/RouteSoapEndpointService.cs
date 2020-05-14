namespace TrainsOnline.Api.SoapEndpoints
{
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Api.SoapEndpoints.Interfaces;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.RouteHandlers.Commands.CreateRoute;
    using TrainsOnline.Application.Handlers.RouteHandlers.Commands.DeleteRoute;
    using TrainsOnline.Application.Handlers.RouteHandlers.Commands.UpdateRoute;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetFilteredRoutesList;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRouteDetails;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRoutesList;

    [SoapRoute("[baseUrl]/route", "Route", "Create, update, and get route")]
    public class RouteSoapEndpointService : IRouteSoapEndpointService
    {
        protected IMediator Mediator { get; }

        public RouteSoapEndpointService(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<IdResponse> CreateRoute(CreateRouteRequest route)
        {
            return await Mediator.Send(new CreateRouteCommand(route));
        }

        public async Task<GetRouteDetailsResponse> GetRouteDetails(IdRequest id)
        {
            return await Mediator.Send(new GetRouteDetailsQuery(id));
        }

        public async Task<Unit> UpdateRoute(UpdateRouteRequest route)
        {
            return await Mediator.Send(new UpdateRouteCommand(route));
        }

        public async Task<Unit> DeleteRoute(IdRequest id)
        {
            return await Mediator.Send(new DeleteRouteCommand(id));
        }

        public async Task<GetRoutesListResponse> GetFilteredRoutesList(GetFilteredRoutesListRequest data)
        {
            return await Mediator.Send(new GetFilteredRoutesListQuery(data));
        }

        public async Task<GetRoutesListResponse> GetRoutesList()
        {
            return await Mediator.Send(new GetRoutesListQuery());
        }
    }
}

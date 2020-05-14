namespace TrainsOnline.Api.SoapEndpoints.Interfaces
{
    using System.ServiceModel;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.CustomMiddlewares.Soap;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.RouteHandlers.Commands.CreateRoute;
    using TrainsOnline.Application.Handlers.RouteHandlers.Commands.UpdateRoute;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetFilteredRoutesList;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRouteDetails;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRoutesList;
    using TrainsOnline.Domain.Jwt;

    [ServiceContract]
    public interface IRouteSoapEndpointService : ISoapEndpointService
    {
        [SoapAuthorize(Roles = Roles.Admin)]
        [OperationContract]
        Task<IdResponse> CreateRoute(CreateRouteRequest route);

        [OperationContract]
        Task<GetRouteDetailsResponse> GetRouteDetails(IdRequest id);

        [SoapAuthorize(Roles = Roles.Admin)]
        [OperationContract]
        Task<Unit> UpdateRoute(UpdateRouteRequest route);

        [SoapAuthorize(Roles = Roles.Admin)]
        [OperationContract]
        Task<Unit> DeleteRoute(IdRequest id);

        [OperationContract]
        Task<GetRoutesListResponse> GetFilteredRoutesList(GetFilteredRoutesListRequest data);

        [OperationContract]
        Task<GetRoutesListResponse> GetRoutesList();
    }
}
namespace TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Desktop.Domain.DTO.Route;

    public interface IRouteData
    {
        //Task<RouteDetailsValueObject> GetRoute(Guid id);
        //Task<IList<RouteDetailsValueObject>> GetRoutes();

        Task<GetRouteDetailsResponse> GetRoute(Guid id);
        Task<GetRoutesListResponse> GetFilteredRoutes(GetFilteredRoutesListRequest data);
        Task<GetRoutesListResponse> GetRoutes();
    }
}

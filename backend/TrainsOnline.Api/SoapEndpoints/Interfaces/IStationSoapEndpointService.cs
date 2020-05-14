namespace TrainsOnline.Api.SoapEndpoints.Interfaces
{
    using System.ServiceModel;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.CustomMiddlewares.Soap;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.StationHandlers.Commands.CreateStation;
    using TrainsOnline.Application.Handlers.StationHandlers.Commands.UpdateStation;
    using TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationDetails;
    using TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationsList;
    using TrainsOnline.Domain.Jwt;

    [ServiceContract]
    public interface IStationSoapEndpointService : ISoapEndpointService
    {
        [SoapAuthorize(Roles = Roles.Admin)]
        [OperationContract]
        Task<IdResponse> CreateStation(CreateStationRequest station);

        [OperationContract]
        Task<GetStationDetailsResponse> GetStationDetails(IdRequest id);

        [SoapAuthorize(Roles = Roles.Admin)]
        [OperationContract]
        Task<Unit> UpdateStation(UpdateStationRequest station);

        [SoapAuthorize(Roles = Roles.Admin)]
        [OperationContract]
        Task<Unit> DeleteStation(IdRequest id);

        [OperationContract]
        Task<GetStationsListResponse> GetStationsList();
    }
}
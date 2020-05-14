namespace TrainsOnline.Api.SoapEndpoints
{
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Api.SoapEndpoints.Interfaces;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.StationHandlers.Commands.CreateStation;
    using TrainsOnline.Application.Handlers.StationHandlers.Commands.DeleteStation;
    using TrainsOnline.Application.Handlers.StationHandlers.Commands.UpdateStation;
    using TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationDetails;
    using TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationsList;

    [SoapRoute("[baseUrl]/station", "Station", "Create, update, and get station")]
    public class StationSoapEndpointService : IStationSoapEndpointService
    {
        protected IMediator Mediator { get; }

        public StationSoapEndpointService(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<IdResponse> CreateStation(CreateStationRequest station)
        {
            return await Mediator.Send(new CreateStationCommand(station));
        }

        public async Task<GetStationDetailsResponse> GetStationDetails(IdRequest id)
        {
            return await Mediator.Send(new GetStationDetailsQuery(id));
        }

        public async Task<Unit> UpdateStation(UpdateStationRequest station)
        {
            return await Mediator.Send(new UpdateStationCommand(station));
        }

        public async Task<Unit> DeleteStation(IdRequest id)
        {
            return await Mediator.Send(new DeleteStationCommand(id));
        }

        public async Task<GetStationsListResponse> GetStationsList()
        {
            return await Mediator.Send(new GetStationsListQuery());
        }
    }
}

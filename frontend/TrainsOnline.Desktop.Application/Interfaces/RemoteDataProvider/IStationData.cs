namespace TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Desktop.Domain.DTO.Station;

    public interface IStationData
    {
        //Task<StationDetailsValueObject> GetStation(Guid id);
        //Task<IList<StationDetailsValueObject>> GetStations();

        Task<GetStationDetailsResponse> GetStation(Guid id);
        Task<GetStationsListResponse> GetStations();
    }
}

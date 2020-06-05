namespace TrainsOnline.Tracker.Application.DTO.Station
{
    public class CreateStationRequest : IDataTransferObject
    {
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

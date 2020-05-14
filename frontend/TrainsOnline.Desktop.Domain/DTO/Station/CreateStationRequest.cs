namespace TrainsOnline.Desktop.Domain.DTO.Station
{
    using TrainsOnline.Desktop.Domain.DTO;

    public class CreateStationRequest : IDataTransferObject
    {
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

namespace TrainsOnline.Desktop.Domain.DTO.Station
{
    using System;
    using TrainsOnline.Desktop.Domain.DTO;

    public class UpdateStationRequest : IDataTransferObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

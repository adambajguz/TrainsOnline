namespace TrainsOnline.Tracker.Application.DTO.Station
{
    using System;

    public class UpdateStationRequest : IDataTransferObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

namespace TrainsOnline.Tracker.Application.DTO.RouteLog
{
    using System;
    using TrainsOnline.Tracker.Application.DTO;

    public class RouteLogLookupModel : IDataTransferObject
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }
        public Guid RouteId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Speed { get; set; }
    }
}

namespace TrainsOnline.Tracker.Application.DTO.RouteLog
{
    using System;

    public class CreateRouteLogRequest : IDataTransferObject
    {
        public DateTime Timestamp { get; set; }
        public Guid RouteId { get; set; }

        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;

        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Speed { get; set; }
    }
}

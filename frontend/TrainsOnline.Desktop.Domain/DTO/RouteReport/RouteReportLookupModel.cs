namespace TrainsOnline.Desktop.Domain.DTO.RouteReport
{
    using System;

    public class RouteReportLookupModel : IDataTransferObject
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public Guid RouteId { get; set; }

        public string FromName { get; set; }
        public string ToName { get; set; }

        public double VoltageMean { get; set; }
        public double CurrentMean { get; set; }
        public double PowerMean { get; set; }
        public double SpeedMean { get; set; }
        public TimeSpan Duration { get; set; }
        public int NumberOfStops { get; set; }
        public double StopDurationMean { get; set; }
        public double StopDurationMax { get; set; }
    }
}

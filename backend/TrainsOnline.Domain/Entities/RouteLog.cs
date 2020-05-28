namespace TrainsOnline.Domain.Entities
{
    using System;
    using TrainsOnline.Domain.Abstractions.Base;

    public class RouteLog : IBaseMongoEntity, IEntityInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public DateTime Timestamp { get; set; }
        public Guid RouteId { get; set; }

        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;

        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Speed { get; set; }
    }
}

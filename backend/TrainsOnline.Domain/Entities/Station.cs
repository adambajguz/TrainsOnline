namespace TrainsOnline.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Domain.Abstractions.Audit;
    using TrainsOnline.Domain.Abstractions.Base;

    public class Station : IBaseEntity, IEntityInfo, IAuditableEntitiy
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = default!;

        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;

        public ICollection<Route> Departures { get; set; } = default!;
        public ICollection<Route> Arrivals { get; set; } = default!;
    }
}

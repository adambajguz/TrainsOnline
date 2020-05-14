namespace TrainsOnline.Domain.Entities
{
    using System;
    using TrainsOnline.Domain.Abstractions.Audit;
    using TrainsOnline.Domain.Abstractions.Base;

    public class Route : IBaseEntity, IEntityInfo, IAuditableEntitiy
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public Guid FromId { get; set; }
        public Guid ToId { get; set; }

        public virtual Station From { get; set; } = default!;
        public virtual Station To { get; set; } = default!;

        public DateTime DepartureTime { get; set; } = default!;
        public TimeSpan Duration { get; set; } = default!;
        public double Distance { get; set; }
        public double TicketPrice { get; set; }
    }
}

namespace TrainsOnline.Domain.Entities
{
    using System;
    using TrainsOnline.Domain.Abstractions.Audit;
    using TrainsOnline.Domain.Abstractions.Base;

    public class Ticket : IBaseEntity, IEntityInfo, IAuditableEntitiy
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public Guid UserId { get; set; }
        public Guid RouteId { get; set; }

        public virtual Route Route { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}

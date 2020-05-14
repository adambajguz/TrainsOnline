namespace TrainsOnline.Domain.Abstractions.Base
{
    using System;

    public interface IEntityLastSaved
    {
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }
    }
}
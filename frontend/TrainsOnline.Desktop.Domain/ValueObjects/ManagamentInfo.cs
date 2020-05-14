namespace TrainsOnline.Desktop.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.ValueObjects.Base;

    public sealed class ManagamentInfo : ValueObject
    {
        public DateTime CreatedOn { get; }
        public Guid? CreatedBy { get; }
        public DateTime LastSavedOn { get; }
        public Guid? LastSavedBy { get; }

        public ManagamentInfo(DateTime createdOn, Guid? createdBy, DateTime lastSavedOn, Guid? lastSavedBy)
        {
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            LastSavedOn = lastSavedOn;
            LastSavedBy = lastSavedBy;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CreatedOn;
            yield return CreatedBy;
            yield return LastSavedOn;
            yield return LastSavedBy;
        }
    }
}

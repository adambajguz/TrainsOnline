namespace TrainsOnline.Desktop.Domain.ValueObjects.TicketComponents
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.ValueObjects.Base;

    public class TicketDetailsValueObject : ValueObject
    {
        public Guid Id { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}

namespace TrainsOnline.Desktop.Domain.ValueObjects.TicketComponents
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.ValueObjects.Base;

    public class NewTicket : ValueObject
    {
        public Guid UserId { get; set; }
        public Guid RouteId { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
            yield return RouteId;
        }
    }
}

namespace TrainsOnline.Desktop.Domain.Aggregates
{
    using System;
    using TrainsOnline.Desktop.Domain.ValueObjects;
    using TrainsOnline.Desktop.Domain.ValueObjects.Base;

    public class Ticket : IAggregateRoot, IGlobbalyIdentifiable
    {
        public Guid Id { get; }

        public ManagamentInfo Info { get; }
    }
}

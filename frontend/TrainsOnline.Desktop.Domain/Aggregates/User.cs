namespace TrainsOnline.Desktop.Domain.Aggregates
{
    using System;
    using TrainsOnline.Desktop.Domain.ValueObjects;
    using TrainsOnline.Desktop.Domain.ValueObjects.Base;
    using TrainsOnline.Desktop.Domain.ValueObjects.UserComponents;

    public class User : IAggregateRoot, IGlobbalyIdentifiable
    {
        public Guid Id { get; }

        public ManagamentInfo Info { get; }
        public Credentials Credentials { get; }
        public DeliveryInfo Delivery { get; }

        //public IList<Ticket> GetTickets()
        //{

        //}
    }
}

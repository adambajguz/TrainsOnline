namespace TrainsOnline.Desktop.Domain.ValueObjects.Base
{
    using System;

    public interface IGlobbalyIdentifiable
    {
        Guid Id { get; }
    }
}

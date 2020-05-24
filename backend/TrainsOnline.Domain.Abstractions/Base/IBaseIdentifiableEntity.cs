namespace TrainsOnline.Domain.Abstractions.Base
{
    using System;

    public interface IBaseIdentifiableEntity
    {
        Guid Id { get; set; }
    }
}

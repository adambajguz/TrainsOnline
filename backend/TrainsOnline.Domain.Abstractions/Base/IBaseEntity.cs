namespace TrainsOnline.Domain.Abstractions.Base
{
    using System;

    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }
}
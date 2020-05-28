namespace TrainsOnline.Common.Cache
{
    using System;

    public interface ICacheEntry : ICacheEntryConfig
    {
        DateTimeOffset? LastSetOn { get; }
        Guid SynchronizationId { get; }
        object? Value { get; set; }
    }
}
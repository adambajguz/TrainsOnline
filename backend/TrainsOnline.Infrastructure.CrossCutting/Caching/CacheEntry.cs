namespace TrainsOnline.Infrastructure.CrossCutting.Caching
{
    using TrainsOnline.Common.Cache;
    using System;

    public sealed class CacheEntry : CacheEntryConfig, ICacheEntry
    {
        private object? _value;

        //
        // Summary:
        //     Gets or set the value of the cache entry.
        public object? Value
        {
            get => _value;
            set
            {
                _value = value;
                SynchronizationId = Guid.NewGuid();
            }
        }

        //
        // Summary:
        //     Gets the value of synchronization id.
        public Guid SynchronizationId { get; private set; } = Guid.Empty;

        //
        // Summary:
        //     Gets last set in cache date for the cache entry.
        public DateTimeOffset? LastSetOn { get; internal set; }

        public CacheEntry()
        {

        }

        public CacheEntry(ICacheEntryConfig cacheEntryConfig)
        {
            Key = cacheEntryConfig.Key;
            ExtendedKey = cacheEntryConfig.ExtendedKey;
            ExtendedKeyMode = cacheEntryConfig.ExtendedKeyMode;
            AbsoluteExpiration = cacheEntryConfig.AbsoluteExpiration;
            AbsoluteExpirationRelativeToNow = cacheEntryConfig.AbsoluteExpirationRelativeToNow;
            SlidingExpiration = cacheEntryConfig.SlidingExpiration;
        }
    }
}
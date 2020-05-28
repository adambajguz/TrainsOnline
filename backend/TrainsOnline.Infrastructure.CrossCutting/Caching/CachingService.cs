namespace TrainsOnline.Infrastructure.Cache
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Serilog;
    using TrainsOnline.Common.Cache;
    using TrainsOnline.Common.Interfaces;
    using TrainsOnline.Infrastructure.CrossCutting.Caching;
    using TrainsOnline.Infrastructure.CrossCutting.Caching.Providers;

    public class CachingService : ICachingService
    {
        private readonly TimeSpan DEFUALT_TIMEOUT = TimeSpan.FromMinutes(3);

        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        public ISampleCacheProvider Provider { get; }

        public CachingService()
        {
            // Provider = new SampleCacheProvider();
        }

        #region Unsynchronized / Thread unsafe
        #region Synchronous
        public ICacheEntry Get(string key, object? extendedKey = null, CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            string fullKey = CacheEntryConfig.GetFullKey(key, extendedKey, extendedKeyMode);

            return Provider.GetObject<CacheEntry>(fullKey);
        }

        public ICacheEntry GetWithFullKey(string fullKey)
        {
            if (fullKey is null)
                throw new ArgumentNullException(nameof(fullKey));

            return Provider.GetObject<CacheEntry>(fullKey);
        }

        public void Set(ICacheEntryConfig entryConfig, object? value)
        {
            Set(new CacheEntry(entryConfig)
            {
                Value = value
            });
        }

        public void Set(ICacheEntry entry)
        {
            ValidateEntryConfig(entry);

            DateTimeOffset now = DateTimeOffset.Now;
            (entry as CacheEntry)!.LastSetOn = now;

            DateTime? absoluteExpiration = entry.AbsoluteExpiration?.DateTime ?? now.Add(entry.AbsoluteExpirationRelativeToNow ?? TimeSpan.Zero).DateTime;

            string fullKey = entry.GetFullKey();
            Provider.SetObject(fullKey, entry, entry.SlidingExpiration ?? TimeSpan.Zero, absoluteExpiration ?? DateTime.MinValue);

            Log.Information($"{nameof(CachingService) }.Set with key: {fullKey} and SyncId {entry?.SynchronizationId} (null? {entry?.Value is null})", this);
        }

        public T? GetOrSet<T>(ICacheEntryConfig entryConfig, Func<T> createValue)
            where T : class
        {
            ValidateGetOrSet(entryConfig, createValue);

            string fullKey = entryConfig.GetFullKey();

            CacheEntry cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
            {
                // Key not in cache, so get data.
                value = createValue();
                Set(entryConfig, value);
            }

            return value;
        }
        #endregion

        #region Asynchronous
        public async Task<T?> GetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<Task<T>> createValue)
            where T : class
        {
            ValidateGetOrSet(entryConfig, createValue);

            string fullKey = entryConfig.GetFullKey();

            CacheEntry cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
            {
                // Key not in cache, so get data.
                value = await createValue();
                Set(entryConfig, value);
            }

            return value;
        }
        #endregion
        #endregion

        //
        // Summary:
        //     Use Synchronized... when:
        //         When the creation time of an item has some sort of cost, and you want to minimize creations as much as possible.
        //         When the creation time of an item is very long.
        //         When the creation of an item has to be ensured to be done once per key.
        //     Don’t use  Synchronized... when:
        //         There’s no danger of multiple threads accessing the same cache item.
        //         You don’t mind creating the item more than once. For example, if one extra trip to the database won’t change much.
        #region Synchronized / Thread safe
        #region Synchronous
        public T? SynchronizedGet<T>(string key,
                                    object? extendedKey = null,
                                    CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode,
                                    TimeSpan? timeout = null)
            where T : class
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            string fullKey = CacheEntryConfig.GetFullKey(key, extendedKey, extendedKeyMode);

            CacheEntry cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
                if (_locks.TryGetValue(fullKey, out SemaphoreSlim? myLock))
                {
                    myLock.Wait(timeout ?? DEFUALT_TIMEOUT);

                    cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
                    value = cacheEntry.GetValue<T>();
                }

            return value;
        }

        public T? SynchronizedGetOrSet<T>(ICacheEntryConfig entryConfig, Func<T> createValue, TimeSpan? timeout = null)
            where T : class
        {
            ValidateGetOrSet(entryConfig, createValue);

            string fullKey = entryConfig.GetFullKey();

            CacheEntry cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
            {
                SemaphoreSlim myLock = _locks.GetOrAdd(fullKey, k => new SemaphoreSlim(1, 1));

                myLock.Wait(timeout ?? DEFUALT_TIMEOUT);
                try
                {
                    cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
                    if (!cacheEntry.TryGetValue(out value))
                    {
                        value = createValue();
                        Set(entryConfig, value);
                    }
                }
                finally
                {
                    myLock.Release();
                }
            }

            return value;
        }
        #endregion

        #region Asynchronous
        public async Task<T?> SynchronizedGetAsync<T>(string key,
                                                     object? extendedKey = null,
                                                     CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode,
                                                     TimeSpan? timeout = null)
            where T : class
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            string fullKey = CacheEntryConfig.GetFullKey(key, extendedKey, extendedKeyMode);

            CacheEntry cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
                if (_locks.TryGetValue(fullKey, out SemaphoreSlim? myLock))
                {
                    await myLock.WaitAsync(timeout ?? DEFUALT_TIMEOUT);

                    cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
                    value = cacheEntry.GetValue<T>();
                }

            return value;
        }

        private async Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig,
                                                           Func<Task<T>>? createValueAsync,
                                                           Func<T>? createValue,
                                                           TimeSpan? timeout)
            where T : class
        {
            if (entryConfig is null)
                throw new ArgumentNullException(nameof(entryConfig));

            if (createValueAsync is null && createValue is null)
                throw new ArgumentNullException(nameof(createValueAsync) + " && " + nameof(createValue));

            string fullKey = entryConfig.GetFullKey();

            CacheEntry cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
            {
                SemaphoreSlim myLock = _locks.GetOrAdd(fullKey, k => new SemaphoreSlim(1, 1));

                await myLock.WaitAsync(timeout ?? DEFUALT_TIMEOUT);
                try
                {
                    cacheEntry = Provider.GetObject<CacheEntry>(fullKey);
                    if (!cacheEntry.TryGetValue(out value))
                    {
                        // Key not in cache, so get data.
                        if (createValueAsync == null)
                            value = createValue!();
                        else
                            value = await createValueAsync();

                        Set(entryConfig, value);
                    }
                }
                finally
                {
                    myLock.Release();
                }
            }

            return value;
        }

        public async Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<Task<T>> createValueAsync, TimeSpan? timeout = null)
            where T : class
        {
            return await SynchronizedGetOrSetAsync(entryConfig, createValueAsync, null, timeout);
        }

        public async Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<T> createValue, TimeSpan? timeout = null)
            where T : class
        {
            return await SynchronizedGetOrSetAsync(entryConfig, null, createValue, timeout);
        }
        #endregion
        #endregion

        private IQueryable<string> GetAllKeysQueryable()
        {
            return Provider.GetCacheKeys().AsQueryable();
        }

        public string[] GetAllNotExtendedKeys()
        {
            return GetAllKeysQueryable().Where(x => !x.Contains(CacheEntryConfig.ExtendedKeySeperator)).ToArray();
        }

        public string[] GetAllExtendedKeys()
        {
            return GetAllKeysQueryable().Where(x => x.Contains(CacheEntryConfig.ExtendedKeySeperator)).ToArray();
        }

        #region Validation
        private static void ValidateEntryConfig(ICacheEntryConfig entryConfig)
        {
            if (entryConfig is null)
                throw new ArgumentNullException(nameof(entryConfig));

            if (string.IsNullOrWhiteSpace(entryConfig.Key))
                throw new ArgumentNullException(nameof(entryConfig));

            if (entryConfig.AbsoluteExpiration is null && entryConfig.AbsoluteExpirationRelativeToNow is null && entryConfig.SlidingExpiration is null)
                throw new ArgumentNullException("One of: AbsoluteExpiration, AbsoluteExpirationRelativeToNow or SlidingExpiration must be defined!");
        }

        private static void ValidateGetOrSet<T>(ICacheEntryConfig entryConfig, Func<T> createValue) where T : class
        {
            if (entryConfig is null)
                throw new ArgumentNullException(nameof(entryConfig));

            if (createValue is null)
                throw new ArgumentNullException(nameof(createValue));
        }
        #endregion
    }
}
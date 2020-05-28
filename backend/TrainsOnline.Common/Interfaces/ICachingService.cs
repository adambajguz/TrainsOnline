namespace TrainsOnline.Common.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Common.Cache;

    public interface ICachingService
    {
        #region Unsynchronized / Thread unsafe
        #region Synchronous
        ICacheEntry Get(string key, object? extendedKey = null, CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode);
        ICacheEntry GetWithFullKey(string fullKey);
        void Set(ICacheEntryConfig entryConfig, object value);
        void Set(ICacheEntry entry);

        T? GetOrSet<T>(ICacheEntryConfig entryConfig, Func<T> createValue)
            where T : class;
        #endregion

        #region Asynchronous
        Task<T?> GetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<Task<T>> createValue)
            where T : class;
        #endregion
        #endregion

        //
        // Summary:
        //     Use Synchronized... when:
        //         When the creation time of an item has some sort of cost, and you want to minimize creations as much as possible.
        //         When the creation time of an item is very long.
        //         When the creation of an item has to be ensured to be done once per key.
        //     Don’t use Synchronized... when:
        //         There’s no danger of multiple threads accessing the same cache item.
        //         You don’t mind creating the item more than once. For example, if one extra trip to the database won’t change much.
        #region Synchronized / Thread safe
        #region Synchronous
        T? SynchronizedGetOrSet<T>(ICacheEntryConfig entryConfig, Func<T> createValue, TimeSpan? timeout = null)
            where T : class;
        #endregion

        #region Asynchronous
        Task<T?> SynchronizedGetAsync<T>(string key,
                                         object? extendedKey = null,
                                         CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode,
                                         TimeSpan? timeout = null)
            where T : class;
        Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<Task<T>> createValueAsync, TimeSpan? timeout = null)
            where T : class;
        Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<T> createValue, TimeSpan? timeout = null)
            where T : class;
        #endregion
        #endregion

        string[] GetAllExtendedKeys();
        string[] GetAllNotExtendedKeys();
    }
}
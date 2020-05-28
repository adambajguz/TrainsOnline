namespace TrainsOnline.Infrastructure.CrossCutting.Caching.Providers
{
    using System;
    using System.Collections.Generic;

    public interface ISampleCacheProvider
    {
        int Count { get; }
        bool Enabled { get; set; }
        long MaxSize { get; set; }
        string Name { get; }
        long RemainingSpace { get; }
        long Size { get; }

        string GetString(string key);
        void SetString(string key, string value);
        void SetString(string key, string value, DateTime absoluteExpiration);

        object GetObject(string key);
        T GetObject<T>(string key) where T : class;
        void SetObject(string key, object value);
        void SetObject(string key, object value, TimeSpan slidingExpiration);
        void SetObject(string key, object value, DateTime absoluteExpiration);
        void SetObject(string key, object value, TimeSpan slidingExpiration, DateTime absoluteExpiration);

        void Clear();
        void Scavenge();
        void Remove(string key);
        ICollection<string> Remove(Predicate<string> predicate);
        void RemovePrefix(string prefix);
        void RemoveKeysContaining(string value);

        bool ContainsKey(string key);
        string[] GetCacheKeys();
    }
}

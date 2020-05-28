//namespace TrainsOnline.Infrastructure.CrossCutting.Caching.Providers
//{
//    using System;
//    using System.Collections.Generic;

//    internal sealed class SampleCacheProvider : CustomCache, ISampleCacheProvider
//    {
//        int ISampleCacheProvider.Count => InnerCache.Count;
//        long ISampleCacheProvider.MaxSize
//        {
//            get => InnerCache.MaxSize;
//            set => InnerCache.MaxSize = value;
//        }
//        long ISampleCacheProvider.RemainingSpace => InnerCache.RemainingSpace;
//        long ISampleCacheProvider.Size => InnerCache.Size;

//        public SampleCacheProvider(ICache innerCache) : base(innerCache)
//        {

//        }

//        public SampleCacheProvider() : base(name, maxSize)
//        {

//        }

//        #region Object
//        object ISampleCacheProvider.GetObject(string key)
//        {
//            return GetObject(key);
//        }

//        T ISampleCacheProvider.GetObject<T>(string key)
//        {
//            return GetObject(key) as T;
//        }

//        void ISampleCacheProvider.SetObject(string key, object value)
//        {
//            SetObject(key, value);
//        }

//        void ISampleCacheProvider.SetObject(string key, object value, TimeSpan slidingExpiration)
//        {
//            InnerCache.Add(key, value, slidingExpiration);
//        }

//        void ISampleCacheProvider.SetObject(string key, object value, DateTime absoluteExpiration)
//        {
//            InnerCache.Add(key, value, absoluteExpiration);
//        }

//        void ISampleCacheProvider.SetObject(string key, object value, TimeSpan slidingExpiration, DateTime absoluteExpiration)
//        {
//            InnerCache.Add(key, value, slidingExpiration, absoluteExpiration);
//        }
//        #endregion

//        #region String
//        string ISampleCacheProvider.GetString(string key)
//        {
//            return GetString(key);
//        }

//        void ISampleCacheProvider.SetString(string key, string value)
//        {
//            SetString(key, value);
//        }

//        void ISampleCacheProvider.SetString(string key, string value, DateTime absoluteExpiration)
//        {
//            SetString(key, value, absoluteExpiration);
//        }
//        #endregion

//        void ISampleCacheProvider.Scavenge()
//        {
//            InnerCache.Scavenge();
//        }

//        void ISampleCacheProvider.Remove(string key)
//        {
//            Remove(key);
//        }

//        ICollection<string> ISampleCacheProvider.Remove(Predicate<string> predicate)
//        {
//            return InnerCache.Remove(predicate);
//        }

//        bool ISampleCacheProvider.ContainsKey(string key)
//        {
//            return InnerCache.ContainsKey(key);
//        }

//        string[] ISampleCacheProvider.GetCacheKeys()
//        {
//            return InnerCache.GetCacheKeys();
//        }
//    }
//}
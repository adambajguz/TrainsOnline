namespace TrainsOnline.Tracker.Common.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (action is null) throw new ArgumentNullException(nameof(action));

            foreach (T item in source)
                action(item);
        }
    }
}

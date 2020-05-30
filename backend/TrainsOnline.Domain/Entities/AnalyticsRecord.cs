namespace TrainsOnline.Domain.Entities
{
    using System;
    using TrainsOnline.Domain.Abstractions.Base;

    public class AnalyticsRecord : IBaseMongoEntity
    {
        public Guid Id { get; set; }

        public long Hash { get; set; }

        public DateTime Timestamp { get; set; }
        public string Uri { get; set; } = default!;
        public string UserAgent { get; set; } = default!;
        public ulong Visits { get; set; }
    }
}

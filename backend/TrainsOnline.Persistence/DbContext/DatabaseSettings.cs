namespace TrainsOnline.Persistence.DbContext
{
    public sealed class DatabaseSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}

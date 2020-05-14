namespace TrainsOnline.Desktop.Common.Extensions
{
    using System;
    using CSharpVitamins;

    public static class GuidExtensions
    {
        public static ShortGuid ToShortGuid(this Guid guid)
        {
            return new ShortGuid(guid);
        }
    }
}

namespace TrainsOnline.Tracker.Common.Extensions
{
    using System.Linq;

    public static class StringExtensions
    {
        public static string RemoveAllWhitespaces(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}

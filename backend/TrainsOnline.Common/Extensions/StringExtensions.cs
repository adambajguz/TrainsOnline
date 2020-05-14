namespace TrainsOnline.Common.Extensions
{
    using System.Linq;

    public static class StringExtensions
    {
        public static string RemoveWhitespaces(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}

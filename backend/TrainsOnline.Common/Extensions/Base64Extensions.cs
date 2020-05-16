namespace TrainsOnline.Common.Extensions
{
    using System.Threading.Tasks;

    public static class Base64Extensions
    {
        public static async Task<byte[]> DecodeBase64BinaryAsync(this byte[] base64Data)
        {
            if (base64Data is null)
                throw new System.ArgumentNullException(nameof(base64Data));

            byte[] decodedData = await Task.Run(() =>
            {
                string str = System.Text.Encoding.UTF8.GetString(base64Data);
                return System.Convert.FromBase64String(str);

            }).ConfigureAwait(false);

            return decodedData;
        }

        public static async Task<byte[]> DecodeBase64Async(this string base64Data)
        {
            if (string.IsNullOrWhiteSpace(base64Data))
                throw new System.ArgumentException("message", nameof(base64Data));

            byte[] decodedData = await Task.Run(() =>
            {
                return System.Convert.FromBase64String(base64Data);

            }).ConfigureAwait(false);

            return decodedData;
        }
    }
}

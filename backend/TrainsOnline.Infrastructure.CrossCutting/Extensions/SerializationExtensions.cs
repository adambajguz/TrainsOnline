namespace TrainsOnline.Infrastructure.CrossCutting.Extensions
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class Serialization
    {
        public static byte[] ToByteArray(this object? obj)
        {
            if (obj == null)
                return new byte[0];

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);

                return memoryStream.ToArray();
            }
        }
        public static T? FromByteArray<T>(this byte[] byteArray)
            where T : class
        {
            if (byteArray == null)
                return default;

            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }

    }
}
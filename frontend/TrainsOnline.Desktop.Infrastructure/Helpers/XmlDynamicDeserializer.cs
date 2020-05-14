namespace TrainsOnline.Desktop.Infrastructure.Helpers
{
    using System.Dynamic;
    using System.IO;
    using System.Xml.Serialization;

    public class XmlDeserializer : DynamicObject
    {
        public class Serializer
        {
            public T Deserialize<T>(string input) where T : class
            {
                XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

                using (StringReader sr = new StringReader(input))
                {
                    return (T)ser.Deserialize(sr);
                }
            }

            public string Serialize<T>(T ObjectToSerialize)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                    return textWriter.ToString();
                }
            }
        }
    }
}

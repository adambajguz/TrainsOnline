namespace TrainsOnline.Infrastructure.Documents
{
    using System.IO;
    using TrainsOnline.Application.Interfaces.Documents;

    public class DocumentsService : IDocumentsService
    {
        public DocumentsService()
        {

        }

        public IDocumentBuilder NewDocument()
        {
            return new DocumentBuilder();
        }

        public IDocumentBuilder EditDocument(Stream stream)
        {
            return new DocumentBuilder(stream);
        }

        public IDocumentBuilder EditDocument(string path)
        {
            return new DocumentBuilder(path);
        }
    }
}

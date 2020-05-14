namespace TrainsOnline.Application.Interfaces.Documents
{
    using System.IO;

    public interface IDocumentsService
    {
        public IDocumentBuilder NewDocument();
        public IDocumentBuilder EditDocument(Stream stream);
        public IDocumentBuilder EditDocument(string path);
    }
}

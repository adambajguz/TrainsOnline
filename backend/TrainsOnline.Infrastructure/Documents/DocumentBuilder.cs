namespace TrainsOnline.Infrastructure.Documents
{
    using System.IO;
    using GemBox.Document;
    using TrainsOnline.Application.Interfaces.Documents;

    internal class DocumentBuilder : IDocumentBuilder
    {
        private DocumentModel Document { get; } = new DocumentModel();

        //https://www.gemboxsoftware.com/document/examples/c-sharp-vb-net-word-pdf-library/801
        public DocumentBuilder()
        {
            Document = new DocumentModel();
        }

        public DocumentBuilder(Stream stream)
        {
            Document = DocumentModel.Load(stream);
        }

        public DocumentBuilder(string path)
        {
            Document = DocumentModel.Load(path);
        }

        public IDocumentSectionBuilder AddSection()
        {
            Section section = new Section(Document);
            Document.Sections.Add(section);

            return new DocumentSectionBuilder(Document, this, section);
        }

        public byte[] BuildDocx()
        {
            return Build(SaveOptions.DocxDefault);
        }

        public byte[] BuildHtml()
        {
            return Build(SaveOptions.HtmlDefault);
        }

        public byte[] BuildImage()
        {
            return Build(SaveOptions.ImageDefault);
        }

        public byte[] BuildPdf()
        {
            return Build(SaveOptions.PdfDefault);
        }

        public byte[] BuildRtf()
        {
            return Build(SaveOptions.RtfDefault);
        }

        public byte[] BuildTxt()
        {
            return Build(SaveOptions.TxtDefault);
        }

        public byte[] BuildXml()
        {
            return Build(SaveOptions.XmlDefault);
        }

        public byte[] BuildXps()
        {
            return Build(SaveOptions.XpsDefault);
        }

        private byte[] Build(SaveOptions options)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Document.Save(stream, options);
                byte[] array = stream.ToArray();

                return array;
            }
        }
    }
}

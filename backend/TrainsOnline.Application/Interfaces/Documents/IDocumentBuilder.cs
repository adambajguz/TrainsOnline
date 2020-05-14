namespace TrainsOnline.Application.Interfaces.Documents
{
    public interface IDocumentBuilder
    {
        IDocumentSectionBuilder AddSection();

        byte[] BuildDocx();
        byte[] BuildPdf();
        byte[] BuildXps();
        byte[] BuildImage();
        byte[] BuildHtml();
        byte[] BuildTxt();
        byte[] BuildRtf();
        byte[] BuildXml();
    }
}

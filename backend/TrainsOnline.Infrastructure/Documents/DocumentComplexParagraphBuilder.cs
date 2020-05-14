namespace TrainsOnline.Infrastructure.Documents
{
    using System.IO;
    using GemBox.Document;
    using TrainsOnline.Application.Interfaces.Documents;

    internal class DocumentComplexParagraphBuilder : IDocumentComplexParagraphBuilder
    {
        private DocumentModel Document { get; }
        private DocumentSectionBuilder Parent { get; }
        private Paragraph Paragraph { get; }

        public DocumentComplexParagraphBuilder(DocumentModel document, DocumentSectionBuilder parent, Paragraph paragraph)
        {
            Document = document;
            Parent = parent;
            Paragraph = paragraph;
        }

        public IDocumentComplexParagraphBuilder AddRun(string text,
                                                       double size = 10,
                                                       bool bold = false,
                                                       bool italic = false,
                                                       System.Drawing.Color? fontColor = null)
        {
            Run run = new Run(Document, text)
            {
                CharacterFormat = {
                    Size = size,
                    Bold = bold,
                    Italic = italic,
                    FontColor = new Color(fontColor?.R ?? 0, fontColor?.G ?? 0, fontColor?.B ?? 0),
                    FontName = "Courier New" //sudo apt-get install ttf-mscorefonts-installer
                }
            };

            Paragraph.Inlines.Add(run);
            return this;
        }

        public IDocumentComplexParagraphBuilder AddRunLine(string text,
                                                           double size = 10,
                                                           bool bold = false,
                                                           bool italic = false,
                                                           System.Drawing.Color? fontColor = null)
        {
            AddRun(text, size, bold, italic, fontColor);
            AddNewLine();

            return this;
        }

        public IDocumentComplexParagraphBuilder AddNewLine(int count = 1)
        {
            for (int i = 0; i < count; ++i)
            {
                SpecialCharacter run = new SpecialCharacter(Document, SpecialCharacterType.LineBreak);
                Paragraph.Inlines.Add(run);
            }

            return this;
        }

        public IDocumentComplexParagraphBuilder AddSpecialCharacter(DocumentSpecialCharacters specialCharacter)
        {
            SpecialCharacter run = new SpecialCharacter(Document, (SpecialCharacterType)specialCharacter);
            Paragraph.Inlines.Add(run);

            return this;
        }

        public IDocumentComplexParagraphBuilder AddImage(string imagePath,
                                                         double width,
                                                         double height)
        {
            Picture picture = new Picture(Document, imagePath, width, height, LengthUnit.Millimeter);
            Paragraph.Inlines.Add(picture);

            return this;
        }

        public IDocumentComplexParagraphBuilder AddImage(Stream imageStream,
                                                         DocumentImageFormats format,
                                                         double width,
                                                         double height)
        {
            Picture picture = new Picture(Document, imageStream, (PictureFormat)format, width, height, LengthUnit.Millimeter);
            Paragraph.Inlines.Add(picture);

            return this;
        }

        public IDocumentComplexParagraphBuilder AddImage(MemoryStream imageStream,
                                                         DocumentImageFormats format,
                                                         double width,
                                                         double height)
        {
            Picture picture = new Picture(Document, imageStream, (PictureFormat)format, width, height, LengthUnit.Millimeter);
            Paragraph.Inlines.Add(picture);

            return this;
        }

        public IDocumentSectionBuilder FinishParagraph()
        {
            return Parent;
        }
    }
}

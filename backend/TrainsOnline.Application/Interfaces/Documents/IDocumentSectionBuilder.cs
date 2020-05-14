namespace TrainsOnline.Application.Interfaces.Documents
{
    using System;

    public interface IDocumentSectionBuilder
    {
        IDocumentSectionBuilder AddParagraph(string text);
        IDocumentComplexParagraphBuilder AddComplexParagraph();

        IDocumentSectionBuilder AddSimpleTable(object[,] tableData, bool hasHeader = true);
        IDocumentSectionBuilder AddMultiColumn(int columnsCount, double paddingHorizontal, params Action<IDocumentComplexParagraphBuilder>[] actions);

        IDocumentBuilder FinishSection();
    }
}

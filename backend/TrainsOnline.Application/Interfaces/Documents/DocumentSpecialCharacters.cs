namespace TrainsOnline.Application.Interfaces.Documents
{
    public enum DocumentSpecialCharacters
    {
        //
        // Summary:
        //     Specifies that the document content shall restart on the next line.
        LineBreak = 0,
        //
        // Summary:
        //     Specifies that the document content shall restart on the next page.
        PageBreak = 1,
        //
        // Summary:
        //     Specifies that the document content shall restart on the next column available
        //     on the current page.
        ColumnBreak = 2,
        //
        // Summary:
        //     Specifies that the position of the current line of text will advance to the next
        //     GemBox.Document.TabStop location which is further along than the starting location
        //     of the tab or to nearest multiple of the default tab stop width.
        Tab = 3,
        //
        // Summary:
        //     Specifies that the current position holds the reference to footnote or endnote
        //     element.
        NoteMark = 4,
        //
        // Summary:
        //     Specifies the location of footnote or endnote area separator.
        Separator = 5,
        //
        // Summary:
        //     Specifies the location of footnote or endnote area continuation separator.
        ContinuationSeparator = 6
    }
}

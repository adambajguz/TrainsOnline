namespace TrainsOnline.Infrastructure.Documents
{
    using System;
    using GemBox.Document;
    using GemBox.Document.Tables;
    using TrainsOnline.Application.Interfaces.Documents;

    internal class DocumentSectionBuilder : IDocumentSectionBuilder
    {
        private const string SimpleTableStyleName = "Simple Table";
        private const string InvisibileTableStyleName = "Invisible Table";

        private DocumentModel Document { get; }
        private DocumentBuilder Parent { get; }
        private Section Section { get; }

        public DocumentSectionBuilder(DocumentModel document, DocumentBuilder parent, Section section)
        {
            Document = document;
            Parent = parent;
            Section = section;
        }

        public IDocumentSectionBuilder AddParagraph(string text)
        {
            Paragraph paragraph = new Paragraph(Document, text);
            Section.Blocks.Add(paragraph);

            return this;
        }

        public IDocumentComplexParagraphBuilder AddComplexParagraph()
        {
            Paragraph paragraph = new Paragraph(Document);
            Section.Blocks.Add(paragraph);

            return new DocumentComplexParagraphBuilder(Document, this, paragraph);
        }

        public IDocumentSectionBuilder AddSimpleTable(object[,] tableData, bool hasHeader = true)
        {
            TableStyle? customTableStyle = null;
            if (Document.Styles.Contains(SimpleTableStyleName))
                customTableStyle = Document.Styles[SimpleTableStyleName] as TableStyle;

            if (customTableStyle is null)
            {
                // Create and add a custom table style.
                customTableStyle = new TableStyle(SimpleTableStyleName);

                // Set table style format.
                Color borderColor = new Color(57, 89, 158);
                Color headerBackgroundColor = new Color(68, 136, 138);

                customTableStyle.ParagraphFormat.Alignment = HorizontalAlignment.Left;
                customTableStyle.CharacterFormat.FontColor = Color.Black;
                customTableStyle.TableFormat.Borders.SetBorders(MultipleBorderTypes.Left | MultipleBorderTypes.Right | MultipleBorderTypes.InsideVertical, BorderStyle.Dashed, borderColor, 1);
                customTableStyle.CellFormat.Borders.SetBorders(MultipleBorderTypes.Outside, BorderStyle.Dashed, borderColor, 1);
                customTableStyle.CellFormat.Padding = new Padding(2, LengthUnit.Millimeter);

                // Set table style conditional format for first row.
                TableStyleFormat firstRowFormat = customTableStyle.ConditionalFormats[TableStyleFormatType.FirstRow];
                firstRowFormat.CharacterFormat.Bold = true;
                firstRowFormat.CharacterFormat.FontColor = Color.White;
                firstRowFormat.ParagraphFormat.Alignment = HorizontalAlignment.Left;
                firstRowFormat.CellFormat.Borders.SetBorders(MultipleBorderTypes.Outside, BorderStyle.Dashed, borderColor, 1);
                firstRowFormat.CellFormat.BackgroundColor = headerBackgroundColor;
                firstRowFormat.CellFormat.Padding = new Padding(2, LengthUnit.Millimeter);

                // Set table style conditional format for last row.
                TableStyleFormat lastRowFormat = customTableStyle.ConditionalFormats[TableStyleFormatType.LastRow];
                lastRowFormat.CharacterFormat.FontColor = Color.Black;
                lastRowFormat.CellFormat.Borders.SetBorders(MultipleBorderTypes.Bottom | MultipleBorderTypes.Left | MultipleBorderTypes.Right, BorderStyle.Dashed, borderColor, 1);
                lastRowFormat.CellFormat.Padding = new Padding(2, LengthUnit.Millimeter);

                Document.Styles.Add(customTableStyle);
            }

            //Create table
            Table table = new Table(Document);
            table.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            table.TableFormat.Style = customTableStyle;
            table.TableFormat.StyleOptions = TableStyleOptions.FirstRow | TableStyleOptions.LastRow;

            Section.Blocks.Add(table);

            int rowsCount = tableData.GetLength(0);
            int columnsCount = tableData.GetLength(1);

            for (int r = 0; r < rowsCount; ++r)
            {
                // Create a row and add it to table.
                TableRow row = new TableRow(Document);
                table.Rows.Add(row);

                for (int c = 0; c < columnsCount; ++c)
                {
                    // Create a cell and add it to row.
                    TableCell cell = new TableCell(Document);
                    row.Cells.Add(cell);

                    // Create a paragraph and add it to cell.
                    Paragraph paragraph = new Paragraph(Document, tableData[r, c].ToString());
                    cell.Blocks.Add(paragraph);
                }
            }

            return this;
        }

        public IDocumentSectionBuilder AddMultiColumn(int columnsCount, double paddingHorizontal, params Action<IDocumentComplexParagraphBuilder>[] actions)
        {
            if (actions.Length != columnsCount)
                throw new ArgumentException($"Parameter {nameof(actions)}  should have length equal to value provided in {nameof(columnsCount)}");

            TableStyle? customTableStyle = null;
            if (Document.Styles.Contains(InvisibileTableStyleName))
                customTableStyle = Document.Styles[InvisibileTableStyleName] as TableStyle;

            if (customTableStyle is null)
            {
                // Create and add a custom table style.
                customTableStyle = new TableStyle(InvisibileTableStyleName);

                // Set table style format.
                customTableStyle.ParagraphFormat.Alignment = HorizontalAlignment.Left;
                customTableStyle.CharacterFormat.FontColor = Color.Black;
                customTableStyle.CellFormat.Borders.ClearBorders();
                customTableStyle.CellFormat.Padding = new Padding(paddingHorizontal, 0, LengthUnit.Millimeter);

                Document.Styles.Add(customTableStyle);
            }

            //Create table
            Table table = new Table(Document);
            table.TableFormat.PreferredWidth = new TableWidth(100, TableWidthUnit.Percentage);
            table.TableFormat.Style = customTableStyle;

            Section.Blocks.Add(table);

            // Create a row and add it to table.
            TableRow row = new TableRow(Document);
            table.Rows.Add(row);

            for (int c = 0; c < columnsCount; ++c)
            {
                // Create a cell and add it to row.
                TableCell cell = new TableCell(Document);
                row.Cells.Add(cell);

                // Create a paragraph and add it to cell.
                Paragraph paragraph = new Paragraph(Document);
                cell.Blocks.Add(paragraph);

                DocumentComplexParagraphBuilder complexParagraph = new DocumentComplexParagraphBuilder(Document, this, paragraph);
                actions[c].Invoke(complexParagraph);
            }

            return this;
        }

        public IDocumentBuilder FinishSection()
        {
            return Parent;
        }
    }
}

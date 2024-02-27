using ClosedXML.Excel;

namespace Cts.AppServices.Utilities;

public static class DataExport
{
    public static MemoryStream AsExcelStream<T>(this IEnumerable<T> records, string sheetName,
        bool hideLastColumn = false)
    {
        using var xlWorkbook = new XLWorkbook();

        var xlWorksheet = xlWorkbook.AddWorksheet(sheetName);
        xlWorksheet.Cell(row: 1, column: 1).InsertTable(records).Cells().Style
            .Alignment.SetWrapText(true)
            .Alignment.SetVertical(XLAlignmentVerticalValues.Top);
        xlWorksheet.Columns().AdjustToContents(minWidth: 8d, maxWidth: 80d);
        if (hideLastColumn) xlWorksheet.Column(xlWorksheet.Columns().Count()).Hide();

        var memoryStream = new MemoryStream();
        xlWorkbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }
}

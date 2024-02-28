using ClosedXML.Excel;

namespace Cts.AppServices.Utilities;

public static class DataExport
{
    /// <summary>
    /// Creates an Excel spreadsheet from <see cref="IEnumerable{T}"/> records. 
    /// </summary>
    /// <param name="records">The records to add to the spreadsheet.</param>
    /// <param name="sheetName">A name for the worksheet.</param>
    /// <param name="deleteLastColumn">A flag indicating whether to delete the final column in the table.</param>
    /// <typeparam name="T">The type of the records being inserted.</typeparam>
    /// <returns>An Excel spreadsheet with a worksheet named <paramref name="sheetName"/> containing the data in
    /// <paramref name="records"/> as a table.</returns>
    public static MemoryStream AsExcelStream<T>(this IEnumerable<T> records, string sheetName,
        bool deleteLastColumn)
    {
        using var xlWorkbook = new XLWorkbook();

        var xlWorksheet = xlWorkbook.AddWorksheet(sheetName);
        var xlTable = xlWorksheet.Cell(row: 1, column: 1).InsertTable(records);
        if (deleteLastColumn) xlTable.Column(xlTable.Columns().Count()).Delete();
        xlTable.Cells().Style
            .Alignment.SetWrapText(true)
            .Alignment.SetVertical(XLAlignmentVerticalValues.Top);
        xlWorksheet.Columns().AdjustToContents(minWidth: 8d, maxWidth: 80d);

        var memoryStream = new MemoryStream();
        xlWorkbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }
}

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace Cts.Domain.DataViews.DataArchiveViews;

public class RecordsCount
{
    public string Table { get; init; } = string.Empty;
    public int Count { get; init; }
    public int Order { get; init; }
}

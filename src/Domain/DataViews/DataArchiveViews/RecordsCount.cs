// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace Cts.Domain.DataViews.DataArchiveViews;

public record RecordsCount(string Table, int Count, int Order);

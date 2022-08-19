using Cts.AppServices.StaffServices;

namespace Cts.AppServices.Offices;

public class OfficeViewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool Active { get; init; }
    public StaffViewDto? MasterUser { get; init; }
}

public class OfficeCreateDto
{
    public string Name { get; init; } = string.Empty;
    public StaffViewDto? MasterUser { get; init; }
}

public class OfficeUpdateDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public StaffViewDto? MasterUser { get; init; }
    public bool Active { get; init; }
}

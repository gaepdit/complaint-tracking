using Cts.AppServices.StaffServices;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cts.Infrastructure.Identity;

public sealed class StaffAppService : IStaffAppService
{
    public async Task<StaffViewDto?> FindAsync(Guid id) => throw new NotImplementedException();

    public async Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter) => throw new NotImplementedException();

    public async Task<IList<string>> GetRolesAsync(Guid id) => throw new NotImplementedException();

    public async Task<IList<CtsRole>> GetCtsRolesAsync(Guid id) => throw new NotImplementedException();

    public async Task<IdentityResult> UpdateRolesAsync(Guid id, Dictionary<string, bool> roles) =>
        throw new NotImplementedException();

    public async Task<IdentityResult> UpdateAsync(StaffUpdateDto resource) => throw new NotImplementedException();

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

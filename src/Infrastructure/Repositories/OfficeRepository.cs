﻿using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Cts.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Repositories;

public sealed class OfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public OfficeRepository(AppDbContext context) : base(context) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.Offices.AsNoTracking().SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
}
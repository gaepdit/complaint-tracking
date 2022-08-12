﻿using Cts.Domain.Entities;
using Cts.Domain.Offices;
using Cts.Domain.Users;
using Cts.TestData.Offices;

namespace Cts.LocalRepository;

public sealed class LocalOfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public LocalOfficeRepository() : base(Data.GetOffices) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Name == name));

    public async Task<List<ApplicationUser>> GetUsersListAsync(Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).Users;
}

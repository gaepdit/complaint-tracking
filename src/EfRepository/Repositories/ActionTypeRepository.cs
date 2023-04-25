﻿using Cts.Domain.Entities.ActionTypes;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Repositories;

/// <inheritdoc cref="IActionTypeRepository" />
public sealed class ActionTypeRepository : BaseRepository<ActionType, Guid>, IActionTypeRepository
{
    public ActionTypeRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<ActionType?> FindByNameAsync(string name, CancellationToken token = default) =>
        await Context.ActionTypes.AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);
}

﻿using Cts.Domain.Complaints;
using Cts.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cts.Infrastructure.Repositories;

public class ComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    public ComplaintRepository(AppDbContext context) : base(context) { }

    public Task<int> GetCountAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default) =>
        Context.Set<Complaint>().AsNoTracking().CountAsync(predicate, token);

    public Task<bool> ExistsAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default) =>
        Context.Set<Complaint>().AsNoTracking().AnyAsync(predicate, token);
}

using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Repositories;

public sealed class UserRepository : BaseReadRepository<ApplicationUser, string>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext) { }

    public new async Task<ApplicationUser> GetAsync(string id, CancellationToken token = new CancellationToken())
    {
        var item = await Context.Users
            .AsTracking()
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);
        
        return item ?? throw new EntityNotFoundException(typeof(ApplicationUser), id);
    }
}

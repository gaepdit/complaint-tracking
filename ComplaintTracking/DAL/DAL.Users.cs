using System;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static ComplaintTracking.Caching;

namespace ComplaintTracking
{
    public partial class DAL
    {
        public Task<bool> EmailAlreadyUsedAsync(string email, string ignoreId = null)
        {
            if (string.IsNullOrEmpty(ignoreId))
            {
                return _context.Users.AsNoTracking()
                    .AnyAsync(e => e.Email == email);
            }

            return _context.Users.AsNoTracking()
                .AnyAsync(e => e.Email == email && e.Id != ignoreId);
        }

        public async Task<SelectList> GetUsersSelectListAsync(Guid? officeId = null, bool includeInactive = false)
        {
            var items = await _context.Users.AsNoTracking()
                .Where(e => e.OfficeId == officeId)
                .Where(t => t.Active || includeInactive)
                .Include(e => e.Office)
                .OrderBy(t => t.LastName)
                .ThenBy(t => t.FirstName)
                .ToListAsync().ConfigureAwait(false);

            return new SelectList(items, nameof(ApplicationUser.Id), nameof(ApplicationUser.SelectableName));
        }

        public async Task<SelectList> GetAllUsersSelectListAsync(bool includeInactive = false)
        {
            return await _cache.GetOrCreateAsync(
                includeInactive ? CacheKeys.UsersIncludeInactiveSelectList : CacheKeys.UsersSelectList,
                entry =>
                {
                    entry.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(LONG_CACHE_TIME));
                    return UsersSelectList();
                }
            );

            async Task<SelectList> UsersSelectList()
            {
                var items = await _context.Users.AsNoTracking()
                    .Where(t => t.Active || includeInactive)
                    .Include(e => e.Office)
                    .OrderBy(t => t.LastName)
                    .ThenBy(t => t.FirstName)
                    .ToListAsync();
                return new SelectList(items, "Id", nameof(ApplicationUser.SelectableNameWithOffice));
            }
        }

        public async Task<SelectList> GetUsersInRoleSelectListAsync(CtsRole ctsRole, Guid? officeId = null)
        {
            var users = (await _userManager.GetUsersInRoleAsync(ctsRole.ToString()))
                .Where(e => officeId == null || e.OfficeId == officeId);
            return new SelectList(users, nameof(ApplicationUser.Id), nameof(ApplicationUser.SortableFullName));
        }
    }
}

using System;
using System.Collections.Generic;
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
        public Task<bool> OfficeExists(Guid id)
        {
            return _context.LookupOffices.AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }

        public Task<bool> OfficeNameExistsAsync(string name, Guid? ignoreId = null)
        {
            if (ignoreId.HasValue)
            {
                return _context.LookupOffices.AsNoTracking()
                    .AnyAsync(e => e.Name == name && e.Id != ignoreId.Value);
            }

            return _context.LookupOffices.AsNoTracking()
                .AnyAsync(e => e.Name == name);
        }

        public async Task<string> GetOfficeName(Guid? officeId)
        {
            if (!officeId.HasValue)
            {
                return null;
            }

            return (await _context.LookupOffices.AsNoTracking()
                    .Where(e => e.Id == officeId.Value)
                    .SingleOrDefaultAsync())?
                .Name;
        }

        public Task<List<Office>> GetOfficesForMasterAsync(string userId)
        {
            return _context.LookupOffices.AsNoTracking()
                .Where(e => e.MasterUserId == userId)
                .Where(t => t.Active)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<SelectList> GetOfficesSelectListAsync(bool requireMaster = false)
        {
            return await _cache.GetOrCreateAsync(
                requireMaster ? CacheKeys.OfficesSelectListRequireMaster : CacheKeys.OfficesSelectList,
                entry =>
                {
                    entry.SlidingExpiration = EXTRA_LONG_CACHE_TIMESPAN;
                    return officesSelectList();
                }
            );

            async Task<SelectList> officesSelectList()
            {
                var items = await _context.LookupOffices.AsNoTracking()
                    .Where(t => t.Active)
                    .Where(t => t.MasterUserId != null || !requireMaster)
                    .OrderBy(t => t.Name)
                    .ToListAsync();
                return new SelectList(items, "Id", "Name");
            }
        }
    }
}

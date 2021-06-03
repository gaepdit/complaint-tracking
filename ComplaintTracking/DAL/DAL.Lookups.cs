using System;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static ComplaintTracking.Caching;

namespace ComplaintTracking
{
    public partial class DAL
    {
        public async Task<CommonSelectLists> GetCommonSelectListsAsync(Guid? officeId)
        {
            officeId ??= default;

            var areasOfConcern = await GetAreasOfConcernSelectListAsync();
            var offices = await GetOfficesSelectListAsync(true);
            var counties = await GetCountiesSelectListAsync();
            var states = await GetStatesSelectListAsync();
            var officeUsers = await GetUsersSelectListAsync(officeId);
            var allUsers = await GetAllUsersSelectListAsync();

            var lists = new CommonSelectLists()
            {
                AreasOfConcernSelectList = areasOfConcern,
                OfficesSelectList = offices,
                CountiesSelectList = counties,
                StatesSelectList = states,
                UsersInOfficeSelectList = officeUsers,
                AllUsersSelectList = allUsers,
                PhoneTypesSelectList = GetPhoneTypesSelectList(),
            };
            return lists;
        }

        public async Task<SelectList> GetCountiesSelectListAsync()
        {
            return await _cache.GetOrCreateAsync(
                CacheKeys.CountiesSelectList,
                entry =>
                {
                    entry.SlidingExpiration = EXTRA_LONG_CACHE_TIMESPAN;
                    return countiesSelectList();
                });

            async Task<SelectList> countiesSelectList()
            {
                var items = await _context.LookupCounties.AsNoTracking()
                    .OrderBy(t => t.Name)
                    .ToListAsync();
                return new SelectList(items, "Id", "Name");
            }
        }

        public async Task<SelectList> GetStatesSelectListAsync()
        {
            return await _cache.GetOrCreateAsync(
                CacheKeys.StatesSelectList,
                entry =>
                {
                    entry.SlidingExpiration = EXTRA_LONG_CACHE_TIMESPAN;
                    return statesSelectList();
                });

            async Task<SelectList> statesSelectList()
            {
                var items = await _context.LookupStates.AsNoTracking()
                    .OrderBy(t => t.Name)
                    .ToListAsync();
                return new SelectList(items, "Id", "Name");
            }
        }

        private static SelectList GetPhoneTypesSelectList() =>
            new(Enum.GetValues(typeof(PhoneType)));

        public async Task<SelectList> GetAreasOfConcernSelectListAsync()
        {
            return await _cache.GetOrCreateAsync(
                CacheKeys.AreasOfConcernSelectList,
                entry =>
                {
                    entry.SlidingExpiration = EXTRA_LONG_CACHE_TIMESPAN;
                    return concernsSelectList();
                });

            async Task<SelectList> concernsSelectList()
            {
                var items = await _context.LookupConcerns.AsNoTracking()
                    .Where(t => t.Active)
                    .OrderBy(t => t.Name)
                    .ToListAsync();
                return new SelectList(items, "Id", "Name");
            }
        }

        public async Task<SelectList> GetActionTypesSelectListAsync()
        {
            return await _cache.GetOrCreateAsync(
                CacheKeys.ActionTypesSelectList,
                entry =>
                {
                    entry.SlidingExpiration = LONG_CACHE_TIMESPAN;
                    return actionsSelectList();
                });

            async Task<SelectList> actionsSelectList()
            {
                var items = await _context.LookupActionTypes.AsNoTracking()
                    .Where(t => t.Active)
                    .OrderBy(t => t.Name)
                    .ToListAsync();
                return new SelectList(items, "Id", "Name");
            }
        }
    }
}

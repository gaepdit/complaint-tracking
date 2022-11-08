﻿using System;
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
            officeId ??= Guid.Empty;

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
                    return CountiesSelectList();
                });

            async Task<SelectList> CountiesSelectList()
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
                    return StatesSelectList();
                });

            async Task<SelectList> StatesSelectList()
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
                    return ConcernsSelectList();
                });

            async Task<SelectList> ConcernsSelectList()
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
                    return ActionsSelectList();
                });

            async Task<SelectList> ActionsSelectList()
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
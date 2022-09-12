﻿using Cts.Domain.ActionTypes;
using Cts.Domain.Offices;
using Cts.Infrastructure.Contexts;
using Cts.Infrastructure.Repositories;
using Cts.TestData.SeedData;
using Microsoft.EntityFrameworkCore;
using TestSupport.EfHelpers;

namespace IntegrationTests
{
    public sealed class RepositoryHelper : IDisposable
    {
        public CtsDbContext DbContext { get; set; } = null!;

        private readonly DbContextOptions<CtsDbContext> _options = SqliteInMemory.CreateOptions<CtsDbContext>();
        private readonly CtsDbContext _context;

        private RepositoryHelper()
        {
            _context = new CtsDbContext(_options);
            _context.Database.EnsureCreated();
        }

        public static RepositoryHelper CreateRepositoryHelper() => new();

        public void ClearChangeTracker() => _context.ChangeTracker.Clear();

        public IActionTypeRepository GetActionTypeRepository()
        {
            DbSeedDataHelpers.SeedActionTypeData(_context);
            DbContext = new CtsDbContext(_options);
            return new ActionTypeRepository(DbContext);
        }

        public IOfficeRepository GetOfficeRepository()
        {
            DbSeedDataHelpers.SeedOfficeData(_context);
            DbContext = new CtsDbContext(_options);
            return new OfficeRepository(DbContext);
        }

        public void Dispose() => _context.Dispose();
    }
}

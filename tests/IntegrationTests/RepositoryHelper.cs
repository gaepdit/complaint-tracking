using GaEpd.AppLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MyAppRoot.Domain.Offices;
using MyAppRoot.Infrastructure.Contexts;
using MyAppRoot.Infrastructure.Repositories;
using MyAppRoot.TestData.SeedData;
using TestSupport.EfHelpers;

namespace IntegrationTests;

public sealed class RepositoryHelper : IDisposable
{
    private AppDbContext Context { get; set; } = null!;

    private readonly DbContextOptions<AppDbContext> _options = SqliteInMemory.CreateOptions<AppDbContext>();
    private readonly AppDbContext _context;

    private RepositoryHelper()
    {
        _context = new AppDbContext(_options);
        _context.Database.EnsureCreated();
    }

    public static RepositoryHelper CreateRepositoryHelper() => new();

    public void ClearChangeTracker() => _context.ChangeTracker.Clear();

    public async Task ClearTableAsync<TEntity>() where TEntity : AuditableEntity
    {
        _context.RemoveRange(_context.Set<TEntity>());
        await _context.SaveChangesAsync();
        ClearChangeTracker();
    }

    public IOfficeRepository GetOfficeRepository()
    {
        DbSeedDataHelpers.SeedOfficeData(_context);
        Context = new AppDbContext(_options);
        return new OfficeRepository(Context);
    }

    public void Dispose()
    {
        _context.Dispose();
        Context.Dispose();
    }
}

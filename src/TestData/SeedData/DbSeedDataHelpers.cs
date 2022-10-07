using MyAppRoot.Infrastructure.Contexts;
using MyAppRoot.TestData.Identity;
using MyAppRoot.TestData.Offices;

namespace MyAppRoot.TestData.SeedData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        SeedOfficeData(context);
        SeedIdentityData(context);
    }

    public static void SeedOfficeData(AppDbContext context)
    {
        if (context.Offices.Any()) return;
        context.Offices.AddRange(OfficeData.GetOffices);
        context.SaveChanges();
    }

    private static void SeedIdentityData(AppDbContext context)
    {
        if (!context.Roles.Any()) context.Roles.AddRange(IdentityData.GetIdentityRoles);
        if (!context.Users.Any()) context.Users.AddRange(IdentityData.GetUsers);
        if (!context.UserRoles.Any()) context.UserRoles.AddRange(IdentityData.GetUserRoles);
        context.SaveChanges();
    }
}

using Cts.TestData;
using Cts.TestData.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Contexts.SeedDevData;

public static class DbSeedDataHelpers
{
    private const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";

    public static void SeedAllData(AppDbContext context)
    {
        SeedActionTypeData(context);
        SeedConcernData(context);
        SeedOfficeData(context);
        SeedIdentityData(context);
        SeedComplaintData(context);
    }

    public static void SeedActionTypeData(AppDbContext context)
    {
        if (context.ActionTypes.Any()) return;
        context.ActionTypes.AddRange(ActionTypeData.GetActionTypes);
        context.SaveChanges();
    }

    private static void SeedComplaintData(AppDbContext context)
    {
        if (context.Complaints.Any()) return;

        context.Database.BeginTransaction();
        if (context.Database.ProviderName == SqlServerProvider)
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Complaints ON");
        context.Complaints.AddRange(ComplaintData.GetComplaints);
        context.SaveChanges();
        if (context.Database.ProviderName == SqlServerProvider)
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Complaints OFF");
        context.Database.CommitTransaction();
    }

    public static void SeedConcernData(AppDbContext context)
    {
        if (context.Concerns.Any()) return;
        context.Concerns.AddRange(ConcernData.GetConcerns);
        context.SaveChanges();
    }

    public static void SeedOfficeData(AppDbContext context)
    {
        if (context.Offices.Any()) return;
        context.Offices.AddRange(OfficeData.GetOffices);
        context.SaveChanges();
    }

    public static void SeedIdentityData(AppDbContext context)
    {
        var roles = IdentityData.GetRoles.ToList();
        var users = IdentityData.GetUsers.ToList();
        var userRoles = roles
            .Select(role => new IdentityUserRole<string> { RoleId = role.Id, UserId = users.First().Id })
            .ToList();

        if (!context.Roles.Any()) context.Roles.AddRange(roles);
        if (!context.Users.Any()) context.Users.AddRange(users);
        if (!context.UserRoles.Any()) context.UserRoles.AddRange(userRoles);

        OfficeData.SeedOfficeAssignors(context.Offices.AsTracking().ToList(), users);

        context.SaveChanges();
    }
}

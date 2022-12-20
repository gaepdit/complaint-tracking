using Cts.TestData;
using Cts.TestData.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Contexts.SeedDevData;

public static class DbSeedDataHelpers
{
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
        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Complaints ON");
        context.Complaints.AddRange(ComplaintData.GetComplaints);
        context.SaveChanges();
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

    private static void SeedIdentityData(AppDbContext context)
    {
        if (!context.Roles.Any()) context.Roles.AddRange(IdentityData.GetIdentityRoles);
        if (!context.Users.Any()) context.Users.AddRange(IdentityData.GetUsers);
        if (!context.UserRoles.Any()) context.UserRoles.AddRange(IdentityData.GetUserRoles);
        context.SaveChanges();
    }
}

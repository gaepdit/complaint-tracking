using Cts.Domain.Identity;
using Cts.TestData;
using Cts.TestData.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Contexts.SeedDevData;

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
        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Complaints ON");

        context.Complaints.AddRange(ComplaintData.GetComplaints);
        context.SaveChanges();

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Complaints OFF");
        context.Database.CommitTransaction();

        if (!context.Attachments.Any())
            context.Attachments.AddRange(AttachmentData.GetAttachments);
        if (!context.ComplaintActions.Any())
            context.ComplaintActions.AddRange(ComplaintActionData.GetComplaintActions);
        if (!context.ComplaintTransitions.Any())
            context.ComplaintTransitions.AddRange(ComplaintTransitionData.GetComplaintTransitions);

        context.SaveChanges();
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
        // Seed Users
        var users = UserData.GetUsers.ToList();
        if (!context.Users.Any()) context.Users.AddRange(users);

        // Seed Roles
        var roles = UserData.GetRoles.ToList();
        if (!context.Roles.Any()) context.Roles.AddRange(roles);

        // Seed User Roles
        if (!context.UserRoles.Any())
        {
            // -- admin
            var adminUserRoles = roles
                .Select(role => new IdentityUserRole<string>
                    { RoleId = role.Id, UserId = users.Single(e => e.GivenName == "Admin").Id })
                .ToList();
            context.UserRoles.AddRange(adminUserRoles);

            // -- staff
            var staffUserId = users.Single(e => e.GivenName == "General").Id;
            context.UserRoles.AddRange(
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.SiteMaintenance).Id,
                    UserId = staffUserId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.Staff).Id,
                    UserId = staffUserId,
                });
        }

        // Seed Office assignors
        OfficeData.SeedOfficeAssignors(context.Offices.AsTracking().ToList(), users);

        context.SaveChanges();
    }
}

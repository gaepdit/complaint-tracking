using ComplaintTracking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ComplaintTracking.Data
{
    public static partial class SeedTestData
    {
        public static async Task AddUsersAsync(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            string password)
        {
            NewUser[] users =
            {
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "DivManager10",
                    Email = "example10@dnr.ga.gov",
                    OfficeName = "Director's Office",
                    CtsRole = CtsRole.DivisionManager,
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Manager11",
                    Email = "example11@dnr.ga.gov",
                    OfficeName = "Mountain District",
                    CtsRole = CtsRole.Manager,
                    IsMaster = true,
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Staff12",
                    Email = "example12@dnr.ga.gov",
                    OfficeName = "Air Protection Branch",
                    CtsRole = CtsRole.AttachmentsEditor,
                    IsMaster = true,
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Manager13",
                    Email = "example13@dnr.ga.gov",
                    OfficeName = "Air Protection Branch",
                    CtsRole = CtsRole.Manager,
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Manager1",
                    Email = "example1@dnr.ga.gov",
                    OfficeName = "Mountain District",
                    CtsRole = CtsRole.Manager,
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Staff2",
                    Email = "example2@dnr.ga.gov",
                    OfficeName = "Mountain District",
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Staff3",
                    Email = "example3@dnr.ga.gov",
                    OfficeName = "Mountain District",
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Manager4",
                    Email = "example4@dnr.ga.gov",
                    OfficeName = "Air Protection Branch",
                    CtsRole = CtsRole.Manager,
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Staff5",
                    Email = "example5@dnr.ga.gov",
                    OfficeName = "Air Protection Branch",
                },
                new NewUser
                {
                    FirstName = "Sample",
                    LastName = "Staff6",
                    Email = "example6@dnr.ga.gov",
                    OfficeName = "Air Protection Branch",
                },
                new NewUser
                {
                    FirstName = "Inactive",
                    LastName = "Staff7",
                    Email = "example7@dnr.ga.gov",
                    OfficeName = "Air Protection Branch",
                    Active = false,
                },
            };

            foreach (var u in users)
            {
                if (await context.Users.AnyAsync(e => e.Email == u.Email)) continue;

                var user = new ApplicationUser
                {
                    UserName = u.Email,
                    Email = u.Email,
                    EmailConfirmed = true,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Active = u.Active,
                    Office = await context.LookupOffices.FirstOrDefaultAsync(e => e.Name == u.OfficeName),
                };
                await userManager.CreateAsync(user, password);
                var tempUser = await userManager.FindByNameAsync(u.Email);
                if (u.CtsRole.HasValue)
                {
                    await userManager.AddToRoleAsync(tempUser, u.CtsRole.Value.ToString());
                }

                if (!u.IsMaster) continue;

                var office = await context.LookupOffices.FirstOrDefaultAsync(e => e.Name == u.OfficeName);
                if (office != null)
                {
                    office.MasterUserId = tempUser.Id;
                    context.Update(office);
                }

                await context.SaveChangesAsync();
            }
        }

        private sealed class NewUser
        {
            public string FirstName { get; init; }
            public string LastName { get; init; }
            public string OfficeName { get; init; }
            public string Email { get; init; }
            public CtsRole? CtsRole { get; init; }
            public bool IsMaster { get; init; }
            public bool Active { get; init; } = true;
        }
    }
}

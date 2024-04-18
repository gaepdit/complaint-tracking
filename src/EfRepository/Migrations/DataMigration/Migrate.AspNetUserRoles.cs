namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string AspNetUserRoles =
        """
        -- Insert existing roles
        insert into dbo.AspNetUserRoles
            (UserId,
             RoleId)
        select lower(ur.UserId) as UserId,
               lower(ur.RoleId) as RoleId
        from dbo._archive_AspNetUserRoles ur
            inner join dbo.AspNetUsers u
            on lower(u.Id) = lower(ur.UserId);
        
        -- Insert Site Maintenance role
        insert into dbo.AspNetUserRoles
            (UserId,
             RoleId)
        select lower(u.Id)                                                                  as UserId,
               (select lower(t.Id) from dbo.AspNetRoles t where t.Name = 'SiteMaintenance') as RoleId
        from dbo._archive_AspNetUserRoles ur
            inner join dbo.AspNetUsers u
            on lower(u.Id) = lower(ur.UserId)
            inner join dbo.AspNetRoles r
            on lower(r.Id) = lower(ur.RoleId)
        where r.Name = 'DivisionManager';
        
        -- Insert Staff role
        insert into dbo.AspNetUserRoles
            (UserId,
             RoleId)
        select lower(u.Id)                                                        as UserId,
               (select lower(t.Id) from dbo.AspNetRoles t where t.Name = 'Staff') as RoleId
        from dbo.AspNetUsers u
        where u.Active = convert(bit, 1)
          and u.EmailConfirmed = convert(bit, 1);
        """;
}

namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string AspNetRoles =
        """
        insert into dbo.AspNetRoles
            (Id,
             Name,
             NormalizedName,
             ConcurrencyStamp)
        select Id,
               Name,
               NormalizedName,
               ConcurrencyStamp
        from dbo._archive_AspNetRoles;
        """;
}

namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string AspNetRoles =
        """
        insert into AspNetRoles
            (Id,
             Name,
             NormalizedName,
             ConcurrencyStamp)
        select Id,
               Name,
               NormalizedName,
               ConcurrencyStamp
        from _archive_AspNetRoles;
        """;
}

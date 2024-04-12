namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string Concerns =
        """
        insert into dbo.Concerns
            (Id,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById,
             Name,
             Active)
        select Id,
               CreatedDate at time zone 'Eastern Standard Time' as CreatedAt,
               IIF(CreatedById = '00000000-0000-0000-0000-000000000000',
                   null, CreatedById)                           as CreatedById,
               UpdatedDate at time zone 'Eastern Standard Time' as UpdatedAt,
               UpdatedById,
               Name,
               Active
        from dbo._archive_LookupConcerns
        where Active = convert(bit, 1)
        or (Id in (select distinct PrimaryConcernId from dbo._archive_Complaints)
         or Id in (select distinct SecondaryConcernId from dbo._archive_Complaints));
        """;
}

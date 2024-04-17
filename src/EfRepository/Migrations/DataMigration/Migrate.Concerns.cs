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
        select lower(Id)                                        as Id,
               CreatedDate at time zone 'Eastern Standard Time' as CreatedAt,
               dbo.FixUserId(CreatedById)                       as CreatedById,
               UpdatedDate at time zone 'Eastern Standard Time' as UpdatedAt,
               dbo.FixUserId(UpdatedById)                       as UpdatedById,
               trim(Name)                                       as Name,
               Active                                           as Active
        from dbo._archive_LookupConcerns
        where Active = convert(bit, 1)
           or (Id in (select distinct PrimaryConcernId from dbo._archive_Complaints)
            or Id in (select distinct SecondaryConcernId from dbo._archive_Complaints));
        """;
}

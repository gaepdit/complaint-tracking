namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string ActionTypes =
        """
        insert into dbo.ActionTypes
            (Id,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById,
             Name,
             Active)
        select lower(Id),
               CreatedDate at time zone 'Eastern Standard Time'                                    as CreatedAt,
               IIF(CreatedById = '00000000-0000-0000-0000-000000000000', null, lower(CreatedById)) as CreatedById,
               UpdatedDate at time zone 'Eastern Standard Time'                                    as UpdatedAt,
               lower(UpdatedById),
               trim(Name)                                                                          as Name,
               Active
        from dbo._archive_LookupActionTypes
        where Active = convert(bit, 1)
           or lower(Id) in (select lower(ActionTypeId) from dbo._archive_ComplaintActions);
        """;
}

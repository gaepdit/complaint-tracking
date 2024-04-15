namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string Offices =
        """
        with
            cte (cId)
                as (select OfficeId as cId
                    from dbo._archive_AspNetUsers
                    union
                    select CurrentOfficeId as cId
                    from dbo._archive_Complaints
                    union
                    select TransferredToOfficeId as cId
                    from dbo._archive_ComplaintTransitions
                    union
                    select TransferredFromOfficeId as cId
                    from dbo._archive_ComplaintTransitions)
        insert
        into Offices
            (Id,
             AssignorId,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById,
             Name,
             Active)
        select lower(o.Id)                                                                             as Id,
               lower(o.MasterUserId)                                                                   as AssignorId,
               o.CreatedDate at time zone 'Eastern Standard Time'                                      as CreatedAt,
               IIF(o.CreatedById = '00000000-0000-0000-0000-000000000000', null, lower(o.CreatedById)) as CreatedById,
               o.UpdatedDate at time zone 'Eastern Standard Time'                                      as UpdatedAt,
               lower(o.UpdatedById)                                                                    as UpdatedById,
               o.Name,
               o.Active
        from dbo._archive_LookupOffices o
            left join cte
            on lower(o.Id) = lower(cte.cId)
        where cte.cId is not null;
        """;
}

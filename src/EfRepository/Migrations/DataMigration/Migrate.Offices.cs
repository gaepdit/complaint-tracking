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
        into dbo.Offices
            (Id,
             AssignorId,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById,
             Name,
             Active)
        select lower(o.Id)                                        as Id,
               dbo.FixUserId(o.MasterUserId)                      as AssignorId,
               o.CreatedDate at time zone 'Eastern Standard Time' as CreatedAt,
               dbo.FixUserId(o.CreatedById)                       as CreatedById,
               o.UpdatedDate at time zone 'Eastern Standard Time' as UpdatedAt,
               dbo.FixUserId(o.UpdatedById)                       as UpdatedById,
               o.Name                                             as Name,
               o.Active                                           as Active
        from dbo._archive_LookupOffices o
            left join cte
            on lower(o.Id) = lower(cte.cId)
        where cte.cId is not null;
        """;
}

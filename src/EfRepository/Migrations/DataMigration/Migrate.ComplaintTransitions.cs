namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string ComplaintTransitions =
        """
        insert into dbo.ComplaintTransitions
            (Id,
             ComplaintId,
             TransitionType,
             CommittedDate,
             CommittedByUserId,
             TransferredToUserId,
             TransferredToOfficeId,
             Comment,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById)
        select lower(t.Id)                                              as Id,
               t.ComplaintId,
               case
                   when t.TransitionType = 0 then 'New'
                   when t.TransitionType = 1 then 'Assigned'
                   when t.TransitionType = 2 then 'SubmittedForReview'
                   when t.TransitionType = 3 then 'ReturnedByReviewer'
                   when t.TransitionType = 4 then 'Closed'
                   when t.TransitionType = 5 then 'Reopened'
                   when t.TransitionType = 6 then 'Deleted'
                   when t.TransitionType = 7 then 'Restored'
               end                                                      as TransitionType,
               t.DateTransferred at time zone 'Eastern Standard Time'   as CommittedDate,
               lower(t.TransferredByUserId)                             as CommittedByUserId,
               lower(t.TransferredToUserId)                             as TransferredToUserId,
               lower(t.TransferredToOfficeId)                           as TransferredToOfficeId,
               trim(CHAR(13) + CHAR(10) + CHAR(9) + ' ' from t.Comment) as Comment,
               t.CreatedDate at time zone 'Eastern Standard Time'       as CreatedDate,
               lower(t.CreatedById)                                     as CreatedById,
               t.UpdatedDate at time zone 'Eastern Standard Time'       as UpdatedDate,
               lower(t.UpdatedById)                                     as UpdatedById
        from dbo._archive_ComplaintTransitions t
            inner join dbo.Complaints c
            on c.Id = t.ComplaintId;
        
        insert into dbo.ComplaintTransitions
            (Id,
             ComplaintId,
             TransitionType,
             CommittedDate,
             CommittedByUserId,
             TransferredToUserId,
             TransferredToOfficeId,
             Comment,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById)
        select newid()                                             as Id,
               t.ComplaintId,
               'Accepted'                                          as TransitionType,
               t.DateAccepted at time zone 'Eastern Standard Time' as CommittedDate,
               lower(t.TransferredToUserId)                        as CommittedByUserId,
               null                                                as TransferredToUserId,
               null                                                as TransferredToOfficeId,
               null                                                as Comment,
               t.CreatedDate at time zone 'Eastern Standard Time'  as CreatedDate,
               lower(t.CreatedById)                                as CreatedById,
               t.UpdatedDate at time zone 'Eastern Standard Time'  as UpdatedDate,
               lower(t.UpdatedById)                                as UpdatedById
        from dbo._archive_ComplaintTransitions t
            inner join dbo.Complaints c
            on c.Id = t.ComplaintId
        where DateAccepted is not null
        """;
}

namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string ComplaintTransitions =
        """
        insert into ComplaintTransitions
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
        select lower(Id)                                            as Id,
               ComplaintId,
               case
                   when TransitionType = 0 then 'New'
                   when TransitionType = 1 then 'Assigned'
                   when TransitionType = 2 then 'SubmittedForReview'
                   when TransitionType = 3 then 'ReturnedByReviewer'
                   when TransitionType = 4 then 'Closed'
                   when TransitionType = 5 then 'Reopened'
                   when TransitionType = 6 then 'Deleted'
                   when TransitionType = 7 then 'Restored'
               end                                                  as TransitionType,
               DateTransferred at time zone 'Eastern Standard Time' as CommittedDate,
               lower(TransferredByUserId)                           as CommittedByUserId,
               lower(TransferredToUserId)                           as TransferredToUserId,
               lower(TransferredToOfficeId)                         as TransferredToOfficeId,
               trim(Comment)                                        as Comment,
               CreatedDate at time zone 'Eastern Standard Time'     as CreatedDate,
               lower(CreatedById)                                   as CreatedById,
               UpdatedDate at time zone 'Eastern Standard Time'     as UpdatedDate,
               lower(UpdatedById)                                   as UpdatedById
        from _archive_ComplaintTransitions;
        
        insert into ComplaintTransitions
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
        select lower(Id)                                         as Id,
               ComplaintId,
               'Accepted'                                        as TransitionType,
               DateAccepted at time zone 'Eastern Standard Time' as CommittedDate,
               lower(TransferredToUserId)                        as CommittedByUserId,
               null                                              as TransferredToUserId,
               null                                              as TransferredToOfficeId,
               null                                              as Comment,
               CreatedDate at time zone 'Eastern Standard Time'  as CreatedDate,
               lower(CreatedById)                                as CreatedById,
               UpdatedDate at time zone 'Eastern Standard Time'  as UpdatedDate,
               lower(UpdatedById)                                as UpdatedById
        from _archive_ComplaintTransitions
        where DateAccepted is not null;
        """;
}

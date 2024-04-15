namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string ComplaintActions =
        """
        insert into ComplaintActions
            (Id,
             ComplaintId,
             ActionTypeId,
             ActionDate,
             Investigator,
             Comments,
             EnteredDate,
             EnteredById,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById,
             IsDeleted,
             DeletedAt,
             DeletedById)
        select lower(Id)                                        as Id,
               ComplaintId,
               lower(ActionTypeId)                              as ActionTypeId,
               ActionDate at time zone 'Eastern Standard Time'  as ActionDate,
               trim(Investigator)                               as Investigator,
               trim(Comments)                                   as Comments,
               DateEntered at time zone 'Eastern Standard Time' as DateEntered,
               lower(EnteredById)                               as EnteredById,
               CreatedDate at time zone 'Eastern Standard Time' as CreatedDate,
               lower(CreatedById)                               as CreatedById,
               UpdatedDate at time zone 'Eastern Standard Time' as UpdatedDate,
               lower(UpdatedById)                               as UpdatedById,
               Deleted,
               DateDeleted at time zone 'Eastern Standard Time' as DateDeleted,
               lower(DeletedById)                               as DeletedById
        from _archive_ComplaintActions;
        """;
}

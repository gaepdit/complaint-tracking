namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string ComplaintActions =
        """
        insert into dbo.ComplaintActions
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
        select lower(a.Id)                                        as Id,
               a.ComplaintId,
               lower(a.ActionTypeId)                              as ActionTypeId,
               a.ActionDate at time zone 'Eastern Standard Time'  as ActionDate,
               trim(a.Investigator)                               as Investigator,
               trim(isnull(a.Comments, ''))                       as Comments,
               a.DateEntered at time zone 'Eastern Standard Time' as DateEntered,
               lower(a.EnteredById)                               as EnteredById,
               a.CreatedDate at time zone 'Eastern Standard Time' as CreatedDate,
               lower(a.CreatedById)                               as CreatedById,
               a.UpdatedDate at time zone 'Eastern Standard Time' as UpdatedDate,
               lower(a.UpdatedById)                               as UpdatedById,
               a.Deleted,
               a.DateDeleted at time zone 'Eastern Standard Time' as DateDeleted,
               lower(a.DeletedById)                               as DeletedById
        from dbo._archive_ComplaintActions a
            inner join dbo.Complaints c
            on c.Id = a.ComplaintId;
        """;
}

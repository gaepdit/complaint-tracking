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
        select lower(a.Id)                                                           as Id,
               a.ComplaintId                                                         as ComplaintId,
               lower(a.ActionTypeId)                                                 as ActionTypeId,
               a.ActionDate at time zone 'Eastern Standard Time'                     as ActionDate,
               trim(a.Investigator)                                                  as Investigator,
               trim(CHAR(13) + CHAR(10) + CHAR(9) + ' ' from isnull(a.Comments, '')) as Comments,
               a.DateEntered at time zone 'Eastern Standard Time'                    as DateEntered,
               dbo.FixUserId(a.EnteredById)                                          as EnteredById,
               a.CreatedDate at time zone 'Eastern Standard Time'                    as CreatedDate,
               dbo.FixUserId(a.CreatedById)                                          as CreatedById,
               a.UpdatedDate at time zone 'Eastern Standard Time'                    as UpdatedDate,
               dbo.FixUserId(a.UpdatedById)                                          as UpdatedById,
               a.Deleted                                                             as Deleted,
               a.DateDeleted at time zone 'Eastern Standard Time'                    as DateDeleted,
               dbo.FixUserId(a.DeletedById)                                          as DeletedById
        from dbo._archive_ComplaintActions a
            inner join dbo.Complaints c
            on c.Id = a.ComplaintId;
        """;
}

namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string Attachments =
        """
        insert into Attachments
            (Id,
             ComplaintId,
             FileName,
             FileExtension,
             Size,
             UploadedDate,
             UploadedById,
             IsImage,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById,
             IsDeleted,
             DeletedAt,
             DeletedById)
        select lower(Id)                                         as Id,
               ComplaintId,
               trim(FileName)                                    as FileName,
               FileExtension,
               Size,
               DateUploaded at time zone 'Eastern Standard Time' as DateUploaded,
               lower(UploadedById)                               as UploadedById,
               IsImage,
               null                                              as CreatedAt,
               null                                              as CreatedById,
               null                                              as UpdatedAt,
               null                                              as UpdatedById,
               Deleted,
               DateDeleted at time zone 'Eastern Standard Time'  as DateDeleted,
               lower(DeletedById)                                as DeletedById
        from _archive_Attachments;
        """;
}

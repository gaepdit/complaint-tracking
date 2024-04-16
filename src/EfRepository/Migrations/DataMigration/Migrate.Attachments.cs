namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string Attachments =
        """
        insert into dbo.Attachments
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
        select lower(a.Id)                                         as Id,
               a.ComplaintId,
               trim(a.FileName)                                    as FileName,
               a.FileExtension,
               a.Size,
               a.DateUploaded at time zone 'Eastern Standard Time' as DateUploaded,
               lower(a.UploadedById)                               as UploadedById,
               a.IsImage,
               null                                                as CreatedAt,
               null                                                as CreatedById,
               null                                                as UpdatedAt,
               null                                                as UpdatedById,
               a.Deleted,
               a.DateDeleted at time zone 'Eastern Standard Time'  as DateDeleted,
               lower(a.DeletedById)                                as DeletedById
        from dbo._archive_Attachments a
            inner join dbo.Complaints c
            on c.Id = a.ComplaintId
        """;
}

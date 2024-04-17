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
               a.ComplaintId                                       as ComplaintId,
               trim(a.FileName)                                    as FileName,
               a.FileExtension                                     as FileExtension,
               a.Size                                              as Size,
               a.DateUploaded at time zone 'Eastern Standard Time' as DateUploaded,
               dbo.FixUserId(a.UploadedById)                       as UploadedById,
               a.IsImage                                           as IsImage,
               null                                                as CreatedAt,
               null                                                as CreatedById,
               null                                                as UpdatedAt,
               null                                                as UpdatedById,
               a.Deleted                                           as Deleted,
               a.DateDeleted at time zone 'Eastern Standard Time'  as DateDeleted,
               dbo.FixUserId(a.DeletedById)                        as DeletedById
        from dbo._archive_Attachments a
            inner join dbo.Complaints c
            on c.Id = a.ComplaintId;
        """;
}

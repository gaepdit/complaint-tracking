namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string EmailLogs =
        """
        insert into dbo.EmailLogs
            (Id,
             Sender,
             Subject,
             Recipients,
             CopyRecipients,
             TextBody,
             HtmlBody,
             CreatedAt)
        select Id,
               left([From], 200)                             as Sender,
               left(Subject, 200)                            as Subject,
               left([To], 2000)                              as Recipients,
               null                                          as CopyRecipients,
               TextBody,
               HtmlBody,
               DateSent at time zone 'Eastern Standard Time' as CreatedAt
        from dbo._archive_EmailLogs;
        """;
}

namespace Cts.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string AspNetUsers =
        """
        with
            cte (cId)
                as (select CreatedById as cId
                    from dbo._archive_Complaints
                    union
                    select CurrentOwnerId
                    from dbo._archive_Complaints
                    union
                    select DeletedById
                    from dbo._archive_Complaints
                    union
                    select EnteredById
                    from dbo._archive_Complaints
                    union
                    select ReceivedById
                    from dbo._archive_Complaints
                    union
                    select ReviewById
                    from dbo._archive_Complaints
                    union
                    select UpdatedById
                    from dbo._archive_Complaints
                    union
                    select CreatedById
                    from dbo._archive_LookupOffices
                    union
                    select MasterUserId
                    from dbo._archive_LookupOffices
                    union
                    select UpdatedById
                    from dbo._archive_LookupOffices
                    union
                    select CreatedById
                    from dbo._archive_ComplaintTransitions
                    union
                    select TransferredByUserId
                    from dbo._archive_ComplaintTransitions
                    union
                    select TransferredFromUserId
                    from dbo._archive_ComplaintTransitions
                    union
                    select TransferredToUserId
                    from dbo._archive_ComplaintTransitions
                    union
                    select UpdatedById
                    from dbo._archive_ComplaintTransitions
                    union
                    select CreatedById
                    from dbo._archive_ComplaintActions
                    union
                    select DeletedById
                    from dbo._archive_ComplaintActions
                    union
                    select EnteredById
                    from dbo._archive_ComplaintActions
                    union
                    select UpdatedById
                    from dbo._archive_ComplaintActions
                    union
                    select DeletedById
                    from dbo._archive_Attachments
                    union
                    select UploadedById
                    from dbo._archive_Attachments
                    union
                    select CreatedById
                    from dbo._archive_LookupActionTypes
                    union
                    select UpdatedById
                    from dbo._archive_LookupActionTypes
                    union
                    select CreatedById
                    from dbo._archive_LookupConcerns
                    union
                    select UpdatedById
                    from dbo._archive_LookupConcerns)
        insert
        into dbo.AspNetUsers
            (Id,
             GivenName,
             FamilyName,
             OfficeId,
             Active,
             ObjectIdentifier,
             UserName,
             NormalizedUserName,
             Email,
             NormalizedEmail,
             EmailConfirmed,
             PasswordHash,
             SecurityStamp,
             ConcurrencyStamp,
             PhoneNumber,
             PhoneNumberConfirmed,
             TwoFactorEnabled,
             LockoutEnd,
             LockoutEnabled,
             AccessFailedCount)
        select lower(a.Id)               as Id,
               trim(a.FirstName)         as GivenName,
               trim(a.LastName)          as FamilyName,
               lower(a.OfficeId)         as OfficeId,
               a.Active,
               null                      as ObjectIdentifier,
               a.UserName,
               a.NormalizedUserName,
               a.Email,
               a.NormalizedEmail,
               a.EmailConfirmed,
               null                      as PasswordHash,
               a.SecurityStamp,
               a.ConcurrencyStamp,
               nullif(trim(a.Phone), '') as PhoneNumber,
               a.PhoneNumberConfirmed,
               a.TwoFactorEnabled,
               null                      as LockoutEnd,
               convert(bit, 1)           as LockoutEnabled,
               a.AccessFailedCount
        from dbo._archive_AspNetUsers a
            left join cte
            on lower(a.Id) = lower(cte.cId)
        where cte.cId is not null
        """;
}

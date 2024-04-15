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
        select lower(u.Id)                                                                         as Id,
               trim(u.FirstName)                                                                   as GivenName,
               trim(u.LastName)                                                                    as FamilyName,
               lower(u.OfficeId)                                                                   as OfficeId,
               u.Active,
               null                                                                                as ObjectIdentifier,
               replace(replace(u.UserName, '_', '.'), '@dnr.state.ga.us', '@dnr.ga.gov')           as UserName,
               replace(replace(u.NormalizedUserName, '_', '.'), '@DNR.STATE.GA.US', '@DNR.GA.GOV') as NormalizedUserName,
               replace(replace(u.Email, '_', '.'), '@dnr.state.ga.us', '@dnr.ga.gov')              as Email,
               replace(replace(u.NormalizedEmail, '_', '.'), '@DNR.STATE.GA.US', '@DNR.GA.GOV')    as NormalizedEmail,
               u.EmailConfirmed,
               null                                                                                as PasswordHash,
               u.SecurityStamp,
               u.ConcurrencyStamp,
               nullif(trim(u.Phone), '')                                                           as PhoneNumber,
               u.PhoneNumberConfirmed,
               u.TwoFactorEnabled,
               null                                                                                as LockoutEnd,
               convert(bit, 1)                                                                     as LockoutEnabled,
               u.AccessFailedCount
        from dbo._archive_AspNetUsers u
            left join cte
            on lower(u.Id) = lower(cte.cId)
        where cte.cId is not null
        """;
}

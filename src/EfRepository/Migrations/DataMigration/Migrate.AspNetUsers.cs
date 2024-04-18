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
               u.Active                                                                            as Active,
               null                                                                                as ObjectIdentifier,
               replace(replace(u.UserName, '_', '.'), '@dnr.state.ga.us', '@dnr.ga.gov')           as UserName,
               replace(replace(u.NormalizedUserName, '_', '.'), '@DNR.STATE.GA.US', '@DNR.GA.GOV') as NormalizedUserName,
               replace(replace(u.Email, '_', '.'), '@dnr.state.ga.us', '@dnr.ga.gov')              as Email,
               replace(replace(u.NormalizedEmail, '_', '.'), '@DNR.STATE.GA.US', '@DNR.GA.GOV')    as NormalizedEmail,
               u.EmailConfirmed                                                                    as EmailConfirmed,
               null                                                                                as PasswordHash,
               u.SecurityStamp                                                                     as SecurityStamp,
               u.ConcurrencyStamp                                                                  as ConcurrencyStamp,
               nullif(trim(u.Phone), '')                                                           as PhoneNumber,
               u.PhoneNumberConfirmed                                                              as PhoneNumberConfirmed,
               u.TwoFactorEnabled                                                                  as TwoFactorEnabled,
               null                                                                                as LockoutEnd,
               convert(bit, 1)                                                                     as LockoutEnabled,
               u.AccessFailedCount                                                                 as AccessFailedCount
        from dbo._archive_AspNetUsers u
            left join cte
            on lower(u.Id) = lower(cte.cId)
        where cte.cId is not null
          and u.Id not in
              ('9311E9DE-C35F-4CF4-8579-800E1B51D998',
               'A99E986A-F9FD-4562-A4C1-DF93FDC1B0B0',
               '509E3F5B-CED2-4684-B96D-0D2E6D19990B',
               '727F3763-26E7-4795-8380-FA33E8DBF617',
               '62AC545A-0F18-465E-B02C-9BBFA88EB8F0',
               '90F5B007-F33A-4CF9-A187-0BF0D9385C77',
               '57949120-5566-4192-899B-5684A5CC9793',
               '85B517B0-E5A7-4ADF-9381-41CB2BDBF168',
               '21AED5D9-12FA-46DB-BFA1-049BD6734E42',
               '7C7485D1-840E-4172-B30A-52183C48233E',
               'C339ECFC-69D1-43D4-B730-0C064BAA1589',
               '5ABA11B5-A0CF-4B05-97A3-07712A873D7D',
               '0BE87595-6B51-4154-8BAD-77885834CA38',
               '081F3450-4B74-4051-8F3F-DCF31B5559B9',
               'EAE3ECC3-F259-40F6-996D-7470080DB6BC',
               'AE3F24BF-9096-4459-B015-9F27FDB6B550',
               '61F069FF-36C8-4EB0-88B4-60A5C7D4AD4C',
               'FB8EE435-EBD3-4E59-BA60-6032C282D964',
               'AFDE9820-2FEB-4351-B65B-067F58BADF19',
               'B2FC5031-E6CC-4F4B-911D-7B767B00ADAE',
               'DBBC470F-D60C-41A4-ACC6-6761984151F9',
               'B950DC18-8278-4B0A-81D4-6298561C86B7',
               '77B5063F-E590-46BC-B3F1-90A75A5F7AC7',
               'E734F906-FCAE-43A3-806A-55C0DF4B1033',
               '7BFAFD02-389D-41E4-8F83-B1CCEBB6FF51');
        """;
}

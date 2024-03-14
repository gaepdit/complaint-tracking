namespace Cts.EfRepository.DbObjects.Reporting;

public static class ReportingQueries
{
    // language=sql
    public const string ComplaintsByStaff =
        """
        select c.CurrentOwnerId                    as Id,
               c.CurrentOfficeId                   as OfficeId,
               u.GivenName                         as GivenName,
               u.FamilyName                        as FamilyName,
               c.Id                                as Id,
               c.ComplaintCounty                   as ComplaintCounty,
               c.SourceFacilityName                as SourceFacilityName,
               convert(date, c.ReceivedDate)       as ReceivedDate,
               c.Status                            as Status
        from dbo.Complaints c
            inner join dbo.AspNetUsers u
            on c.CurrentOwnerId = u.Id
        where c.IsDeleted = convert(bit, 0)
          and c.CurrentOwnerId is not null
          and c.CurrentOfficeId = @officeId
          and convert(date, c.ReceivedDate) between @dateFrom and @dateTo
        order by u.FamilyName, u.GivenName, c.ReceivedDate
        """;

    // language=sql
    public const string ComplaintsAssignedToInactiveUsers =
        """
        select c.CurrentOwnerId              as Id,
               c.CurrentOfficeId             as OfficeId,
               u.GivenName                   as GivenName,
               u.FamilyName                  as FamilyName,
               c.Id                          as Id,
               c.ComplaintCounty             as ComplaintCounty,
               c.SourceFacilityName          as SourceFacilityName,
               convert(date, c.ReceivedDate) as ReceivedDate,
               c.Status                      as Status
        from dbo.Complaints c
            inner join dbo.AspNetUsers u
            on c.CurrentOwnerId = u.Id
        where c.IsDeleted = convert(bit, 0)
          and c.ComplaintClosed = convert(bit, 0)
          and c.CurrentOwnerId is not null
          and u.Active = convert(bit, 0)
        order by u.FamilyName, u.GivenName, c.Id
        """;

    // language=sql
    public const string DaysSinceMostRecentAction =
        """
        select c.CurrentOwnerId                                as Id,
               c.CurrentOfficeId                               as OfficeId,
               u.GivenName                                     as GivenName,
               u.FamilyName                                    as FamilyName,
               c.Id                                            as Id,
               c.ComplaintCounty                               as ComplaintCounty,
               c.SourceFacilityName                            as SourceFacilityName,
               convert(date, c.ReceivedDate)                   as ReceivedDate,
               c.Status                                        as Status,
               a.MostRecentActionDate                          as MostRecentActionDate,
               iif(a.MostRecentActionDate is null, datediff(day, c.ReceivedDate, getdate()),
                   datediff(day, a.MostRecentActionDate, getdate())) as DaysSinceMostRecentAction
        from dbo.Complaints c
            inner join dbo.AspNetUsers u
            on c.CurrentOwnerId = u.Id
            inner join (select c.Id,
                               convert(date, max(a.ActionDate)) as MostRecentActionDate
                        from dbo.Complaints c
                            left join dbo.ComplaintActions a
                            on c.Id = a.ComplaintId
                        group by c.Id) a
            on c.Id = a.Id
        where c.IsDeleted = 0
          and c.ComplaintClosed = 0
          and c.CurrentOwnerId is not null
          and iif(a.MostRecentActionDate is null, datediff(day, c.ReceivedDate, getdate()),
                  datediff(day, a.MostRecentActionDate, getdate())) >= @threshold
          and c.CurrentOfficeId = @officeId
        order by u.FamilyName, u.GivenName, c.Id
        """;

    // language=sql
    public const string DaysToClosureByOffice =
        """
        select c.CurrentOfficeId as OfficeId,
               o.Name            as OfficeName,
               count(*)          as TotalComplaintsCount,
               sum(datediff(d, c.ReceivedDate, c.ComplaintClosedDate))
                                 as TotalDaysToClosure,
               avg(convert(decimal, datediff(d, c.ReceivedDate, c.ComplaintClosedDate)))
                                 as AverageDaysToClosure
        from dbo.Complaints c
            inner join dbo.Offices o
            on o.Id = c.CurrentOfficeId
        where c.ComplaintClosed = convert(bit, 1)
          and c.ComplaintClosedDate is not null
          and c.IsDeleted = convert(bit, 0)
          and (@includeAdminClosed = convert(bit, 1) or c.Status = 'Closed')
          and convert(date, c.ComplaintClosedDate) between @dateFrom and @dateTo
        group by c.CurrentOfficeId, o.Name
        order by o.Name
        """;

    // language=sql
    public const string DaysToClosureByStaff =
        """
        select c.CurrentOwnerId                     as Id,
               c.CurrentOfficeId                    as OfficeId,
               u.GivenName                          as GivenName,
               u.FamilyName                         as FamilyName,
               c.Id                                 as Id,
               c.ComplaintCounty                    as ComplaintCounty,
               c.SourceFacilityName                 as SourceFacilityName,
               convert(date, c.ReceivedDate)        as ReceivedDate,
               c.Status                             as Status,
               convert(date, c.ComplaintClosedDate) as ComplaintClosedDate
        from dbo.Complaints c
            inner join dbo.AspNetUsers u
            on c.CurrentOwnerId = u.Id
        where c.IsDeleted = convert(bit, 0)
          and c.ComplaintClosed = convert(bit, 1)
          and c.CurrentOwnerId is not null
          and c.CurrentOfficeId = @officeId
          and (@includeAdminClosed = convert(bit, 1) or c.Status = 'Closed')
          and convert(date, c.ComplaintClosedDate) between @dateFrom and @dateTo
        order by u.FamilyName, u.GivenName, c.ComplaintClosedDate
        """;

    // language=sql
    public const string DaysToFollowupByStaff =
        """
        select c.CurrentOwnerId                    as Id,
               c.CurrentOfficeId                   as OfficeId,
               u.GivenName                         as GivenName,
               u.FamilyName                        as FamilyName,
               c.Id                                as Id,
               c.ComplaintCounty                   as ComplaintCounty,
               c.SourceFacilityName                as SourceFacilityName,
               convert(date, c.ReceivedDate)       as ReceivedDate,
               c.Status                            as Status,
               convert(date, a.EarliestActionDate) as EarliestActionDate
        from dbo.Complaints c
            inner join dbo.AspNetUsers u
            on c.CurrentOwnerId = u.Id
            inner join (select c1.Id,
                               convert(date, min(a1.ActionDate)) as EarliestActionDate
                        from dbo.Complaints c1
                            inner join dbo.ComplaintActions a1
                            on c1.Id = a1.ComplaintId
                        group by c1.Id) a
            on c.Id = a.Id
        where c.IsDeleted = convert(bit, 0)
          and c.CurrentOwnerId is not null
          and c.CurrentOfficeId = @officeId
          and convert(date, a.EarliestActionDate) between @dateFrom and @dateTo
        order by u.FamilyName, u.GivenName, c.Id
        """;
}

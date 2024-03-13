namespace Cts.EfRepository.DbObjects.Reporting;

public static class ReportingQueries
{
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
        from Complaints c
            inner join AspNetUsers u
            on c.CurrentOwnerId = u.Id
            inner join (select c.Id,
                               convert(date, max(a.ActionDate)) as MostRecentActionDate
                        from Complaints c
                            left join ComplaintActions a
                            on c.Id = a.ComplaintId
                        group by c.Id) a
            on c.Id = a.Id
        where c.IsDeleted = 0
          and c.ComplaintClosed = 0
          and c.CurrentOwnerId is not null
          and iif(a.MostRecentActionDate is null, datediff(day, c.ReceivedDate, getdate()),
                  datediff(day, a.MostRecentActionDate, getdate())) >= @threshold
          and c.CurrentOfficeId = @officeId
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
               convert(date, c.ComplaintClosedDate) as ComplaintClosedDate
        from Complaints c
            inner join AspNetUsers u
            on c.CurrentOwnerId = u.Id
        where c.IsDeleted = convert(bit, 0)
          and c.ComplaintClosed = convert(bit, 1)
          and c.CurrentOwnerId is not null
          and c.CurrentOfficeId = @officeId
          and (@includeAdminClosed = convert(bit, 1) or c.Status = 'Closed')
          and convert(date, c.ComplaintClosedDate) between @dateFrom and @dateTo
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
               convert(date, a.EarliestActionDate) as EarliestActionDate
        from Complaints c
            inner join AspNetUsers u
            on c.CurrentOwnerId = u.Id
            inner join (select c1.Id,
                               convert(date, min(a1.ActionDate)) as EarliestActionDate
                        from Complaints c1
                            inner join ComplaintActions a1
                            on c1.Id = a1.ComplaintId
                        group by c1.Id) a
            on c.Id = a.Id
        where c.IsDeleted = convert(bit, 0)
          and c.CurrentOwnerId is not null
          and c.CurrentOfficeId = @officeId
          and convert(date, a.EarliestActionDate) between @dateFrom and @dateTo
        """;
}

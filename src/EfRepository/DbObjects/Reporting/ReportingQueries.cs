namespace Cts.EfRepository.DbObjects.Reporting;

public static class ReportingQueries
{
    // language=sql
    public const string DaysSinceLastAction =
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
               a.LastActionDate                                as LastActionDate,
               iif(a.LastActionDate is null, datediff(day, c.ReceivedDate, getdate()),
                   datediff(day, a.LastActionDate, getdate())) as DaysSinceLastAction
        from Complaints c
            inner join AspNetUsers u
            on c.CurrentOwnerId = u.Id
            inner join (select c.Id,
                               convert(date, max(a.ActionDate)) as LastActionDate
                        from Complaints c
                            left join ComplaintActions a
                            on c.Id = a.ComplaintId
                        group by c.Id) a
            on c.Id = a.Id
        where c.IsDeleted = 0
          and c.ComplaintClosed = 0
          and c.CurrentOwnerId is not null
          and iif(a.LastActionDate is null, datediff(day, c.ReceivedDate, getdate()),
                  datediff(day, a.LastActionDate, getdate())) >= @Threshold
          and c.CurrentOfficeId = @OfficeId
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
               convert(date, c.ComplaintClosedDate) as ComplaintClosedDate,
               c.Status                             as Status
        from Complaints c
            inner join AspNetUsers u
            on c.CurrentOwnerId = u.Id
        where c.IsDeleted = convert(bit, 0)
          and c.ComplaintClosed = convert(bit, 1)
          and c.CurrentOwnerId is not null
          and c.CurrentOfficeId = @officeId
          and (@includeAdminClosed = convert(bit, 1) or c.Status = 'Closed')
          and c.ComplaintClosedDate >= @dateFrom
          and convert(date, c.ComplaintClosedDate) <= @dateTo
        """;
}

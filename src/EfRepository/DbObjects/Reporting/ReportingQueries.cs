namespace Cts.EfRepository.DbObjects.Reporting;

// language=sql
public static class ReportingQueries
{
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
}

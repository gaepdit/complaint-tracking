CREATE OR ALTER VIEW dbo.ClosedComplaintsView
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Redacted view of closed CTS complaints.
  Part of a set of queries used to respond to public requests for an archived copy of the database.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2017-12-21  DWaldron            Initial Version
2021-10-06  DWaldron            Update email regex
2024-02-29  DWaldron            Application rewrite

---

Notes:

  Uses SQL Server Regex Functions to redact email and phone number data.
  See https://github.com/gaepdit/SqlServerRegexFunctions
  
  Phone number RegEx: https://regexr.com/3h4h0
  Email RegEx: https://regexr.com/670vn
  Replacement email based on https://tools.ietf.org/html/rfc2606#section-2

***************************************************************************************************/

select c.Id                       as [ComplaintId],
       c.CallerAddress_City       as [CallerCity],
       c.CallerName               as [CallerName],
       c.CallerAddress_PostalCode as [CallerPostalCode],
       c.CallerRepresents         as [CallerRepresents],
       c.CallerAddress_State      as [CallerState],
       c.CallerAddress_Street     as [CallerStreet],
       c.CallerAddress_Street2    as [CallerStreet2],
       c.ComplaintCity            as [ComplaintCity],
       c.ComplaintCounty          as [ComplaintCounty],
       c.ComplaintDirections      as [ComplaintDirections],
       dbo.RegexReplace(dbo.RegexReplace(c.ComplaintLocation, N'\b\d{3}[- .]\d{4}\b', '[phone number removed]'),
                        N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b', '[email@removed.invalid]')
                                  as [ComplaintLocation],
       IIF(len(c.ComplaintNature) > 32600,
           concat('[This entry has been truncated because it was longer than 32,600 characters] ',
                  CHAR(13), CHAR(10),
                  left(dbo.RegexReplace(
                               dbo.RegexReplace(c.ComplaintNature, N'\b\d{3}[- .]\d{4}\b', '[phone number removed]'),
                               N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b', '[email@removed.invalid]'), 32600)),
           dbo.RegexReplace(dbo.RegexReplace(c.ComplaintNature, N'\b\d{3}[- .]\d{4}\b', '[phone number removed]'),
                            N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b', '[email@removed.invalid]'))
                                  as [ComplaintNature],
       currentOffice.Name         as [CurrentOffice],
       IIF(c.CurrentOwnerId is null, null, concat_ws(', ', currentOwner.FamilyName, currentOwner.GivenName))
                                  as [CurrentOwner],
       c.ComplaintClosedDate      as [ComplaintClosedDate],
       c.CurrentOwnerAcceptedDate as [CurrentOwnerAcceptedDate],
       c.CurrentOwnerAssignedDate as [CurrentOwnerAssignedDate],
       c.EnteredDate              as [EnteredDate],
       c.ReceivedDate             as [ReceivedDate],
       IIF(c.EnteredById is null, null, concat_ws(', ', enteredBy.FamilyName, enteredBy.GivenName))
                                  as [EnteredBy],
       primaryConcern.Name        as [PrimaryConcern],
       secondaryConcern.Name      as [SecondaryConcern],
       IIF(c.ReceivedById is null, null, concat_ws(', ', receivedBy.FamilyName, receivedBy.GivenName))
                                  as [ReceivedBy],
       IIF(c.ReviewedById is null, null, concat_ws(', ', reviewedBy.FamilyName, reviewedBy.GivenName))
                                  as [ReviewedBy],
       dbo.RegexReplace(dbo.RegexReplace(c.ReviewComments, N'\b\d{3}[- .]\d{4}\b', '[phone number removed]'),
                        N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b', '[email@removed.invalid]')
                                  as [ReviewComments],
       c.SourceAddress_City       as [SourceCity],
       c.SourceContactName        as [SourceContactName],
       c.SourceFacilityIdNumber   as [SourceFacilityId],
       c.SourceFacilityName       as [SourceFacilityName],
       c.SourceAddress_PostalCode as [SourcePostalCode],
       c.SourceAddress_State      as [SourceState],
       c.SourceAddress_Street     as [SourceStreet],
       c.SourceAddress_Street2    as [SourceStreet2]
from Complaints c
    left join Offices currentOffice
    on c.CurrentOfficeId = currentOffice.Id
    left join AspNetUsers currentOwner
    on c.CurrentOwnerId = currentOwner.Id
    left join AspNetUsers enteredBy
    on c.EnteredById = enteredBy.Id
    left join Concerns primaryConcern
    on c.PrimaryConcernId = primaryConcern.Id
    left join Concerns secondaryConcern
    on c.SecondaryConcernId = secondaryConcern.Id
    left join AspNetUsers receivedBy
    on c.ReceivedById = receivedBy.Id
    left join AspNetUsers reviewedBy
    on c.ReviewedById = reviewedBy.Id
where c.IsDeleted = convert(bit, 0) -- Complaint not deleted
  and c.ComplaintClosed = convert(bit, 1); -- Complaint closed

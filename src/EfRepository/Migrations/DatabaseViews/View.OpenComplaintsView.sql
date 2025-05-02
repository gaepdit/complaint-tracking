CREATE OR ALTER VIEW dbo.OpenComplaintsView
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Redacted view of open CTS complaints.
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
       primaryConcern.Name        as [PrimaryConcern],
       secondaryConcern.Name      as [SecondaryConcern],
       c.ReceivedDate             as [ReceivedDate],
       IIF(c.ReceivedById is null, '', concat_ws(', ', receivedBy.FamilyName, receivedBy.GivenName))
                                  as [ReceivedBy]
from Complaints c
         left join Concerns primaryConcern
                   on c.PrimaryConcernId = primaryConcern.Id
         left join Concerns secondaryConcern
                   on c.SecondaryConcernId = secondaryConcern.Id
         left join AspNetUsers receivedBy
                   on c.ReceivedById = receivedBy.Id
where c.IsDeleted = convert(bit, 0) -- Complaint not deleted
  and c.ComplaintClosed = convert(bit, 0); -- Complaint open

GO

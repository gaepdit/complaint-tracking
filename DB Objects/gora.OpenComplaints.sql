USE ComplaintTracking;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

/*******************************************************************************

Author:     Doug Waldron
Created:    2017-12-21
Overview:   Redacted view of open CTS complaints

Part of a set of queries used to respond to a public GORA request for 
an updated copy of the CTS database 

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2017-12-21  DWaldron            Initial Version
2021-10-06  DWaldron            Update email regex

---

Notes:

Uses SQL Server Regex Functions to redact email and phone number data.
See https://github.com/gaepdit/SqlServerRegexFunctions

Phone number RegEx: https://regexr.com/3h4h0
Email RegEx: https://regexr.com/670vn
Replacement email based on https://tools.ietf.org/html/rfc2606#section-2

*******************************************************************************/

CREATE OR ALTER VIEW gora.OpenComplaints
AS

SELECT c.Id                                    AS [ComplaintId],
       c.CallerCity,
       c.CallerName,
       c.CallerPostalCode,
       c.CallerRepresents,
       -- CallerStateId,
       callerState.Name                        AS CallerState,
       c.CallerStreet,
       c.CallerStreet2,
       c.ComplaintCity,
       -- ComplaintClosed,
       -- ComplaintCountyId,
       complaintCounty.Name                    AS ComplaintCounty,
       c.ComplaintDirections,
       -- ComplaintLocation,
       dbo.RegexReplace(
               dbo.RegexReplace(
                       c.ComplaintLocation,
                       N'\b\d{3}[- .]\d{4}\b',
                       'xxx-xxxx'),
               N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b',
               '[email@removed.invalid]')      AS ComplaintLocation,
       -- ComplaintNature,
       IIF(len(c.ComplaintNature) > 32600,
           concat('[This entry has been truncated because it was longer than 32,600 characters] ',
                  CHAR(13), CHAR(10),
                  left(dbo.RegexReplace(
                               dbo.RegexReplace(
                                       c.ComplaintNature,
                                       N'\b\d{3}[- .]\d{4}\b',
                                       'xxx-xxxx'),
                               N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b',
                               '[email@removed.invalid]'), 32600)),
           dbo.RegexReplace(
                   dbo.RegexReplace(
                           c.ComplaintNature,
                           N'\b\d{3}[- .]\d{4}\b',
                           'xxx-xxxx'),
                   N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b',
                   '[email@removed.invalid]')) AS ComplaintNature,
       -- PrimaryConcernId,
       primaryConcern.Name                     AS PrimaryConcern,
       -- SecondaryConcernId,
       secondaryConcern.Name                   AS SecondaryConcern,
       c.DateReceived,
       -- ReceivedById,
       IIF(c.ReceivedById IS NULL, NULL,
           concat_ws(', ', receivedBy.LastName, receivedBy.FirstName))
                                               AS ReceivedBy
       -- Status
FROM Complaints c
    LEFT JOIN LookupStates callerState
    ON c.CallerStateId = callerState.Id
    LEFT JOIN LookupCounties complaintCounty
    ON c.ComplaintCountyId = complaintCounty.Id
    LEFT JOIN LookupConcerns primaryConcern
    ON c.PrimaryConcernId = primaryConcern.Id
    LEFT JOIN AspNetUsers receivedBy
    ON c.ReceivedById = receivedBy.Id
    LEFT JOIN LookupConcerns secondaryConcern
    ON c.SecondaryConcernId = secondaryConcern.Id
WHERE c.Deleted = 0 -- Not deleted
  AND c.ComplaintClosed = 0 -- Not Closed

GO

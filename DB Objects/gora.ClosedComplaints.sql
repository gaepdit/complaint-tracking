USE ComplaintTracking;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER VIEW gora.ClosedComplaints
AS

/*******************************************************************************

Author:     Doug Waldron
Created:    2017-12-21
Overview:   Redacted view of closed CTS complaints

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
       -- c.CreatedById,
       -- c.CreatedDate,
       -- CurrentAssignmentTransitionId,
       -- CurrentOfficeId,
       currentOffice.Name                      AS CurrentOffice,
       -- CurrentOwnerId,
       IIF(c.CurrentOwnerId IS NULL, NULL,
           concat_ws(', ', currentOwner.LastName, currentOwner.FirstName))
                                               AS CurrentOwner,
       c.DateComplaintClosed,
       c.DateCurrentOwnerAccepted,
       c.DateCurrentOwnerAssigned,
       -- DateDeleted,
       c.DateEntered,
       c.DateReceived,
       -- DeleteComments,
       -- Deleted,
       -- DeletedById,
       -- EnteredById,
       IIF(c.EnteredById IS NULL, NULL,
           concat_ws(', ', enteredBy.LastName, enteredBy.FirstName))
                                               AS EnteredBy,
       -- PrimaryConcernId,
       primaryConcern.Name                     AS PrimaryConcern,
       -- ReceivedById,
       IIF(c.ReceivedById IS NULL, NULL,
           concat_ws(', ', receivedBy.LastName, receivedBy.FirstName))
                                               AS ReceivedBy,
       -- ReviewById,
       IIF(c.ReviewById IS NULL, NULL,
           concat_ws(', ', reviewBy.LastName, reviewBy.FirstName))
                                               AS ReviewBy,
       -- ReviewComments,
       dbo.RegexReplace(
               dbo.RegexReplace(
                       c.ReviewComments,
                       N'\b\d{3}[- .]\d{4}\b',
                       'xxx-xxxx'),
               N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b',
               '[email@removed.invalid]')
                                               AS ReviewComments,
       -- SecondaryConcernId,
       secondaryConcern.Name                   AS SecondaryConcern,
       c.SourceCity,
       c.SourceContactName,
       c.SourceFacilityId,
       c.SourceFacilityName,
       c.SourcePostalCode,
       -- SourceStateId,
       sourceState.Name                        AS SourceState,
       c.SourceStreet,
       c.SourceStreet2
       -- Status
FROM Complaints c
    LEFT JOIN LookupStates callerState
    ON c.CallerStateId = callerState.Id
    LEFT JOIN LookupCounties complaintCounty
    ON c.ComplaintCountyId = complaintCounty.Id
    LEFT JOIN LookupOffices currentOffice
    ON c.CurrentOfficeId = currentOffice.Id
    LEFT JOIN AspNetUsers currentOwner
    ON c.CurrentOwnerId = currentOwner.Id
    LEFT JOIN AspNetUsers enteredBy
    ON c.EnteredById = enteredBy.Id
    LEFT JOIN LookupConcerns primaryConcern
    ON c.PrimaryConcernId = primaryConcern.Id
    LEFT JOIN AspNetUsers receivedBy
    ON c.ReceivedById = receivedBy.Id
    LEFT JOIN AspNetUsers reviewBy
    ON c.ReviewById = reviewBy.Id
    LEFT JOIN LookupConcerns secondaryConcern
    ON c.SecondaryConcernId = secondaryConcern.Id
    LEFT JOIN LookupStates sourceState
    ON sourceState.Id = c.SourceStateId
WHERE c.Deleted = 0 -- Not deleted
  AND c.ComplaintClosed = 1; -- Closed

GO

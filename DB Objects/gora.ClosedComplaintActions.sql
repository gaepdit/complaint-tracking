USE ComplaintTracking;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

/*******************************************************************************

Author:     Doug Waldron
Created:    2017-12-21
Overview:   Redacted view of complaint actions for closed CTS complaints

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

CREATE OR ALTER VIEW gora.ClosedComplaintActions
AS

SELECT ca.ComplaintId,
       -- ca.Id,
       ca.ActionDate,
       -- ActionTypeId,
       actionType.Name                    AS ActionType,
       -- Comments,
       dbo.RegexReplace(
               dbo.RegexReplace(
                       ca.Comments,
                       N'\b\d{3}[- .]\d{4}\b',
                       'xxx-xxxx'),
               N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b',
               '[email@removed.invalid]') AS Comments,
       -- ca.CreatedById,
       -- ca.CreatedDate,
       -- DateDeleted,
       ca.DateEntered,
       -- Deleted,
       -- DeletedById,
       -- EnteredById,
       IIF(ca.EnteredById IS NULL, NULL,
           concat_ws(', ', enteredBy.LastName, enteredBy.FirstName))
                                          AS EnteredBy,
       ca.Investigator
FROM ComplaintActions ca
    INNER JOIN Complaints c
    ON ca.ComplaintId = c.Id
    LEFT JOIN LookupActionTypes actionType
    ON ca.ActionTypeId = actionType.Id
    LEFT JOIN AspNetUsers enteredBy
    ON ca.EnteredById = enteredBy.Id
WHERE ca.Deleted = 0 -- Not deleted
  AND c.Deleted = 0  -- Not deleted
  AND c.ComplaintClosed = 1 -- Closed

GO

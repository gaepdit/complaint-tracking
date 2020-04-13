SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('gora.ClosedComplaints') IS NOT NULL
    DROP VIEW gora.ClosedComplaints;
GO

/*****************************************************************************

Author:     Doug Waldron
Created:    2017-12-21
Overview:   Redacted view of closed CTS complaints

Part of a set of queries used to respond to a public GORA request for 
an updated copy of the CTS database 

========================

Modification History:

When        Who                 What
----------  ------------------  ----------------------------------------
2017-12-21  DWaldron            Initial Version

========================

Notes:

Uses SQL Server Regex Functions to redact email and phone number data.
See https://gitlab.com/ga-epd-it/sql-server-regex/

Phone number RegEx: https://regexr.com/3h4h0
Email RegEx: https://regexr.com/3h4ho
Replacement email based on https://tools.ietf.org/html/rfc2606#section-2

*******************************************************************************/

CREATE VIEW gora.ClosedComplaints
    AS

        SELECT
            c.Id                  AS [ComplaintId],
            CallerCity,
            CallerName,
            CallerPostalCode,
            CallerRepresents,
            -- CallerStateId,
            callerState.Name      AS CallerState,
            CallerStreet,
            CallerStreet2,
            ComplaintCity,
            -- ComplaintClosed,
            -- ComplaintCountyId,
            complaintCounty.Name  AS ComplaintCounty,
            ComplaintDirections,
            -- ComplaintLocation,
            dbo.RegexReplace(
                dbo.RegexReplace(ComplaintLocation, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
                N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]')
                                  AS ComplaintLocation,
            -- ComplaintNature,
            CASE
            WHEN len(ComplaintNature) > 32600
                THEN concat(
                    '[This entry has been truncated because it was longer than 32,600 characters] ' + CHAR(13) +
                    CHAR(10),
                    left(dbo.RegexReplace(
                             dbo.RegexReplace(ComplaintNature, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
                             N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]'), 32600))
            ELSE dbo.RegexReplace(
                dbo.RegexReplace(ComplaintNature, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
                N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]')
            END                   AS ComplaintNature,
            -- c.CreatedById,
            -- c.CreatedDate,
            -- CurrentAssignmentTransitionId,
            -- CurrentOfficeId,
            currentOffice.Name    AS CurrentOffice,
            -- CurrentOwnerId,
            CASE
            WHEN c.CurrentOwnerId IS NULL
                THEN NULL
            ELSE
                concat(currentOwner.LastName, concat(', ', currentOwner.FirstName))
            END                   AS CurrentOwner,
            DateComplaintClosed,
            DateCurrentOwnerAccepted,
            DateCurrentOwnerAssigned,
            -- DateDeleted,
            DateEntered,
            DateReceived,
            -- DeleteComments,
            -- Deleted,
            -- DeletedById,
            -- EnteredById,
            CASE
            WHEN c.EnteredById IS NULL
                THEN NULL
            ELSE
                concat(enteredBy.LastName, concat(', ', enteredBy.FirstName))
            END                   AS EnteredBy,
            -- PrimaryConcernId,
            primaryConcern.Name   AS PrimaryConcern,
            -- ReceivedById,
            CASE
            WHEN c.ReceivedById IS NULL
                THEN NULL
            ELSE
                concat(receivedBy.LastName, concat(', ', receivedBy.FirstName))
            END                   AS ReceivedBy,
            -- ReviewById,
            CASE
            WHEN c.ReviewById IS NULL
                THEN NULL
            ELSE
                concat(reviewBy.LastName, concat(', ', reviewBy.FirstName))
            END                   AS ReviewBy,
            -- ReviewComments,
            dbo.RegexReplace(
                dbo.RegexReplace(ReviewComments, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
                N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]')
                                  AS ReviewComments,
            -- SecondaryConcernId,
            secondaryConcern.Name AS SecondaryConcern,
            SourceCity,
            SourceContactName,
            SourceFacilityId,
            SourceFacilityName,
            SourcePostalCode,
            -- SourceStateId,
            sourceState.Name      AS SourceState,
            SourceStreet,
            SourceStreet2
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
              AND ComplaintClosed = 1 -- Closed

GO

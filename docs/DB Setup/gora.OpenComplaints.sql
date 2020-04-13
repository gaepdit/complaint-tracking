SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('gora.OpenComplaints') IS NOT NULL
    DROP VIEW gora.OpenComplaints;
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

CREATE VIEW gora.OpenComplaints
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
            -- PrimaryConcernId,
            primaryConcern.Name   AS PrimaryConcern,
            -- SecondaryConcernId,
            secondaryConcern.Name AS SecondaryConcern,
            DateReceived,
            -- ReceivedById,
            CASE
            WHEN c.ReceivedById IS NULL
                THEN NULL
            ELSE
                concat(receivedBy.LastName, concat(', ', receivedBy.FirstName))
            END                   AS ReceivedBy
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
              AND ComplaintClosed = 0 -- Not Closed

GO

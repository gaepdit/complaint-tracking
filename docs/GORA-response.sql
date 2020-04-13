-- ========================
-- Queries used to respond to a public GORA request for 
-- an updated copy of the CTS database (GORA-1, CTS-295)
--
-- Uses SQL Server Regex Functions: https://gitlab.com/ga-epd-it/sql-server-regex/
-- Phone number RegEx: https://regexr.com/3h4h0
-- Email RegEx: https://regexr.com/3h4ho
-- Replacement email based on https://tools.ietf.org/html/rfc2606#section-2
-- ========================

-- ========================
-- Table counts
-- ========================

SELECT
    'Closed Complaints' AS [Table],
    count(*)           AS [Count]
FROM Complaints c
WHERE c.Deleted = 0
      AND ComplaintClosed = 1

UNION

SELECT
    'Open Complaints',
    count(*)
FROM Complaints c
WHERE c.Deleted = 0
      AND ComplaintClosed = 0

UNION

SELECT
    'Complaint Actions (closed complaints)',
    count(*)
FROM ComplaintActions ca
    INNER JOIN Complaints c
        ON ca.ComplaintId = c.Id
WHERE ca.Deleted = 0
      AND c.Deleted = 0
      AND c.ComplaintClosed = 1;

-- ========================
-- Closed Complaints
-- ========================

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
    ComplaintClosed,
    -- ComplaintCountyId,
    complaintCounty.Name  AS ComplaintCounty,
    ComplaintDirections,
    -- ComplaintLocation,
    dbo.RegexReplace(
        dbo.RegexReplace(ComplaintLocation, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
        N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]')
                    AS ComplaintLocation,
    -- ComplaintNature,
    dbo.RegexReplace(
        dbo.RegexReplace(ComplaintNature, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
        N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]')
                    AS ComplaintNature,
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
    SourceStreet2,
    -- Status,
    CASE
    WHEN Status = 0
        THEN 'New'
    WHEN Status = 1
        THEN 'Under Investigation'
    WHEN Status = 2
        THEN 'Review Pending'
    WHEN Status = 3
        THEN 'Approved/Closed'
    END                   AS Status
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
ORDER BY c.Id;

-- ========================
-- Closed Complaint Actions
-- ========================

SELECT
    ComplaintId,
    -- ca.Id,
    ActionDate,
    -- ActionTypeId,
    actionType.Name AS ActionType,
    -- Comments,
    dbo.RegexReplace(
        dbo.RegexReplace(Comments, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
        N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]')
                    AS Comments,
    -- ca.CreatedById,
    -- ca.CreatedDate,
    -- DateDeleted,
    ca.DateEntered,
    -- Deleted,
    -- DeletedById,
    -- EnteredById,
    CASE
    WHEN ca.EnteredById IS NULL
        THEN NULL
    ELSE
        concat(enteredBy.LastName, concat(', ', enteredBy.FirstName))
    END             AS EnteredBy,
    Investigator
FROM ComplaintActions ca
    INNER JOIN Complaints c
        ON ca.ComplaintId = c.Id
    LEFT JOIN LookupActionTypes actionType
        ON ca.ActionTypeId = actionType.Id
    LEFT JOIN AspNetUsers enteredBy
        ON ca.EnteredById = enteredBy.Id
WHERE ca.Deleted = 0
      AND c.Deleted = 0
      AND c.ComplaintClosed = 1
ORDER BY ComplaintId, ActionDate;

-- ========================
-- Open Complaints
-- ========================

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
    ComplaintClosed,
    -- ComplaintCountyId,
    complaintCounty.Name  AS ComplaintCounty,
    ComplaintDirections,
    -- ComplaintLocation,
    dbo.RegexReplace(
        dbo.RegexReplace(ComplaintLocation, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
        N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]')
                    AS ComplaintLocation,
    -- ComplaintNature,
    dbo.RegexReplace(
        dbo.RegexReplace(ComplaintNature, N'\b\d{3}[- .]\d{4}\b', 'xxx-xxxx'),
        N'\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b', '[email@removed.invalid]')
                    AS ComplaintNature,
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
    END                   AS ReceivedBy,
    -- Status,
    CASE
    WHEN Status = 0
        THEN 'New'
    WHEN Status = 1
        THEN 'Under Investigation'
    WHEN Status = 2
        THEN 'Review Pending'
    WHEN Status = 3
        THEN 'Approved/Closed'
    END                   AS Status
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
ORDER BY c.Id;

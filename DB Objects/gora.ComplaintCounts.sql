USE ComplaintTracking;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER VIEW gora.ComplaintCounts
AS

/*******************************************************************************

Author:     Doug Waldron
Created:    2017-12-21
Overview:   Counts of complaints/actions in the database

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2017-12-21  DWaldron            Initial Version

*******************************************************************************/

SELECT 'Open Complaints' AS [Table],
       count(*)          AS [Count]
FROM Complaints c
WHERE c.Deleted = 0       -- Not deleted
  AND ComplaintClosed = 0 -- Not Closed

UNION

SELECT 'Closed Complaints',
       count(*)
FROM Complaints c
WHERE c.Deleted = 0       -- Not deleted
  AND ComplaintClosed = 1 -- Closed

UNION

SELECT 'Complaint Actions (closed complaints)',
       count(*)
FROM ComplaintActions ca
    INNER JOIN Complaints c
    ON ca.ComplaintId = c.Id
WHERE ca.Deleted = 0 -- Not deleted
  AND c.Deleted = 0  -- Not deleted
  AND c.ComplaintClosed = 1; -- Closed

GO

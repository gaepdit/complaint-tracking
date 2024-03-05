CREATE OR ALTER VIEW dbo.RecordsCountView
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Counts of public complaints/actions in the database.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2017-12-21  DWaldron            Initial Version
2024-02-29  DWaldron            Application rewrite

***************************************************************************************************/

SELECT 'Open Complaints' AS [Table],
       count(*)          AS [Count],
       1                 as [Order]
FROM Complaints c
WHERE c.IsDeleted = convert(bit, 0) -- Not deleted
  AND ComplaintClosed = 0           -- Not Closed

UNION

SELECT 'Closed Complaints',
       count(*),
       2
FROM Complaints c
WHERE c.IsDeleted = convert(bit, 0) -- Not deleted
  AND ComplaintClosed = 1           -- Closed

UNION

SELECT 'Complaint Actions',
       count(*),
       3
FROM ComplaintActions ca
         INNER JOIN Complaints c
                    ON ca.ComplaintId = c.Id
WHERE ca.IsDeleted = convert(bit, 0) -- Not deleted
  AND c.IsDeleted = convert(bit, 0)  -- Not deleted
  AND c.ComplaintClosed = convert(bit, 1); -- Closed

USE ComplaintTracking;
GO

DROP PROCEDURE IF EXISTS admin.CloseComplaints;
GO

IF TYPE_ID('admin.CloseComplaintsTableType') IS NOT NULL
    DROP TYPE admin.CloseComplaintsTableType;
GO

CREATE TYPE admin.CloseComplaintsTableType AS table
(
    Id            int not null primary key ,
    ReviewComment nvarchar(4000)
);
GO

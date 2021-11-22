USE ComplaintTracking;
GO

IF TYPE_ID('admin.CloseComplaintsTableType') IS NOT NULL
    DROP TYPE admin.CloseComplaintsTableType;
GO

CREATE TYPE admin.CloseComplaintsTableType AS table
(
    Id            int NOT NULL PRIMARY KEY,
    ReviewComment nvarchar(4000)
);
GO

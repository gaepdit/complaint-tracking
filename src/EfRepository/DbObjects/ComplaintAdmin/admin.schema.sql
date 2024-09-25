USE ComplaintTracking;
GO

-- This schema contains administrative scripts not used by the application.
IF NOT EXISTS
    (
        SELECT 1
        FROM sys.schemas
        WHERE name = 'admin'
    )
    BEGIN
        EXEC ('CREATE SCHEMA admin');
    END;
GO

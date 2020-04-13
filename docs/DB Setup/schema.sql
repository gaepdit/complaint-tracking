IF NOT EXISTS
(
    SELECT 1
    FROM sys.schemas
    WHERE name = 'gora'
)
    BEGIN
        EXEC ('CREATE SCHEMA gora');
    END;

-- This file is created by the SqlServerRegexFunctions project.
-- https://github.com/gaepdit/SqlServerRegexFunctions

IF OBJECT_ID('dbo.RegexMatch') IS NOT NULL
    DROP FUNCTION dbo.RegexMatch;

CREATE FUNCTION dbo.RegexMatch(@input nvarchar(max), @pattern nvarchar(max))
    RETURNS bit
    WITH EXECUTE AS CALLER ,
        RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME
    RegexFunctions.UserDefinedFunctions.RegexMatch;

IF OBJECT_ID('dbo.RegexReplace') IS NOT NULL
    DROP FUNCTION dbo.RegexReplace;

CREATE FUNCTION dbo.RegexReplace(@expression nvarchar(max), @pattern nvarchar(max), @replace nvarchar(max)
)
    RETURNS nvarchar(max)
    WITH EXECUTE AS CALLER ,
        RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME
    RegexFunctions.UserDefinedFunctions.RegexReplace;

IF OBJECT_ID('dbo.RegexSelectAll') IS NOT NULL
    DROP FUNCTION dbo.RegexSelectAll;

CREATE FUNCTION dbo.RegexSelectAll(@input nvarchar(max), @pattern nvarchar(max), @matchDelimiter nvarchar(max)
)
    RETURNS nvarchar(max)
    WITH EXECUTE AS CALLER ,
        RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME
    RegexFunctions.UserDefinedFunctions.RegexSelectAll;

IF OBJECT_ID('dbo.RegexSelectOne') IS NOT NULL
    DROP FUNCTION dbo.RegexSelectOne;

CREATE FUNCTION dbo.RegexSelectOne(@input nvarchar(max), @pattern nvarchar(max), @matchIndex int)
    RETURNS nvarchar(max)
    WITH EXECUTE AS CALLER ,
        RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME
    RegexFunctions.UserDefinedFunctions.RegexSelectOne;

namespace Cts.EfRepository.Migrations.DatabaseView;

public static partial class View
{
    // language=sql
    public const string ClosedComplaintActionsView =
        """
        CREATE OR ALTER VIEW dbo.ClosedComplaintActionsView
        AS
        
        /**************************************************************************************************
        
        Author:     Doug Waldron
        Overview:
          Redacted view of complaint actions for closed CTS complaints.
          Part of a set of queries used to respond to public requests for an archived copy of the database.
        
        Modification History:
        When        Who                 What
        ----------  ------------------  -------------------------------------------------------------------
        2017-12-21  DWaldron            Initial Version
        2021-10-06  DWaldron            Update email regex
        2024-02-29  DWaldron            Application rewrite
        
        ---
        
        Notes:
        
          Uses SQL Server Regex Functions to redact email and phone number data.
          See https://github.com/gaepdit/SqlServerRegexFunctions
          
          Phone number RegEx: https://regexr.com/3h4h0
          Email RegEx: https://regexr.com/670vn
          Replacement email based on https://tools.ietf.org/html/rfc2606#section-2
        
        ***************************************************************************************************/
        
        select a.ComplaintId   as [ComplaintId],
               a.ActionDate    as [ActionDate],
               actionType.Name as [ActionType],
               dbo.RegexReplace(dbo.RegexReplace(a.Comments, N'\b\d{3}[- .]\d{4}\b', '[phone number removed]'),
                                N'\b[\w\.-]+@[\w\.-]+\.\w{2,63}\b', '[email@removed.invalid]')
                               as [Comments],
               a.EnteredDate   as [EnteredDate],
               IIF(a.EnteredById is null, null, concat_ws(', ', enteredBy.FamilyName, enteredBy.GivenName))
                               as [EnteredBy],
               a.Investigator  as [Investigator]
        from ComplaintActions a
            inner join Complaints c
            on a.ComplaintId = c.Id
            left join ActionTypes actionType
            on a.ActionTypeId = actionType.Id
            left join AspNetUsers enteredBy
            on a.EnteredById = enteredBy.Id
        where a.IsDeleted = convert(bit, 0) -- Action not deleted
          and c.IsDeleted = convert(bit, 0) -- Complaint not deleted
          and c.ComplaintClosed = convert(bit, 1); -- Complaint closed
        """;
}

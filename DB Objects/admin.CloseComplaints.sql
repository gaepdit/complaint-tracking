USE ComplaintTracking;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE admin.CloseComplaints
    @ComplaintsTVP admin.CloseComplaintsTableType READONLY
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   This procedure is used to administratively close complaints.

Input Parameters:
  @ComplaintsTVP    - A table-valued parameter containing:
    Id              - The IDs of the complaints to administratively close.
    ReviewComment   - The review comment to add.

Example usage:

begin
    declare @complaints as admin.CloseComplaintsTableType;

    insert into @complaints
        (Id, ReviewComment)
    values
        (999998, 'Administratively closed with this review comment.'),
        (999999, 'Administratively closed with another review comment.');

    exec admin.CloseComplaints @complaints;
end;


Returns:
   0 on success
  -1 on error

Tables written to:
  Complaints
  ComplaintTransitions

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2021-11-22  DWaldron            Initial version (complaint-tracking#496)

*******************************************************************************/

    SET XACT_ABORT, NOCOUNT ON;
BEGIN TRY

    declare
        @adminId uniqueidentifier;

    BEGIN TRANSACTION;

    select @adminId = Id
    from AspNetUsers
    where Email = 'epd_it@dnr.ga.gov';

    insert into ComplaintTransitions
        (Id,
         Comment,
         ComplaintId,
         CreatedById,
         CreatedDate,
         DateTransferred,
         TransferredByUserId,
         TransitionType)
    select newid(),
           t.ReviewComment,
           c.Id,
           @adminId,
           sysdatetime(),
           sysdatetime(),
           @adminId,
           4 -- Closed
    from Complaints c
        inner join @ComplaintsTVP t
        on c.Id = t.Id
    where c.ComplaintClosed = 0
      and c.Deleted = 0;

    update c
    set ReviewById          = @adminId,
        ReviewComments      = t.ReviewComment,
        ComplaintClosed     = 1,
        DateComplaintClosed = sysdatetime(),
        Status              = 4 -- Administratively Closed
    from Complaints c
        inner join @ComplaintsTVP t
        on c.Id = t.Id
    where c.ComplaintClosed = 0
      and c.Deleted = 0;

    COMMIT TRANSACTION;

    select c.Id,
           c.ComplaintClosed,
           c.CreatedDate,
           c.DateComplaintClosed,
           c.ReviewById,
           c.ReviewComments,
           c.Status,
           c.UpdatedById,
           c.UpdatedDate
    from Complaints c
        inner join @ComplaintsTVP t
        on c.Id = t.Id;

    select r.ComplaintId,
           r.Comment,
           r.CreatedById,
           r.CreatedDate,
           r.DateTransferred,
           r.TransferredByUserId,
           r.TransitionType
    from ComplaintTransitions r
        inner join @ComplaintsTVP t
        on r.ComplaintId = t.Id
    where r.TransitionType = 4;

    RETURN 0;
END TRY
BEGIN CATCH
    IF @@trancount > 0
        ROLLBACK TRANSACTION;
    DECLARE
        @ErrorMessage nvarchar(4000) = ERROR_MESSAGE(),
        @ErrorSeverity int = ERROR_SEVERITY();
    RAISERROR (@ErrorMessage, @ErrorSeverity, 1);
    RETURN -1;
END CATCH;
GO

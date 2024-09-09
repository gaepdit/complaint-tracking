USE ComplaintTracking;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE admin.CloseComplaints @AdminUserId nvarchar(450),
                                                @ComplaintsTVP admin.CloseComplaintsTableType READONLY
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   This procedure is used to administratively close complaints.

Input Parameters:
  @AdminUserId      - The GUID of the admin user executing the procedure.
  @ComplaintsTVP    - A table-valued parameter containing:
    Id                - The IDs of the complaints to administratively close.
    ReviewComment     - The review comment to add.

Example usage:

begin

    declare @adminUserId as varchar(450);
    declare @closeComplaints as admin.CloseComplaintsTableType;

    select @adminUserId = Id from AspNetUsers where Email = 'admin.user@example.com';

    insert into @closeComplaints
        (Id, ReviewComment)
    values
        (999998, 'Administratively closed with this review comment.'),
        (999999, 'Administratively closed with another review comment.');

    exec admin.CloseComplaints @AdminUserId = @adminUserId, @ComplaintsTVP = @closeComplaints

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
2021-11-22  DWaldron            Initial version (#496)
2021-12-08  DWaldron            Trim comments
2024-09-09  DWaldron            Updated for new CTS (#682)

*******************************************************************************/

    SET XACT_ABORT, NOCOUNT ON;
BEGIN TRY

    BEGIN TRANSACTION;

    insert into ComplaintTransitions
    (Id, ComplaintId, TransitionType, CommittedDate, CommittedByUserId, Comment, CreatedAt, CreatedById)
    select newid(),
           t.Id,
           'Closed',
           sysdatetimeoffset(),
           @AdminUserId,
           trim(t.ReviewComment),
           sysdatetimeoffset(),
           @AdminUserId
    from Complaints c
        inner join @ComplaintsTVP t
            on c.Id = t.Id
    where c.ComplaintClosed = convert(bit, 0)
      and c.IsDeleted = convert(bit, 0);

    update Complaints
    set Status              = 'AdministrativelyClosed',
        ComplaintClosed     = convert(bit, 1),
        ComplaintClosedDate = sysdatetimeoffset(),
        ReviewComments      = trim(t.ReviewComment),
        ReviewedById        = @AdminUserId,
        UpdatedAt           = sysdatetimeoffset(),
        UpdatedById         = @AdminUserId
    from Complaints c
        inner join @ComplaintsTVP t
            on c.Id = t.Id
    where c.ComplaintClosed = convert(bit, 0)
      and c.IsDeleted = convert(bit, 0);

    COMMIT TRANSACTION;

    select c.Id,
           c.Status,
           c.ComplaintClosed,
           c.ComplaintClosedDate,
           c.IsDeleted,
           c.ReviewComments,
           c.ReviewedById,
           c.UpdatedAt,
           c.UpdatedById
    from Complaints c
        inner join @ComplaintsTVP t
            on c.Id = t.Id;

    select r.ComplaintId,
           r.TransitionType,
           r.Comment,
           r.CommittedDate,
           r.CreatedById,
           r.CreatedAt
    from ComplaintTransitions r
        inner join @ComplaintsTVP t
            on r.ComplaintId = t.Id
    where r.TransitionType = 'Closed';

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

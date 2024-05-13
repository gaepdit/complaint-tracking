# Site Map

* `/` Home page with public search form and [menu bar](Menu%20bar.md) allowing access to staff pages.
* `/Support` (Help/support page)

## Public Pages

These pages are available to the public.

* `/` (Public complaint search)
* `/Complaint/{id}` (Public complaint detail view with list of Actions)
* `/Complaint/Attachment/{attachmentId}/{fileName}` (Public complaint attachment)

## Staff Pages

These pages are only available to logged-in staff.

* `/Staff` (Staff dashboard)

### Complaints

* `/Staff/Complaints` (Complaint search)

#### Complaint Details and Attachments

* `/Staff/Complaints/Details/{id}` (Complaint details with list of Actions and a new Action form)
* `/Staff/Complaints/Attachment/{attachmentId}/{fileName}` (Attachment download)
* `/Staff/Complaints/Attachment/Delete/{attachmentId}` (Delete Attachment)

#### Complaint Workflow

* `/Staff/Complaints/Add` (Add new complaint)
* `/Staff/Complaints/Approve/{id}` (Review and close a complaint)
* `/Staff/Complaints/Assign/{id}` (Transfer/reassign a complaint)
* `/Staff/Complaints/Delete/{id}` (Delete a complaint)
* `/Staff/Complaints/Edit/{id}` (Edit complaint details)
* `/Staff/Complaints/Reopen/{id}` (Reopen a closed complaint)
* `/Staff/Complaints/RequestReview/{id}` (Request a complaint to be reviewed & closed)
* `/Staff/Complaints/Restore/{id}` (Restore a deleted complaint)
* `/Staff/Complaints/Return/{id}` (Review and return a complaint)

### Complaint Actions

* `/Staff/Complaints/Details/{complaintId}#actions` (Embedded form for adding an action)
* `/Staff/ComplaintActions` (Search complaint actions)
* `/Staff/ComplaintActions/Edit/{actionId}` (Edit action details)
* `/Staff/ComplaintActions/Delete/{actionId}` (Delete an action)
* `/Staff/ComplaintActions/Restore/{actionId}` (Restore a deleted action)

## User Account

* `/Account` (View profile)
* `/Account/Login` (Work account login form)
* `/Account/Edit` (Edit contact info)

## Admin pages

### Reports

(Pages must be named "Reporting" because "Reports" is reserved by the ArcGIS application.)

* `/Admin/Reporting` (Management & error reports)
* `/Admin/Reporting/[report type]` (View report)
* `/Admin/Reporting/Export` (Export database archive)

### Site Maintenance

Maintenance pages available to Site Admin personnel to modify lookup tables used for drop-down lists. Editable items
comprise Action Types, Areas of Concern, and Offices.

* `/Admin/Maintenance`
* `/Admin/Maintenance/ActionTypes`
* `/Admin/Maintenance/Concerns`
* `/Admin/Maintenance/Offices`
* `/Admin/Maintenance/[maintenance option]/Add`
* `/Admin/Maintenance/[maintenance option]/Edit/{id}`

### User Management

* `/Admin/Users` (User search)
* `/Admin/Users/Details/{id}` (View user profile)
* `/Admin/Users/Edit/{id}` (Edit contact info)
* `/Admin/Users/EditRoles/{id}` (Edit roles)

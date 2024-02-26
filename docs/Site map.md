# Site Map

* `/` (Home page with links to public inquiry and employee access portals)

## Public Pages

These pages are available to the public.

* `/Public` (Public complaint search)
* `/Public/Complaints/{id}` (Public complaint detail view with list of Actions)
* `/Public/Complaints/Attachment/{attachmentId}/{fileName}` (Public complaint attachment download)

## Staff Pages

These pages are only available to logged-in staff.

* `/Staff` (Home page/staff dashboard)

### Complaints

* `/Staff/Complaints` (Complaint search)
* `/Staff/Complaints/Download` (Export search results)

#### Complaint Details and Attachments

* `/Staff/Complaints/Details/{id}` (Complaint details with list of Actions and a new Action form)
* `/Staff/Complaints/Attachment/{attachmentId}/{fileName}` (Attachment download)
* `/Staff/Complaints/Attachment/Delete/{attachmentId}/{fileName}` (Delete Attachment)

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

* `/Account/Login` (Work account login form)
* `/Account` (View profile)
* `/Account/Edit` (Edit contact info)
* `/Account/Support` (Help/support page)

## Admin pages

### Reports

* `/Admin/Reports` (Management & error reports)
* `/Admin/Reports/[report type]` (View report)
* `/Admin/Reports/Export` (Export database archive)

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

# Menu Bar

## Public

Shown when user is not logged in.

* {Logo} (`~/`)
* Search (`~/Public`)
* Sign In (`~/Account/Login`)

## Staff

Shown when staff is logged in.

* {Logo} (`~/Staff`)
* New Complaint (`~/Staff/Complaints/Add`)
* Complaint Search (`~/Staff/Complaints`)
* Action Search (`~/Staff/ComplaintActions`)
* More (Drop-down)
    * Reports (`~/Admin/Reports`)
    * CTS Users (`~/Admin/Users`)
    * Site Maintenance (`~/Admin/Maintenance`)
    * Public Portal (`~/Public`)
* Account (Drop-down)
    * You profile (`~/Account`)
    * Support (`~/Account/Support`)
    * Sign out (*form*)
* Theme toggle

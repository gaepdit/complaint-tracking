# Site Map

* `/` (links to public inquiry and employee access portals)

## Public pages

* `/Public` (public search)
* `/Public/Complaints/{id}` (public detail view)
* `/Public/Complaints/Attachment/{attachmentId}/{fileName}` (public attachment download)

### Redirects

| Redirect                        | To                        |
|---------------------------------|---------------------------|
| `/Public/ComplaintDetails/{id}` | `/Public/Complaints/{id}` |

## Account & login pages

* `/Account` (view profile)
* `/Account/Login`
* `/Account/Edit` (edit contact info)
* `/Account/Support` (help/support)

## Staff Dashboard

* `/Staff` (user dashboard)

## Complaint summary pages

* `/Staff/Complaints` (search)
* `/Staff/Complaints/Download` (search results export)
* `/Staff/Complaints/Export` (archive export)
* `/Staff/Complaints/Reports`

### Redirects

| Redirect                | To                          |
|-------------------------|-----------------------------|
| `/Complaints`           | `/Staff/Complaints`         |
| `/Complaints/Export`    | `/Staff/Complaints/Export`  |
| `/Complaints/Reports`   | `/Staff/Complaints/Reports` |
| `/Complaints/Reports/*` | `/Staff/Complaints/Reports` |

## Complaint details

* `/Staff/Complaints/Details/{id}`
* `/Staff/Complaints/Attachment/{attachmentId}/{fileName}`

## Complaint user actions

* `/Staff/Complaints/Add`
* `/Staff/Complaints/Approve/{id}`
* `/Staff/Complaints/Assign/{id}`
* `/Staff/Complaints/Delete/{id}`
* `/Staff/Complaints/Edit/{id}`
* `/Staff/Complaints/Reopen/{id}`
* `/Staff/Complaints/RequestReview/{id}`
* `/Staff/Complaints/Restore/{id}`
* `/Staff/Complaints/Return/{id}`

### Redirects

| Redirect                         | To                       |
|----------------------------------|--------------------------|
| `/Complaints/Details/{id}`       | `/Staff/Complaints/{id}` |
| `/Complaints/[user action]/{id}` | `/Staff/Complaints/{id}` |
| `/Staff/Complaints/Details`      | `/Staff/Complaints`      |

## Complaint Actions

* `/Staff/ComplaintActions` (search)
* `/Staff/Complaints/Details/{id}#actions` (embedded form)
* `/Staff/Complaints/Details/{id}/DeleteAction/{actionId}`
* `/Staff/Complaints/Details/{id}/EditAction/{actionId}`

### Redirects

| Redirect                                | To                        |
|-----------------------------------------|---------------------------|
| `/ComplaintActions`                     | `/Staff/ComplaintActions` |
| `/Complaints/Actions/{id}`              | `/Staff/ComplaintActions` |
| `/Complaints/EditAction/{actionid}`     | `/Staff/ComplaintActions` |
| `/Complaints/Details/{id}/DeleteAction` | `/Staff/Complaints/{id}`  |
| `/Complaints/Details/{id}/EditAction`   | `/Staff/Complaints/{id}`  |

## Users

* `/Admin/Users` (search)
* `/Admin/Users/Details/{id}`
* `/Admin/Users/Edit/{id}` (edit contact info)
* `/Admin/Users/EditRoles/{id}` (edit roles)

### Redirects

| Redirect               | To                          |
|------------------------|-----------------------------|
| `/Users`               | `/Admin/Users`              |
| `/Users/Details/{id}`  | `/Admin/Users/Details/{id}` |
| `/Users/Edit/{id}`     | `/Admin/Users/Details/{id}` |
| `/Admin/Users/Details` | `/Admin/Users`              |

## Site Maintenance

* `/Admin/Maintenance`
* `/Admin/Maintenance/Offices`
* `/Admin/Maintenance/ActionTypes`
* `/Admin/Maintenance/Concerns`
* `/Admin/Maintenance/[type]/Add`
* `/Admin/Maintenance/[type]/Edit/{id}`

### Redirects

| Redirect            | To                          |
|---------------------|-----------------------------|
| `/Maintenance`      | `/Admin/Maintenance`        |
| `/[type]`           | `/Admin/Maintenance/[type]` |
| `/[type]/Create`    | `/Admin/Maintenance/[type]` |
| `/[type]/Details/*` | `/Admin/Maintenance/[type]` |
| `/[type]/Edit/*`    | `/Admin/Maintenance/[type]` |

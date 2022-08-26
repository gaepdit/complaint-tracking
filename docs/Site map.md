# Site Map

* `/` (links to public inquiry and employee access portals)

## Public pages

* `/Public` (search)
* `/Public/Complaint/{id}` (detail view)

### Redirects

| Redirect                        | To                       |
|---------------------------------|--------------------------|
| `/Public/ComplaintDetails/{id}` | `/Public/Complaint/{id}` |

## Account & login pages

* `/Account` (view profile)
* `/Account/Login`

## Admin Dashboard

* `/Admin` (user dashboard)
* `/Admin/Support` (help/support)

## Complaint summary pages

* `/Admin/Complaints` (search)
* `/Admin/Complaints/Download` (search results export)
* `/Admin/Complaints/Export` (archive export)
* `/Admin/Complaints/Reports`

### Redirects

| Redirect                | To                          |
|-------------------------|-----------------------------|
| `/Complaints`           | `/Admin/Complaints`         |
| `/Complaints/Export`    | `/Admin/Complaints/Export`  |
| `/Complaints/Reports`   | `/Admin/Complaints/Reports` |
| `/Complaints/Reports/*` | `/Admin/Complaints/Reports` |

## Complaint details & user actions

* `/Admin/Complaints/Create`
* `/Admin/Complaints/Details/{id}`
* `/Admin/Complaints/Details/Approve/{id}`
* `/Admin/Complaints/Details/Assign/{id}`
* `/Admin/Complaints/Details/Delete/{id}`
* `/Admin/Complaints/Details/Edit/{id}`
* `/Admin/Complaints/Details/Reopen/{id}`
* `/Admin/Complaints/Details/RequestReview/{id}`
* `/Admin/Complaints/Details/Restore/{id}`
* `/Admin/Complaints/Details/Return/{id}`

### Redirects

| Redirect                         | To                       |
|----------------------------------|--------------------------|
| `/Complaints/Details/{id}`       | `/Admin/Complaints/{id}` |
| `/Complaints/[user action]/{id}` | `/Admin/Complaints/{id}` |
| `/Admin/Complaints/Details`      | `/Admin/Complaints`      |

## Complaint Actions

* `/Admin/ComplaintActions` (search)
* `/Admin/Complaints/Details/{id}#actions` (embedded form)
* `/Admin/Complaints/Details/{id}/DeleteAction/{actionId}`
* `/Admin/Complaints/Details/{id}/EditAction/{actionId}`

### Redirects

| Redirect                                      | To                        |
|-----------------------------------------------|---------------------------|
| `/ComplaintActions`                           | `/Admin/ComplaintActions` |
| `/Complaints/Actions/{id}`                    | `/Admin/ComplaintActions` |
| `/Complaints/EditAction/{actionid}`           | `/Admin/ComplaintActions` |
| `/Admin/Complaints/Details/{id}/DeleteAction` | `/Admin/Complaints/{id}`  |
| `/Admin/Complaints/Details/{id}/EditAction`   | `/Admin/Complaints/{id}`  |

## Users

* `/Admin/Users` (search)
* `/Admin/Users/Details/{id}`
* `/Admin/Users/Edit/{id}` (edit roles)

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
* `/Admin/Maintenance/[type]/Create`
* `/Admin/Maintenance/[type]/Edit/{id}`

### Redirects

| Redirect            | To                          |
|---------------------|-----------------------------|
| `/Maintenance`      | `/Admin/Maintenance`        |
| `/[type]`           | `/Admin/Maintenance/[type]` |
| `/[type]/Create`    | `/Admin/Maintenance/[type]` |
| `/[type]/Details/*` | `/Admin/Maintenance/[type]` |
| `/[type]/Edit/*`    | `/Admin/Maintenance/[type]` |

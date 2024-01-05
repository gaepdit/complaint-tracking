# Site URL Redirects

This document defines routes from the old version of the application that should be redirected according to the
new [Site Map](Site%20map.md).

## Public pages

| Redirect                        | To                        |
|---------------------------------|---------------------------|
| `/Public/ComplaintDetails/{id}` | `/Public/Complaints/{id}` |

## Complaint pages

| Redirect                             | To                       |
|--------------------------------------|--------------------------|
| `/Complaints`                        | `/Staff/Complaints`      |
| `/Complaints/Details/{id}`           | `/Staff/Complaints/{id}` |
| `/Complaints/[workflow action]/{id}` | `/Staff/Complaints/{id}` |
| `/Staff/Complaints/Details`          | `/Staff/Complaints`      |

## Complaint Actions

| Redirect                                | To                        |
|-----------------------------------------|---------------------------|
| `/ComplaintActions`                     | `/Staff/ComplaintActions` |
| `/Complaints/Actions/{id}`              | `/Staff/ComplaintActions` |
| `/Complaints/EditAction/{actionid}`     | `/Staff/ComplaintActions` |
| `/Complaints/Details/{id}/DeleteAction` | `/Staff/Complaints/{id}`  |
| `/Complaints/Details/{id}/EditAction`   | `/Staff/Complaints/{id}`  |

## Users

| Redirect               | To                          |
|------------------------|-----------------------------|
| `/Users`               | `/Admin/Users`              |
| `/Users/Details/{id}`  | `/Admin/Users/Details/{id}` |
| `/Users/Edit/{id}`     | `/Admin/Users/Details/{id}` |
| `/Admin/Users/Details` | `/Admin/Users`              |

## Site Maintenance

| Redirect                          | To                                        |
|-----------------------------------|-------------------------------------------|
| `/Maintenance`                    | `/Admin/Maintenance`                      |
| `/[maintenance option]`           | `/Admin/Maintenance/[maintenance option]` |
| `/[maintenance option]/Create`    | `/Admin/Maintenance/[maintenance option]` |
| `/[maintenance option]/Details/*` | `/Admin/Maintenance/[maintenance option]` |
| `/[maintenance option]/Edit/*`    | `/Admin/Maintenance/[maintenance option]` |
| `/Reports`                        | `/Admin/Reports`                          |
| `/Reports/[report type]`          | `/Admin/Reports/[report type]`            |
| `/Export`                         | `/Admin/Reports/Export`                   |

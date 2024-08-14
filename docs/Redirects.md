# Site URL Redirects

This document defines routes from the old version of the application that should be redirected according to
the [Site Map](Site%20map.md). Also included are partial/incomplete URLs.

Rules are approximately ordered from the most frequently matched rule to the least frequently matched rule.

## Public pages

| Redirect                                       | To                                     |
|------------------------------------------------|----------------------------------------|
| `/Public`                                      | `/`                                    |
| `/Public/ComplaintDetails/{id}`                | `/Complaint/{id}`                      |
| `/Public/Attachment/{attachmentId}[/filename]` | `/Complaint/Attachment/{attachmentId}` |
| `/Complaint`                                   | `/`                                    |
| `/Complaint/Attachment`                        | `/`                                    |

## Staff complaint pages

| Redirect                                           | To                                            |
|----------------------------------------------------|-----------------------------------------------|
| `/Complaints`                                      | `/Staff/Complaints`                           |
| `/Complaints/Create`                               | `/Staff/Complaints/Add`                       |
| `/Complaints/Details/{id}`                         | `/Staff/Complaints/Details/{id}`              |
| `/Complaints/Actions/{id}`                         | `/Staff/Complaints/Details/{id}`              |
| `/Complaints/Attachment/{attachmentId}[/filename]` | `/Staff/Complaints/Attachment/{attachmentId}` |
| `/Staff/Complaints/Details`                        | `/Staff/Complaints`                           |
| `/Staff/Complaints/Attachment`                     | `/Staff/Complaints`                           |
| `/Admin`                                           | `/Staff`                                      |

## Complaint Actions

| Redirect            | To                        |
|---------------------|---------------------------|
| `/ComplaintActions` | `/Staff/ComplaintActions` |

## Reporting

| Redirect   | To                        |
|------------|---------------------------|
| `/Reports` | `/Admin/Reporting`        |
| `/Export`  | `/Admin/Reporting/Export` |

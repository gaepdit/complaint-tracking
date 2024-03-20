# Site URL Redirects

This document defines incomplete URLs or routes from the old version of the application that should be redirected
according to the [Site Map](Site%20map.md).

## Public pages

| Redirect                            | To                                     |
|-------------------------------------|----------------------------------------|
| `/Public`                           | `/Index`                               |
| `/Public/ComplaintDetails/{id}`     | `/Complaint/{id}`                      |
| `/Public/Attachment/{attachmentId}` | `/Complaint/Attachment/{attachmentId}` |
| `/Complaint`                        | `/Index`                               |

## Staff complaint pages

| Redirect                                | To                                 |
|-----------------------------------------|------------------------------------|
| `/Complaints`                           | `/Staff/Index`                     |
| `/Complaints/Details/{id}`              | `/Staff/Complaints/{id}`           |
| `/Complaints/Attachment/{attachmentId}` | `/Staff/Attachment/{attachmentId}` |
| `/Staff/Complaints`                     | `/Staff/Index`                     |

## Complaint Actions

| Redirect                   | To                               |
|----------------------------|----------------------------------|
| `/ComplaintActions`        | `/Staff/ComplaintActions/Index`  |
| `/Complaints/Actions/{id}` | `/Staff/Complaints/{id}#actions` |

## Users

| Redirect              | To                          |
|-----------------------|-----------------------------|
| `/Users`              | `/Admin/Users/Index`        |
| `/Users/Details/{id}` | `/Admin/Users/Details/{id}` |

## Site Maintenance

| Redirect                          | To                                        |
|-----------------------------------|-------------------------------------------|
| `/Maintenance`                    | `/Admin/Maintenance`                      |
| `/[maintenance option]`           | `/Admin/Maintenance/[maintenance option]` |
| `/[maintenance option]/Details/*` | `/Admin/Maintenance/[maintenance option]` |
| `/Reports`                        | `/Admin/Reporting` *                      |
| `/Reports/[report type]`          | `/Admin/Reporting/[report type]` *        |
| `/Export`                         | `/Admin/Reporting/Export`                 |

* If possible, given implementation of ArcGIS.

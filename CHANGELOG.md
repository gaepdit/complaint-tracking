# Changelog

## [2025.4.10] - 2025-04-10

- This version includes security updates.

## [2025.3.31] - 2025-03-31

### Fixed

- The Excel export tool was broken for the complaint action search form. 

## [2025.3.24] - 2025-03-24

### Added

- Improved display of outage notifications.

## [2025.3.7] - 2025-03-07

### Fixed

- Emails were not getting sent to assignors when complaints were reassigned to a different office.

## [2025.3.5] - 2025-03-05

### Fixed

- The email templates have been updated for clarity when no comments have been entered by the user.

- The complaint search form has been fixed in several ways:
  * Searching by "Facility ID" was broken.
  * Searching by "Source Contact" was broken.
  * Complaint city and county have been added back to the search results table and Excel export.
  * The "Source Contact" search fields were reorganized and labeled more clearly.
  * The Source Contact data in the search results and Excel export are labeled more clearly.

## [2025.2.3] - 2025-02-03

- Updated to .NET 9.

## [2025.1.10.1] - 2025-01-10

### Fixed

- The updated file attachment drag and drop feature was not always working correctly due to a caching issue.

## [2025.1.10] - 2025-01-10

### Changed

- The complaint action search page performance was improved.

## [2025.1.8] - 2025-01-08

### Changed

- File attachment drag and drop usability was improved.
- Display of deleted items was improved.

### Added

- You can now filter complaints by assigned office on the Complaint Action search page.
- Complaint Action search results can be exported to Excel.

### Fixed

- Deleted complaints were not labeled correctly on the Complaint Action search page.
- Some search terms were not preserved on the Complaint Action search page when sorting.

## [2024.12.23] - 2024-12-23

### Fixed

- The Find by ID form on the public home page could break the search from in some circumstances.
- Search results tables had some styling issues on smaller screens in dark mode.

## [2024.11.5] - 2024-11-05

### Fixed

- If a user's email changes in the SOG account system, the email in the CTS database will now update the next time the
  user logs in.

## [2024.10.16] - 2024-10-16

### Fixed

- Search form data validation has been improved.

## [2024.10.2] - 2024-10-02

### Added

- The display of search results has been improved on smaller screens.
- The navigation menu display has been improved on extra-small screens.

### Fixed

- The color mode toggle now works on older browsers.
- The data export tool was exporting the wrong complaints.

## [2024.9.11] - 2024-09-11

### Added

- The complaint search page now has an option to filter for unassigned complaints.

### Fixed

- Notification emails were not being sent to the assignor for unassigned complaints.

## [2024.8.29] - 2024-08-29

### Added

- A new "Super-User Account Admin" role has been added.

## [2024.8.23] - 2024-08-23

### Added

- Complaint details pages (staff & public) have a table of contents to aid in navigating the page.
- Photo attachments now open in a gallery view when clicked.
- Attachments can now be added to a new or existing complaint using drag-and-drop in addition to the file picker.
- The staff search page can now filter complaints based on whether they have attachments or not.
- The public search page now allows searching by city. Also, the status drop-down has been simplified.
- Some web security features have been enabled.

### Fixed

- Some page styling has been improved.

## [2024.7.26] - 2024-07-26

### Fixed

- GEMA staff were unable to log in.
- Links to the old Add Complaint page now correctly redirect to the new URL.

## [2024.7.25] - 2024-07-25

### Fixed

- The Complaint Action search results had broken links.

## [2024.7.24] - 2024-07-24

_This version represents a complete rewrite of the Complaint Tracking System._

New login system. New design library. Updated workflows. New public complaint page. Dark mode. And more! We hope you
like it!

[2025.4.10]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.4.10
[2025.3.31]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.3.31
[2025.3.24]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.3.24
[2025.3.7]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.3.7
[2025.3.5]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.3.5
[2025.2.3]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.2.3
[2025.1.10.1]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.1.10.1
[2025.1.10]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.1.10
[2025.1.8]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2025.1.8
[2024.12.23]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.12.23
[2024.11.5]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.11.5
[2024.10.16]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.10.16
[2024.10.2]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.10.2
[2024.9.11]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.9.11
[2024.8.29]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.8.29
[2024.8.23]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.8.23
[2024.7.26]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.7.26
[2024.7.25]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.7.25
[2024.7.24]: https://github.com/gaepdit/complaint-tracking/releases/tag/v2024.7.24

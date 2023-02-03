# Complaint Tracking System Application

The Complaint Tracking System (CTS) is an online application to allow EPD staff to enter, assign, review, and close complaints received from the public.

[![.NET Test](https://github.com/gaepdit/complaint-tracking/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/gaepdit/complaint-tracking/actions/workflows/dotnet-test.yml)
[![CodeQL](https://github.com/gaepdit/complaint-tracking/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/complaint-tracking/actions/workflows/codeql-analysis.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit.complaint-tracking&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit.complaint-tracking)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit.complaint-tracking&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit.complaint-tracking)

---

## Background and project requirements

Public complaints are time-critical and high-profile public information. The CTS is used by staff throughout EPD.

* The application will allow EPD staff to enter new complaints, review and update existing complaints, and remove complaints erroneously entered.
* The admin side of the application will be restricted to authenticated EPD employees.
* A public website will be available for reviewing or searching for complaints.

## Info for developers

This is an ASP.NET 6 web application.

### Prerequisites for development

+ [Visual Studio](https://www.visualstudio.com/vs/) or similar
+ [.NET 6.0 SDK](https://dotnet.microsoft.com/download)

### Project organization

The solution contains the following projects:

* **Domain** — A class library containing the data models and business logic.
* **AppServices** — A class library containing the services used by an application to interact with the domain.
* **LocalRepository** — A class library implementing the repository and services without using a database (for local development).
* **EfRepository** — A class library implementing the repository and services using LocalDb and Entity Framework.
* **WebApp** — The front end web application.

There are also corresponding unit test projects for each, plus a **TestData** project containing test data for development and testing.

### Launch profiles

There are two launch profiles:

* **WebApp Local** — This profile uses data in the "TestData" project and either uses the data in-memory or builds and seeds a database. A local user account can be used to simulate authentication, or an Azure AD account can be configured.

    You can configure various development scenarios by creating an "appsettings.Local.json" file in the "WebApp" folder with the following settings:

    - *UseInMemoryData* — Uses in-memory data when `true`. Connects to a SQL Server database when `false`.
    - *UseEfMigrations* - Uses Entity Framework migrations when `true`. When set to `false`, the database is deleted and recreated on each run. (Only applies if *UseInMemoryData* is `false`.)
    - *UseAzureAd* — If `true`, the app must be registered in the Azure portal, and configuration settings added in the "AzureAd" settings section. If `false`, authentication is simulated using test user data.
    - *LocalUserIsAuthenticated* — Simulates a successful login with a test account when `true`. Simulates a failed login when `false`. (Only applies if *UseAzureAd* is `false`.)
    - *LocalUserIsAdmin* — Adds all App Roles to the logged in account when `true` or no roles when `false`. (Applies whether *UserAzureAd* is `true` or `false`.)

* **WebApp Dev Server** — This profile connects to a remote database server for data. *To use this profile, you must add the "appsettings.Development.json" file from the "app-config" repo.*

    Most development should be done using the Local profile. The Dev Server profile is only needed when specifically troubleshooting issues with the database server or SOG account.

Here's a visualization of how each launch profile (plus the `UseInMemoryData` setting) configures the data storage at runtime.
```mermaid
flowchart LR
    subgraph SPL["'Local' launch profile & UseInMemoryData = true"]
        direction LR
        D[Domain]
        T["Test Data (in memory)"]
        R[Local Repository]
        A[App Services]
        W([Web App])

        W --> A
        A --> D
        A --> R
        R --> T
        T --> D
    end
```

```mermaid
flowchart LR
    subgraph SPB["'Local' launch profile & UseInMemoryData = false"]
        direction LR
        D[Domain]
        T[Test Data]
        R[EfRepository]
        A[App Services]
        W([Web App])
        B[(LocalDB)]

        W --> A
        A --> D
        R --> B
        A --> R
        T -->|Seed| B
        B -.-> D
    end
```

```mermaid
flowchart LR
    subgraph SPD["'Dev Server' launch profile"]
        direction LR
        D[Domain]
        R[EfRepository]
        A[App Services]
        W([Web App])
        B[(DB Server)]

        W --> A
        A --> D
        A --> R
        R ==>|VPN| B -.-> D
    end
```

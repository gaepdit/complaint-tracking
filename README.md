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

This is an ASP.NET 6 web application. The CTS is currently being completely rebuilt in the `vNext` branch.

### Prerequisites for development

+ [Visual Studio](https://www.visualstudio.com/vs/) or similar
+ [.NET 6.0 SDK](https://dotnet.microsoft.com/download)

### Project organization

The solution contains the following projects:

* **Domain** — A class library containing the data models, business logic, and repository interfaces.
* **AppServices** — A class library containing the services used by an application to interact with the domain.
* **LocalRepository** — A class library implementing the repositories and data stores using static in-memory test data (for local development).
* **EfRepository** — A class library implementing the repositories and data stores using Entity Framework and a database (as specified by the configured connection string).
* **WebApp** — The front end web application.

There are also corresponding unit test projects for each, plus a **TestData** project containing test data for development and testing.

### Development settings

The following development settings configure the data stores and authentication. To change these settings, add an "appsettings.Development.json" file in the root of the "WebApp" folder with a `DevSettings` section. (Optionally, the settings file can also include an `AzureAd` section or database connection string as needed.)

- *UseInMemoryData* — 
    - When `true`, the `LocalRepository` project is used for repositories and data stores. Data is initially populated from the `TestData` project. 
    - When `false`, the `EfRepository` project is used, and a SQL Server database (as specified by the connection string) is seeded from the `TestData` project. <small>(If the connection string is missing, then a temporary EF Core in-memory database provider is used. This option is included for convenience and is not recommended.)</small>
- *UseEfMigrations* — Uses Entity Framework migrations when `true`. When `false`, the database is deleted and recreated on each run. (Only applies if *UseInMemoryData* is `false`.)
- *UseAzureAd* — If `true`, connects to Azure AD for user authentication. (The app must be registered in the Azure portal, and configuration added to the settings file.) If `false`, authentication is simulated using test user data.
- *LocalUserIsAuthenticated* — Simulates a successful login with a test account when `true`. Simulates a failed login when `false`. (Only applies if *UseAzureAd* is `false`.)
- *LocalUserIsAdmin* — Adds all App Roles to the logged in account when `true` or no roles when `false`. (Applies whether *UserAzureAd* is `true` or `false`.)
- *UseInMemoryFiles* — If `true`, files are seeded from the TestData project and stored in memory. If `false`, attachment files are saved/loaded from the file system.

When running in the production environment, the settings are automatically set as follows:

```csharp
UseInMemoryData = false,
UseEfMigrations = true,
UseAzureAd = true,
LocalUserIsAuthenticated = false,
LocalUserIsAdmin = false,
UseInMemoryFiles = false,
```

Here's a visualization of how the settings configure data storage at runtime.

```mermaid
flowchart LR
    subgraph SPL["'UseInMemoryData' = true"]
        direction LR
        D[Domain]
        T["Test Data (in memory)"]
        R[Local Repositories]
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
    subgraph SPB["'UseInMemoryData' = false"]
        direction LR
        D[Domain]
        T[Test Data]
        R[EF Repositories]
        A[App Services]
        W([Web App])
        B[(Database)]

        W --> A
        A --> D
        R --> B
        A --> R
        T -->|Seed| B
        B --> D
    end
```

```mermaid
flowchart LR
    subgraph SPD["Production or staging environment"]
        direction LR
        D[Domain]
        R[EF Repositories]
        A[App Services]
        W([Web App])
        B[(Database)]

        W --> A
        A --> D
        A --> R
        R --> B
        B --> D
    end
```

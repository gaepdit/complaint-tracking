# Complaint Tracking System Application

The Complaint Tracking System (CTS) is an online application to allow EPD staff to enter, assign, review, and close complaints received from the public.

[![.NET Test](https://github.com/gaepdit/complaint-tracking/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/complaint-tracking/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/gaepdit/complaint-tracking/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/complaint-tracking/actions/workflows/codeql-analysis.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit.complaint-tracking&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit.complaint-tracking)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit.complaint-tracking&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit.complaint-tracking)

## Background and project requirements

Public complaints are time-critical and high-profile public information. The CTS is used by staff throughout EPD.

* The application will allow EPD staff to enter new complaints, review and update existing complaints, and remove complaints erroneously entered.
* The admin side of the application will be restricted to authenticated EPD employees.
* A public web site will be available for reviewing or searching for complaints.

# Info for developers

CTS is an ASP.NET 6 web application.

## Prerequisites for development

+ [Visual Studio](https://www.visualstudio.com/vs/) or similar
+ [.NET 6.0 SDK](https://dotnet.microsoft.com/download)
+ [NPM](https://www.npmjs.com/) or [PNPM](https://pnpm.io/)

## Project organization

The solution contains the following projects:

* **Domain** — A class library containing the data models and business logic.
* **AppServices** — A class library containing the services used by an application to interact with the domain.
* **LocalRepository** — A class library implementing the repository and services without using a database.
* **Infrastructure** — A class library implementing the repository and services using Entity Framework.
* **WebApp** — The front end web application written in ASP.NET Razor Pages.

There are also corresponding unit test projects for each, plus a **TestData** project containing test data for development/testing purposes.

## Getting started

In the web app folder, run `pnpm install` or `npm install` to install dependencies.

```
cd src/WebApp
npm install
dotnet run
```

### Launch profiles

There are two launch profiles:

* **WebApp Local** — This profile does not connect to any external server. A local user account is used for authentication.

    You can modify some development settings by creating an "appsettings.Local.json" file to test various scenarios:

    - *AuthenticatedUser* — Simulates a successful login with the test account when `true`. Simulates a failed login when `false`.
    - *BuildLocalDb* — Uses LocalDB when `true`. Uses in-memory data when `false`.
    - *UseEfMigrations* - Uses Entity Framework migrations when `true`. Deletes and recreates database when `false`. (Only used if *BuildLocalDb* is `true`.)

* **WebApp Dev Server** — This profile connects to the remote Dev database server for data and requires an SOG account to log in. *To use this profile, you must add the "appsettings.Development.json" file from the "app-config" repo.*

Most development should be done using the Local profile. The Dev Server profile is only needed when specifically troubleshooting issues with the database server or SOG account.

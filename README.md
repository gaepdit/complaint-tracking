# Complaint Tracking System Application

The Complaint Tracking System (CTS) is an online application to allow EPD staff to enter, assign, review, and close complaints received from the public.

![Uptime](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/gaepdit/status/main/api/complaint-tracking-system/uptime.json)
![Response time](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/gaepdit/status/main/api/complaint-tracking-system/response-time.json)

[![CodeQL](https://github.com/gaepdit/complaint-tracking/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/complaint-tracking/actions/workflows/codeql-analysis.yml)
[![Raygun Deployment Tracking](https://github.com/gaepdit/complaint-tracking/actions/workflows/raygun-deployment.yml/badge.svg)](https://github.com/gaepdit/complaint-tracking/actions/workflows/raygun-deployment.yml)

## General project requirements

Public complaints are time-critical and high-profile public information. The CTS is used by staff throughout EPD.

* The application will allow EPD staff to enter new complaints, review and update existing complaints, and remove complaints erroneously entered.

* The application will be restricted to authenticated EPD employees.

* A public web site for review or entry of complaints is under consideration.

# Info for developers

CTS is an ASP.NET 6 web application.

## Prerequisites for development

+ [Visual Studio](https://www.visualstudio.com/vs/) or similar
+ [.NET SDK](https://dotnet.microsoft.com/download/download)
+ Node/NPM must be installed
+ Gulp must be installed as a global package: `npm install -g gulp`

## Getting started

Clone the repo and check out the `develop` branch:

```
git clone git@bitbucket.org:gaepdit/complaint-tracking-system.git
cd complaint-tracking
git checkout develop
```

Restore packages, build, and run:

```
cd ComplaintTracking
npm install
gulp
dotnet restore
dotnet build
dotnet run
```

## Configuration

Optionally, add `appsettings.{configuration}.json` files to modify settings for different configurations. If "DefaultConnection" Connection String is added, then SQL Server will be used with that string. Otherwise a Sqlite database will be created and seeded with test data.

## UI testing

Cypress.io is used for UI tests. Restore packages and start the Cypress app (requires app to be running -- see above):

```
cd tests
npm install
npm run cypress
```

## Versioning

Releases are tagged with the environment and date like `ENV/yyyy.m.d`. For example, `prod/2020.4.1`.

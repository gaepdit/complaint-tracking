### Entity Framework database migrations

Instructions for adding a new Entity Framework database migration:

1. Build the solution.

2. Open a command prompt to the "./src/EfRepository/" folder.

3. Run the following command with an appropriate migration name:

   `dotnet ef migrations add NAME_OF_MIGRATION --msbuildprojectextensionspath ..\..\.artifacts\EfRepository\obj\`

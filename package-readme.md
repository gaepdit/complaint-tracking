# NuGet Package maintenance

Some NuGet packages have been added directly to work around vulnerable dependencies in other packages.

- `System.Security.Cryptography.Xml` 10.0.10 has added to `AppServices` to avoid a vulnerable version in
  `Microsoft.Identity.Web`.

- `System.Security.Cryptography.Xml` 10.0.10 has added to `EfRepositoryTests` to avoid a vulnerable version in
  `EfCore.TestSupport`.

- `SQLitePCLRaw.lib.e_sqlite3` 2.1.12 has added to `EfRepositoryTests` to avoid a vulnerable version in
  `EfCore.TestSupport` (via `Microsoft.EntityFrameworkCore.Sqlite`).

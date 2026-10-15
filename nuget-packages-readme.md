# NuGet Package maintenance

The following NuGet packages have been added directly to work around vulnerable dependencies in other packages.

- `System.Security.Cryptography.Xml` was added to `AppServices` to avoid a vulnerable version referenced in
  `Microsoft.Identity.Web`.

- `System.Security.Cryptography.Xml` was added to `EfRepositoryTests` to avoid a vulnerable version referenced in
  `EfCore.TestSupport`.

- `SQLitePCLRaw.lib.e_sqlite3` was added to `EfRepositoryTests` and pinned to version 2.x to avoid a vulnerable version
  referenced in `EfCore.TestSupport` (via `Microsoft.EntityFrameworkCore.Sqlite`).

- `System.Security.Cryptography.Pkcs` was added to `WebApp` to avoid a vulnerable version
  referened in `Microsoft.Data.SqlClient`.

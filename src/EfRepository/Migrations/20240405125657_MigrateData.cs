using Cts.EfRepository.Migrations.DataMigration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cts.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class MigrateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Identity
            migrationBuilder.Sql(Migrate.AspNetUsers);
            migrationBuilder.Sql(Migrate.AspNetRoles);
            migrationBuilder.Sql(Migrate.AspNetUserRoles);

            // Lookups
            migrationBuilder.Sql(Migrate.ActionTypes);
            migrationBuilder.Sql(Migrate.Concerns);
            migrationBuilder.Sql(Migrate.Offices);
            
            // Application data
            migrationBuilder.Sql(Migrate.Complaints);

            // Ancillary data
            migrationBuilder.Sql(Migrate.EmailLogs);
        }
    }
}

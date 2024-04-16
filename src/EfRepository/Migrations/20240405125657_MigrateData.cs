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
            // Disable foreign key constraint
            migrationBuilder.Sql("ALTER TABLE AspNetUsers NOCHECK CONSTRAINT FK_AspNetUsers_Offices_OfficeId");

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
            migrationBuilder.Sql(Migrate.ComplaintActions);
            migrationBuilder.Sql(Migrate.ComplaintTransitions);
            migrationBuilder.Sql(Migrate.Attachments);

            // Ancillary data
            migrationBuilder.Sql(Migrate.EmailLogs);

            // Reenable foreign key constraint
            migrationBuilder.Sql("ALTER TABLE AspNetUsers WITH CHECK CHECK CONSTRAINT FK_AspNetUsers_Offices_OfficeId");
        }
    }
}

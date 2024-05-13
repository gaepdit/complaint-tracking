using Cts.EfRepository.Migrations.DataMigration;
using Cts.EfRepository.Migrations.UserDefinedFunction;
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
            // Add function to consolidate duplicate Users
            migrationBuilder.Sql(Function.FixUserId);
            
            // Disable foreign key constraint
            migrationBuilder.Sql("ALTER TABLE AspNetUsers NOCHECK CONSTRAINT FK__AspNetUsers_Offices_OfficeId");
            
            // Add archival user data columns
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers ADD MigratedEmail nvarchar(42)");
            migrationBuilder.Sql("ALTER TABLE dbo.AspNetUsers ADD OracleEmail nvarchar(42)");

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
            migrationBuilder.Sql("ALTER TABLE AspNetUsers WITH CHECK CHECK CONSTRAINT FK__AspNetUsers_Offices_OfficeId");

            // Remove function to consolidate duplicate Users
            migrationBuilder.Sql(Function.DropFixUserId);
        }
    }
}

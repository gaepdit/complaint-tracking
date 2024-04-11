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
            migrationBuilder.Sql(Migrate.ActionTypes);
            migrationBuilder.Sql(Migrate.EmailLogs);
        }
    }
}

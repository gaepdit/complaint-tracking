using Cts.EfRepository.Migrations.DatabaseView;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cts.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add database views
            // migrationBuilder.Sql(View.OpenComplaintsView);
            // migrationBuilder.Sql(View.ClosedComplaintsView);
            // migrationBuilder.Sql(View.ClosedComplaintActionsView);
            migrationBuilder.Sql(View.RecordsCountView);
        }
    }
}

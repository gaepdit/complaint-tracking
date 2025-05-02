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
            // Code to add database views has been removed. These views rely on the RegexReplace function from the
            // SqlServerRegexFunctions repo. Since that function is not available when creating a new database, the
            // migration would cause errors.
            // 
            // The views that were added here are only used by the CTS data archive tool. To enable data archiving,
            // add the regex functions and the following views to the database.
            // 
            // * SqlServerRegexFunctions: https://github.com/gaepdit/SqlServerRegexFunctions
            // 
            // * Database views (in the `DatabaseViews` folder):
            //   - View.OpenComplaintsView.sql
            //   - View.ClosedComplaintsView.sql
            //   - View.ClosedComplaintActionsView.sql
            //   - View.RecordsCountView.sql
        }
    }
}

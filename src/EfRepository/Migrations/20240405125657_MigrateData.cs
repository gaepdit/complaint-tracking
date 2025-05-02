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
            // The data migration scripts were only needed for the deployment of CTS Model 4 on 2024-07-24
            // and have been removed. For background, see:
            // * https://github.com/gaepdit/complaint-tracking/pull/747
            // * https://github.com/gaepdit/complaint-tracking/commit/35be8bf5
            // * https://github.com/gaepdit/complaint-tracking/releases/tag/archive%2Fmodel-four-init
        }
    }
}

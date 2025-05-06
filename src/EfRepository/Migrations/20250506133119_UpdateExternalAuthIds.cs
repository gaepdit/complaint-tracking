using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cts.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExternalAuthIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ObjectIdentifier",
                table: "AspNetUsers",
                newName: "EntraIdSubjectId");

            migrationBuilder.RenameIndex(
                name: "missing_index_198_197",
                table: "AspNetUsers",
                newName: "missing_index_198_197_A");

            migrationBuilder.AddColumn<string>(
                name: "OktaSubjectId",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "missing_index_198_197_B",
                table: "AspNetUsers",
                column: "OktaSubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "missing_index_198_197_B",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OktaSubjectId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "EntraIdSubjectId",
                table: "AspNetUsers",
                newName: "ObjectIdentifier");

            migrationBuilder.RenameIndex(
                name: "missing_index_198_197_A",
                table: "AspNetUsers",
                newName: "missing_index_198_197");
        }
    }
}

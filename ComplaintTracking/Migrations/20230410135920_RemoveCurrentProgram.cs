using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplaintTracking.Migrations
{
    public partial class RemoveCurrentProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentProgram",
                table: "Complaints");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentProgram",
                table: "Complaints",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}

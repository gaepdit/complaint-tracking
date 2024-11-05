using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cts.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAuditingDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AccountCreatedAt",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AccountUpdatedAt",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "MostRecentLogin",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProfileUpdatedAt",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccountUpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MostRecentLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileUpdatedAt",
                table: "AspNetUsers");
        }
    }
}

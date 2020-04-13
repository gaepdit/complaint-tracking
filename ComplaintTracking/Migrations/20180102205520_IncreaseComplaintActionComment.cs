using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ComplaintTracking.Migrations
{
    public partial class IncreaseComplaintActionComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "ComplaintActions",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "ComplaintActions",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

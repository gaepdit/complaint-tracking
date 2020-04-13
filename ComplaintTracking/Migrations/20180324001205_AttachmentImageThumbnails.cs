using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ComplaintTracking.Migrations
{
    public partial class AttachmentImageThumbnails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageThumbnail",
                table: "Attachments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsImage",
                table: "Attachments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageThumbnail",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "IsImage",
                table: "Attachments");
        }
    }
}

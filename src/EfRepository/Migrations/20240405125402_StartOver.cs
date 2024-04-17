using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cts.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class StartOver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename old tables
            migrationBuilder.RenameTable(name: "AspNetRoleClaims", newName: "_archive_AspNetRoleClaims");
            migrationBuilder.RenameTable(name: "AspNetRoles", newName: "_archive_AspNetRoles");
            migrationBuilder.RenameTable(name: "AspNetUserClaims", newName: "_archive_AspNetUserClaims");
            migrationBuilder.RenameTable(name: "AspNetUserLogins", newName: "_archive_AspNetUserLogins");
            migrationBuilder.RenameTable(name: "AspNetUserRoles", newName: "_archive_AspNetUserRoles");
            migrationBuilder.RenameTable(name: "AspNetUsers", newName: "_archive_AspNetUsers");
            migrationBuilder.RenameTable(name: "AspNetUserTokens", newName: "_archive_AspNetUserTokens");
            migrationBuilder.RenameTable(name: "Attachments", newName: "_archive_Attachments");
            migrationBuilder.RenameTable(name: "ComplaintActions", newName: "_archive_ComplaintActions");
            migrationBuilder.RenameTable(name: "Complaints", newName: "_archive_Complaints");
            migrationBuilder.RenameTable(name: "ComplaintTransitions", newName: "_archive_ComplaintTransitions");
            migrationBuilder.RenameTable(name: "EmailLogs", newName: "_archive_EmailLogs");
            migrationBuilder.RenameTable(name: "LookupActionTypes", newName: "_archive_LookupActionTypes");
            migrationBuilder.RenameTable(name: "LookupConcerns", newName: "_archive_LookupConcerns");
            migrationBuilder.RenameTable(name: "LookupCounties", newName: "_archive_LookupCounties");
            migrationBuilder.RenameTable(name: "LookupOffices", newName: "_archive_LookupOffices");
            migrationBuilder.RenameTable(name: "LookupStates", newName: "_archive_LookupStates");

            // Create new tables
            migrationBuilder.CreateTable(
                name: "ActionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ActionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Concerns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Concerns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Sender = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Recipients = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CopyRecipients = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TextBody = table.Column<string>(type: "nvarchar(max)", maxLength: 15000, nullable: true),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EmailLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK__AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ObjectIdentifier = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK__AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Offices", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Offices_AspNetUsers_AssignorId",
                        column: x => x.AssignorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    EnteredDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EnteredById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReceivedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ReceivedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CallerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CallerRepresents = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CallerAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallerAddress_Street2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallerAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallerAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallerAddress_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallerPhoneNumber_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CallerPhoneNumber_Type = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    CallerSecondaryPhoneNumber_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CallerSecondaryPhoneNumber_Type = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    CallerTertiaryPhoneNumber_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CallerTertiaryPhoneNumber_Type = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    CallerEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ComplaintNature = table.Column<string>(type: "nvarchar(max)", maxLength: 15000, nullable: true),
                    ComplaintLocation = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ComplaintDirections = table.Column<string>(type: "nvarchar(2600)", maxLength: 2600, nullable: true),
                    ComplaintCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ComplaintCounty = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PrimaryConcernId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecondaryConcernId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceFacilityIdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceFacilityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceAddress_Street2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceAddress_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourcePhoneNumber_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SourcePhoneNumber_Type = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    SourceSecondaryPhoneNumber_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SourceSecondaryPhoneNumber_Type = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    SourceTertiaryPhoneNumber_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SourceTertiaryPhoneNumber_Type = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    SourceEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CurrentOfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentOwnerAssignedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CurrentOwnerAcceptedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReviewedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewComments = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    ComplaintClosed = table.Column<bool>(type: "bit", nullable: false),
                    ComplaintClosedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeleteComments = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Complaints_AspNetUsers_CurrentOwnerId",
                        column: x => x.CurrentOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Complaints_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Complaints_AspNetUsers_EnteredById",
                        column: x => x.EnteredById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Complaints_AspNetUsers_ReceivedById",
                        column: x => x.ReceivedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Complaints_AspNetUsers_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Complaints_Concerns_PrimaryConcernId",
                        column: x => x.PrimaryConcernId,
                        principalTable: "Concerns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Complaints_Concerns_SecondaryConcernId",
                        column: x => x.SecondaryConcernId,
                        principalTable: "Concerns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Complaints_Offices_CurrentOfficeId",
                        column: x => x.CurrentOfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplaintId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(245)", maxLength: 245, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UploadedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UploadedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsImage = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Attachments_AspNetUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Attachments_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplaintId = table.Column<int>(type: "int", nullable: false),
                    ActionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Investigator = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    EnteredDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EnteredById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ComplaintActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ComplaintActions_ActionTypes_ActionTypeId",
                        column: x => x.ActionTypeId,
                        principalTable: "ActionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ComplaintActions_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ComplaintActions_AspNetUsers_EnteredById",
                        column: x => x.EnteredById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ComplaintActions_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintTransitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplaintId = table.Column<int>(type: "int", nullable: false),
                    TransitionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommittedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CommittedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransferredToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransferredToOfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ComplaintTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ComplaintTransitions_AspNetUsers_CommittedByUserId",
                        column: x => x.CommittedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ComplaintTransitions_AspNetUsers_TransferredToUserId",
                        column: x => x.TransferredToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ComplaintTransitions_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ComplaintTransitions_Offices_TransferredToOfficeId",
                        column: x => x.TransferredToOfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX__AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleName_Index",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX__AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX__AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX__AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "Email_Index",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX__AspNetUsers_OfficeId",
                table: "AspNetUsers",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "UserName_Index",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX__Attachments_ComplaintId",
                table: "Attachments",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX__Attachments_UploadedById",
                table: "Attachments",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX__ComplaintActions_ActionTypeId",
                table: "ComplaintActions",
                column: "ActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX__ComplaintActions_ComplaintId",
                table: "ComplaintActions",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX__ComplaintActions_DeletedById",
                table: "ComplaintActions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX__ComplaintActions_EnteredById",
                table: "ComplaintActions",
                column: "EnteredById");

            migrationBuilder.CreateIndex(
                name: "IX__Complaints_CurrentOfficeId",
                table: "Complaints",
                column: "CurrentOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX__Complaints_CurrentOwnerId",
                table: "Complaints",
                column: "CurrentOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX__Complaints_DeletedById",
                table: "Complaints",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX__Complaints_EnteredById",
                table: "Complaints",
                column: "EnteredById");

            migrationBuilder.CreateIndex(
                name: "IX__Complaints_PrimaryConcernId",
                table: "Complaints",
                column: "PrimaryConcernId");

            migrationBuilder.CreateIndex(
                name: "IX__Complaints_ReceivedById",
                table: "Complaints",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX__Complaints_ReviewedById",
                table: "Complaints",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX__Complaints_SecondaryConcernId",
                table: "Complaints",
                column: "SecondaryConcernId");

            migrationBuilder.CreateIndex(
                name: "IX__ComplaintTransitions_CommittedByUserId",
                table: "ComplaintTransitions",
                column: "CommittedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX__ComplaintTransitions_ComplaintId",
                table: "ComplaintTransitions",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX__ComplaintTransitions_TransferredToOfficeId",
                table: "ComplaintTransitions",
                column: "TransferredToOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX__ComplaintTransitions_TransferredToUserId",
                table: "ComplaintTransitions",
                column: "TransferredToUserId");

            migrationBuilder.CreateIndex(
                name: "IX__Offices_AssignorId",
                table: "Offices",
                column: "AssignorId");

            migrationBuilder.AddForeignKey(
                name: "FK__AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__AspNetUsers_Offices_OfficeId",
                table: "AspNetUsers",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Offices_AspNetUsers_AssignorId",
                table: "Offices");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "ComplaintActions");

            migrationBuilder.DropTable(
                name: "ComplaintTransitions");

            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ActionTypes");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "Concerns");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Offices");
            
            migrationBuilder.RenameTable(name: "_archive_AspNetRoleClaims", newName: "AspNetRoleClaims");
            migrationBuilder.RenameTable(name: "_archive_AspNetRoles", newName: "AspNetRoles");
            migrationBuilder.RenameTable(name: "_archive_AspNetUserClaims", newName: "AspNetUserClaims");
            migrationBuilder.RenameTable(name: "_archive_AspNetUserLogins", newName: "AspNetUserLogins");
            migrationBuilder.RenameTable(name: "_archive_AspNetUserRoles", newName: "AspNetUserRoles");
            migrationBuilder.RenameTable(name: "_archive_AspNetUsers", newName: "AspNetUsers");
            migrationBuilder.RenameTable(name: "_archive_AspNetUserTokens", newName: "AspNetUserTokens");
            migrationBuilder.RenameTable(name: "_archive_Attachments", newName: "Attachments");
            migrationBuilder.RenameTable(name: "_archive_ComplaintActions", newName: "ComplaintActions");
            migrationBuilder.RenameTable(name: "_archive_Complaints", newName: "Complaints");
            migrationBuilder.RenameTable(name: "_archive_ComplaintTransitions", newName: "ComplaintTransitions");
            migrationBuilder.RenameTable(name: "_archive_EmailLogs", newName: "EmailLogs");
            migrationBuilder.RenameTable(name: "_archive_LookupActionTypes", newName: "LookupActionTypes");
            migrationBuilder.RenameTable(name: "_archive_LookupConcerns", newName: "LookupConcerns");
            migrationBuilder.RenameTable(name: "_archive_LookupCounties", newName: "LookupCounties");
            migrationBuilder.RenameTable(name: "_archive_LookupOffices", newName: "LookupOffices");
            migrationBuilder.RenameTable(name: "_archive_LookupStates", newName: "LookupStates");
        }
    }
}

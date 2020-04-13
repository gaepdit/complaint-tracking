using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ComplaintTracking.Migrations
{
    public partial class NewInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LookupActionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupActionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LookupConcerns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupConcerns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LookupCounties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupCounties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LookupStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PostalAbbreviation = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
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
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
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
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "ComplaintActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ComplaintId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEntered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EnteredById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Investigator = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintActions_LookupActionTypes_ActionTypeId",
                        column: x => x.ActionTypeId,
                        principalTable: "LookupActionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CallerCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CallerEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CallerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CallerPhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CallerPhoneType = table.Column<int>(type: "int", nullable: true),
                    CallerPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CallerRepresents = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CallerSecondaryPhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CallerSecondaryPhoneType = table.Column<int>(type: "int", nullable: true),
                    CallerStateId = table.Column<int>(type: "int", nullable: true),
                    CallerStreet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CallerStreet2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CallerTertiaryPhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CallerTertiaryPhoneType = table.Column<int>(type: "int", nullable: true),
                    ComplaintCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ComplaintClosed = table.Column<bool>(type: "bit", nullable: false),
                    ComplaintCountyId = table.Column<int>(type: "int", nullable: true),
                    ComplaintDirections = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ComplaintLocation = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ComplaintNature = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentAssignmentTransitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentOfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentProgram = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateComplaintClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCurrentOwnerAccepted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCurrentOwnerAssigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEntered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateReceived = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteComments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EnteredById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PrimaryConcernId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceivedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewComments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    SecondaryConcernId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SourceFacilityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceFacilityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourcePhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SourcePhoneType = table.Column<int>(type: "int", nullable: true),
                    SourcePostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SourceSecondaryPhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SourceSecondaryPhoneType = table.Column<int>(type: "int", nullable: true),
                    SourceStateId = table.Column<int>(type: "int", nullable: true),
                    SourceStreet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceStreet2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceTertiaryPhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SourceTertiaryPhoneType = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaints_LookupStates_CallerStateId",
                        column: x => x.CallerStateId,
                        principalTable: "LookupStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Complaints_LookupCounties_ComplaintCountyId",
                        column: x => x.ComplaintCountyId,
                        principalTable: "LookupCounties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Complaints_LookupConcerns_PrimaryConcernId",
                        column: x => x.PrimaryConcernId,
                        principalTable: "LookupConcerns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Complaints_LookupConcerns_SecondaryConcernId",
                        column: x => x.SecondaryConcernId,
                        principalTable: "LookupConcerns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Complaints_LookupStates_SourceStateId",
                        column: x => x.SourceStateId,
                        principalTable: "LookupStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintTransitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ComplaintId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateAccepted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTransferred = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransferredByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransferredFromOfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransferredFromUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransferredToOfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransferredToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransitionType = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintTransitions_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LookupOffices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MasterUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupOffices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_LookupOffices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "LookupOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OfficeId",
                table: "AspNetUsers",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintActions_ActionTypeId",
                table: "ComplaintActions",
                column: "ActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintActions_ComplaintId",
                table: "ComplaintActions",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintActions_DeletedById",
                table: "ComplaintActions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintActions_EnteredById",
                table: "ComplaintActions",
                column: "EnteredById");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CallerStateId",
                table: "Complaints",
                column: "CallerStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ComplaintCountyId",
                table: "Complaints",
                column: "ComplaintCountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CurrentOfficeId",
                table: "Complaints",
                column: "CurrentOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CurrentOwnerId",
                table: "Complaints",
                column: "CurrentOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_DeletedById",
                table: "Complaints",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_EnteredById",
                table: "Complaints",
                column: "EnteredById");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_PrimaryConcernId",
                table: "Complaints",
                column: "PrimaryConcernId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ReceivedById",
                table: "Complaints",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ReviewById",
                table: "Complaints",
                column: "ReviewById");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_SecondaryConcernId",
                table: "Complaints",
                column: "SecondaryConcernId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_SourceStateId",
                table: "Complaints",
                column: "SourceStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintTransitions_ComplaintId",
                table: "ComplaintTransitions",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintTransitions_TransferredByUserId",
                table: "ComplaintTransitions",
                column: "TransferredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintTransitions_TransferredFromOfficeId",
                table: "ComplaintTransitions",
                column: "TransferredFromOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintTransitions_TransferredFromUserId",
                table: "ComplaintTransitions",
                column: "TransferredFromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintTransitions_TransferredToOfficeId",
                table: "ComplaintTransitions",
                column: "TransferredToOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintTransitions_TransferredToUserId",
                table: "ComplaintTransitions",
                column: "TransferredToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupActionTypes_Name",
                table: "LookupActionTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LookupConcerns_Name",
                table: "LookupConcerns",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LookupCounties_Name",
                table: "LookupCounties",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LookupOffices_MasterUserId",
                table: "LookupOffices",
                column: "MasterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupOffices_Name",
                table: "LookupOffices",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LookupStates_Name",
                table: "LookupStates",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintActions_AspNetUsers_DeletedById",
                table: "ComplaintActions",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintActions_AspNetUsers_EnteredById",
                table: "ComplaintActions",
                column: "EnteredById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintActions_Complaints_ComplaintId",
                table: "ComplaintActions",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_CurrentOwnerId",
                table: "Complaints",
                column: "CurrentOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_DeletedById",
                table: "Complaints",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_EnteredById",
                table: "Complaints",
                column: "EnteredById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_ReceivedById",
                table: "Complaints",
                column: "ReceivedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_ReviewById",
                table: "Complaints",
                column: "ReviewById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_LookupOffices_CurrentOfficeId",
                table: "Complaints",
                column: "CurrentOfficeId",
                principalTable: "LookupOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintTransitions_AspNetUsers_TransferredByUserId",
                table: "ComplaintTransitions",
                column: "TransferredByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintTransitions_AspNetUsers_TransferredFromUserId",
                table: "ComplaintTransitions",
                column: "TransferredFromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintTransitions_AspNetUsers_TransferredToUserId",
                table: "ComplaintTransitions",
                column: "TransferredToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintTransitions_LookupOffices_TransferredFromOfficeId",
                table: "ComplaintTransitions",
                column: "TransferredFromOfficeId",
                principalTable: "LookupOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintTransitions_LookupOffices_TransferredToOfficeId",
                table: "ComplaintTransitions",
                column: "TransferredToOfficeId",
                principalTable: "LookupOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LookupOffices_AspNetUsers_MasterUserId",
                table: "LookupOffices",
                column: "MasterUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LookupOffices_AspNetUsers_MasterUserId",
                table: "LookupOffices");

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
                name: "ComplaintActions");

            migrationBuilder.DropTable(
                name: "ComplaintTransitions");

            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "LookupActionTypes");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "LookupStates");

            migrationBuilder.DropTable(
                name: "LookupCounties");

            migrationBuilder.DropTable(
                name: "LookupConcerns");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LookupOffices");
        }
    }
}

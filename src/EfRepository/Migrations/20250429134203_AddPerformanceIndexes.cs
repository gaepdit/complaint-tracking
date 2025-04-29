using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cts.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Complaints_CurrentOfficeId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_CurrentOwnerId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_ReceivedById",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_ComplaintActions_EnteredById",
                table: "ComplaintActions");

            migrationBuilder.CreateIndex(
                name: "missing_index_1131_1130",
                table: "Complaints",
                columns: new[] { "IsDeleted", "ComplaintCity" })
                .Annotation("SqlServer:Include", new[] { "ComplaintNature", "ComplaintLocation", "ComplaintDirections" });

            migrationBuilder.CreateIndex(
                name: "missing_index_1147_1146",
                table: "Complaints",
                columns: new[] { "ComplaintCounty", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "ComplaintNature", "ComplaintLocation", "ComplaintDirections" });

            migrationBuilder.CreateIndex(
                name: "missing_index_12_11",
                table: "Complaints",
                columns: new[] { "CurrentOwnerId", "CurrentOwnerAcceptedDate", "ComplaintClosed", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "missing_index_1208_1207",
                table: "Complaints",
                columns: new[] { "IsDeleted", "SourceContactName" });

            migrationBuilder.CreateIndex(
                name: "missing_index_1260_1259",
                table: "Complaints",
                columns: new[] { "ComplaintCounty", "IsDeleted", "SourceFacilityName" });

            migrationBuilder.CreateIndex(
                name: "missing_index_22_21",
                table: "Complaints",
                columns: new[] { "ComplaintCounty", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "ReceivedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_24_23",
                table: "Complaints",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "missing_index_348_347",
                table: "Complaints",
                columns: new[] { "CurrentOfficeId", "CurrentOwnerAcceptedDate", "ComplaintClosed", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "missing_index_486_485",
                table: "Complaints",
                column: "IsDeleted")
                .Annotation("SqlServer:Include", new[] { "PrimaryConcernId", "SecondaryConcernId" });

            migrationBuilder.CreateIndex(
                name: "missing_index_50_49",
                table: "Complaints",
                column: "IsDeleted")
                .Annotation("SqlServer:Include", new[] { "ReceivedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_55_54",
                table: "Complaints",
                columns: new[] { "IsDeleted", "SourceFacilityName" });

            migrationBuilder.CreateIndex(
                name: "missing_index_594_593",
                table: "Complaints",
                columns: new[] { "CurrentOfficeId", "IsDeleted", "CurrentOwnerId" })
                .Annotation("SqlServer:Include", new[] { "Status", "ReceivedDate", "ComplaintCounty", "SourceFacilityName" });

            migrationBuilder.CreateIndex(
                name: "missing_index_596_595",
                table: "Complaints",
                columns: new[] { "CurrentOfficeId", "CurrentOwnerId", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "ReceivedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_632_631",
                table: "Complaints",
                columns: new[] { "ReceivedById", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "missing_index_678_677",
                table: "Complaints",
                columns: new[] { "CurrentOfficeId", "ComplaintClosed", "IsDeleted", "CurrentOwnerId" })
                .Annotation("SqlServer:Include", new[] { "Status", "ReceivedDate", "ComplaintCounty", "SourceFacilityName" });

            migrationBuilder.CreateIndex(
                name: "missing_index_690_689",
                table: "Complaints",
                columns: new[] { "CurrentOfficeId", "ComplaintClosed", "IsDeleted", "CurrentOwnerId" })
                .Annotation("SqlServer:Include", new[] { "Status", "ReceivedDate", "ComplaintCounty", "SourceFacilityName", "ComplaintClosedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_696_695",
                table: "Complaints",
                columns: new[] { "CurrentOfficeId", "CurrentOwnerId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "missing_index_727_726",
                table: "Complaints",
                column: "IsDeleted")
                .Annotation("SqlServer:Include", new[] { "ReceivedDate", "PrimaryConcernId", "SecondaryConcernId" });

            migrationBuilder.CreateIndex(
                name: "missing_index_734_733",
                table: "Complaints",
                columns: new[] { "CurrentOfficeId", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "ReceivedDate", "PrimaryConcernId", "SecondaryConcernId" });

            migrationBuilder.CreateIndex(
                name: "missing_index_739_738",
                table: "Complaints",
                columns: new[] { "ComplaintCounty", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "missing_index_743_742",
                table: "Complaints",
                columns: new[] { "ComplaintCounty", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "PrimaryConcernId", "SecondaryConcernId" });

            migrationBuilder.CreateIndex(
                name: "missing_index_951_950",
                table: "Complaints",
                columns: new[] { "ComplaintCounty", "ComplaintClosed", "IsDeleted", "ComplaintClosedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_1190_1189",
                table: "ComplaintActions",
                columns: new[] { "EnteredById", "IsDeleted", "EnteredDate" })
                .Annotation("SqlServer:Include", new[] { "ComplaintId" });

            migrationBuilder.CreateIndex(
                name: "missing_index_1192_1191",
                table: "ComplaintActions",
                columns: new[] { "EnteredById", "IsDeleted", "EnteredDate" })
                .Annotation("SqlServer:Include", new[] { "ComplaintId", "ActionTypeId", "ActionDate", "Investigator", "Comments", "CreatedAt", "CreatedById", "UpdatedAt", "UpdatedById", "DeletedAt", "DeletedById" });

            migrationBuilder.CreateIndex(
                name: "missing_index_198_197",
                table: "AspNetUsers",
                column: "ObjectIdentifier");
            
            // The following three indexes were manually added to the migration. See notes in the AppDbContext file.
            
            migrationBuilder.CreateIndex(
                name: "missing_index_14_13",
                table: "Complaints",
                columns: new[] { "CurrentOwnerId", "ComplaintClosed", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "Status", "EnteredDate", "EnteredById", "ReceivedDate", "ReceivedById", "CallerName", "CallerRepresents", "CallerAddress_Street", "CallerAddress_Street2", "CallerAddress_City", "CallerAddress_State", "CallerAddress_PostalCode", "CallerPhoneNumber_Number", "CallerPhoneNumber_Type", "CallerSecondaryPhoneNumber_Number", "CallerSecondaryPhoneNumber_Type", "CallerTertiaryPhoneNumber_Number", "CallerTertiaryPhoneNumber_Type", "CallerEmail", "ComplaintNature", "ComplaintLocation", "ComplaintDirections", "ComplaintCity", "ComplaintCounty", "PrimaryConcernId", "SecondaryConcernId", "SourceFacilityIdNumber", "SourceFacilityName", "SourceContactName", "SourceAddress_Street", "SourceAddress_Street2", "SourceAddress_City", "SourceAddress_State", "SourceAddress_PostalCode", "SourcePhoneNumber_Number", "SourcePhoneNumber_Type", "SourceSecondaryPhoneNumber_Number", "SourceSecondaryPhoneNumber_Type", "SourceTertiaryPhoneNumber_Number", "SourceTertiaryPhoneNumber_Type", "SourceEmail", "CurrentOfficeId", "CurrentOwnerAssignedDate", "CurrentOwnerAcceptedDate", "ReviewedById", "ReviewComments", "ComplaintClosedDate", "DeleteComments", "CreatedAt", "CreatedById", "UpdatedAt", "UpdatedById", "DeletedAt", "DeletedById" });

            migrationBuilder.CreateIndex(
                name: "missing_index_745_744",
                table: "Complaints",
                columns: new[] { "ComplaintCounty", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "Status", "EnteredDate", "EnteredById", "ReceivedDate", "ReceivedById", "CallerName", "CallerRepresents", "CallerAddress_Street", "CallerAddress_Street2", "CallerAddress_City", "CallerAddress_State", "CallerAddress_PostalCode", "CallerPhoneNumber_Number", "CallerPhoneNumber_Type", "CallerSecondaryPhoneNumber_Number", "CallerSecondaryPhoneNumber_Type", "CallerTertiaryPhoneNumber_Number", "CallerTertiaryPhoneNumber_Type", "CallerEmail", "ComplaintNature", "ComplaintLocation", "ComplaintDirections", "ComplaintCity", "PrimaryConcernId", "SecondaryConcernId", "SourceFacilityIdNumber", "SourceFacilityName", "SourceContactName", "SourceAddress_Street", "SourceAddress_Street2", "SourceAddress_City", "SourceAddress_State", "SourceAddress_PostalCode", "SourcePhoneNumber_Number", "SourcePhoneNumber_Type", "SourceSecondaryPhoneNumber_Number", "SourceSecondaryPhoneNumber_Type", "SourceTertiaryPhoneNumber_Number", "SourceTertiaryPhoneNumber_Type", "SourceEmail", "CurrentOfficeId", "CurrentOwnerId", "CurrentOwnerAssignedDate", "CurrentOwnerAcceptedDate", "ReviewedById", "ReviewComments", "ComplaintClosed", "ComplaintClosedDate", "DeleteComments", "CreatedAt", "CreatedById", "UpdatedAt", "UpdatedById", "DeletedAt", "DeletedById" });

            migrationBuilder.CreateIndex(
                name: "missing_index_949_948",
                table: "Complaints",
                columns: new[] { "ComplaintCounty", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "SourceAddress_Street", "SourceAddress_Street2", "SourceAddress_City", "SourceAddress_State", "SourceAddress_PostalCode" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // The following three indexes were manually added to the migration.

            migrationBuilder.DropIndex(
                name: "missing_index_14_13",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_745_744",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_949_948",
                table: "Complaints");

            // The following indexes were created from the AppDbContext model builder.

            migrationBuilder.DropIndex(
                name: "missing_index_1131_1130",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_1147_1146",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_12_11",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_1208_1207",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_1260_1259",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_22_21",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_24_23",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_348_347",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_486_485",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_50_49",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_55_54",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_594_593",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_596_595",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_632_631",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_678_677",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_690_689",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_696_695",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_727_726",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_734_733",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_739_738",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_743_742",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_951_950",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "missing_index_1190_1189",
                table: "ComplaintActions");

            migrationBuilder.DropIndex(
                name: "missing_index_1192_1191",
                table: "ComplaintActions");

            migrationBuilder.DropIndex(
                name: "missing_index_198_197",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CurrentOfficeId",
                table: "Complaints",
                column: "CurrentOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CurrentOwnerId",
                table: "Complaints",
                column: "CurrentOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ReceivedById",
                table: "Complaints",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintActions_EnteredById",
                table: "ComplaintActions",
                column: "EnteredById");
        }
    }
}

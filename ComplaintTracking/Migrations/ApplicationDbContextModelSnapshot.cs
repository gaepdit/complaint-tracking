﻿// <auto-generated />
using System;
using ComplaintTracking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ComplaintTracking.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ComplaintTracking.Models.ActionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UpdatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("LookupActionTypes");
                });

            modelBuilder.Entity("ComplaintTracking.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid?>("OfficeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("OfficeId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ComplaintTracking.Models.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ComplaintId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("DeletedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FileExtension")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("FileName")
                        .HasMaxLength(245)
                        .HasColumnType("nvarchar(245)");

                    b.Property<bool>("IsImage")
                        .HasColumnType("bit");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("UploadedById")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ComplaintId");

                    b.HasIndex("DeletedById");

                    b.HasIndex("UploadedById");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("ComplaintTracking.Models.Complaint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CallerCity")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CallerEmail")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("CallerName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CallerPhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("CallerPhoneType")
                        .HasColumnType("int");

                    b.Property<string>("CallerPostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CallerRepresents")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("CallerSecondaryPhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("CallerSecondaryPhoneType")
                        .HasColumnType("int");

                    b.Property<int?>("CallerStateId")
                        .HasColumnType("int");

                    b.Property<string>("CallerStreet")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CallerStreet2")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CallerTertiaryPhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("CallerTertiaryPhoneType")
                        .HasColumnType("int");

                    b.Property<string>("ComplaintCity")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("ComplaintClosed")
                        .HasColumnType("bit");

                    b.Property<int?>("ComplaintCountyId")
                        .HasColumnType("int");

                    b.Property<string>("ComplaintDirections")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("ComplaintLocation")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("ComplaintNature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CurrentAssignmentTransitionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CurrentOfficeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CurrentOwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DateComplaintClosed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateCurrentOwnerAccepted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateCurrentOwnerAssigned")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateEntered")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateReceived")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeleteComments")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("DeletedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EnteredById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("PrimaryConcernId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReceivedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ReviewById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ReviewComments")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<Guid?>("SecondaryConcernId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SourceCity")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SourceContactName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SourceEmail")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("SourceFacilityId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SourceFacilityName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SourcePhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("SourcePhoneType")
                        .HasColumnType("int");

                    b.Property<string>("SourcePostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SourceSecondaryPhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("SourceSecondaryPhoneType")
                        .HasColumnType("int");

                    b.Property<int?>("SourceStateId")
                        .HasColumnType("int");

                    b.Property<string>("SourceStreet")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SourceStreet2")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SourceTertiaryPhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int?>("SourceTertiaryPhoneType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CallerStateId");

                    b.HasIndex("ComplaintCountyId");

                    b.HasIndex("CurrentOfficeId");

                    b.HasIndex("CurrentOwnerId");

                    b.HasIndex("DeletedById");

                    b.HasIndex("EnteredById");

                    b.HasIndex("PrimaryConcernId");

                    b.HasIndex("ReceivedById");

                    b.HasIndex("ReviewById");

                    b.HasIndex("SecondaryConcernId");

                    b.HasIndex("SourceStateId");

                    b.ToTable("Complaints");
                });

            modelBuilder.Entity("ComplaintTracking.Models.ComplaintAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ActionDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ActionTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ComplaintId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateEntered")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("DeletedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EnteredById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Investigator")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UpdatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ActionTypeId");

                    b.HasIndex("ComplaintId");

                    b.HasIndex("DeletedById");

                    b.HasIndex("EnteredById");

                    b.ToTable("ComplaintActions");
                });

            modelBuilder.Entity("ComplaintTracking.Models.ComplaintTransition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int>("ComplaintId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateAccepted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTransferred")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransferredByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("TransferredFromOfficeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TransferredFromUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("TransferredToOfficeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TransferredToUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TransitionType")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ComplaintId");

                    b.HasIndex("TransferredByUserId");

                    b.HasIndex("TransferredFromOfficeId");

                    b.HasIndex("TransferredFromUserId");

                    b.HasIndex("TransferredToOfficeId");

                    b.HasIndex("TransferredToUserId");

                    b.ToTable("ComplaintTransitions");
                });

            modelBuilder.Entity("ComplaintTracking.Models.Concern", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UpdatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("LookupConcerns");
                });

            modelBuilder.Entity("ComplaintTracking.Models.County", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("LookupCounties");
                });

            modelBuilder.Entity("ComplaintTracking.Models.EmailLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<string>("From")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HtmlBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("To")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EmailLogs");
                });

            modelBuilder.Entity("ComplaintTracking.Models.Office", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MasterUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UpdatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MasterUserId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("LookupOffices");
                });

            modelBuilder.Entity("ComplaintTracking.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("PostalAbbreviation")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("LookupStates");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ComplaintTracking.Models.ApplicationUser", b =>
                {
                    b.HasOne("ComplaintTracking.Models.Office", "Office")
                        .WithMany("Users")
                        .HasForeignKey("OfficeId");

                    b.Navigation("Office");
                });

            modelBuilder.Entity("ComplaintTracking.Models.Attachment", b =>
                {
                    b.HasOne("ComplaintTracking.Models.Complaint", "Complaint")
                        .WithMany("Attachments")
                        .HasForeignKey("ComplaintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "UploadedBy")
                        .WithMany()
                        .HasForeignKey("UploadedById");

                    b.Navigation("Complaint");

                    b.Navigation("DeletedBy");

                    b.Navigation("UploadedBy");
                });

            modelBuilder.Entity("ComplaintTracking.Models.Complaint", b =>
                {
                    b.HasOne("ComplaintTracking.Models.State", "CallerState")
                        .WithMany()
                        .HasForeignKey("CallerStateId");

                    b.HasOne("ComplaintTracking.Models.County", "ComplaintCounty")
                        .WithMany()
                        .HasForeignKey("ComplaintCountyId");

                    b.HasOne("ComplaintTracking.Models.Office", "CurrentOffice")
                        .WithMany()
                        .HasForeignKey("CurrentOfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "CurrentOwner")
                        .WithMany()
                        .HasForeignKey("CurrentOwnerId");

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "EnteredBy")
                        .WithMany()
                        .HasForeignKey("EnteredById");

                    b.HasOne("ComplaintTracking.Models.Concern", "PrimaryConcern")
                        .WithMany()
                        .HasForeignKey("PrimaryConcernId");

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "ReceivedBy")
                        .WithMany()
                        .HasForeignKey("ReceivedById");

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "ReviewBy")
                        .WithMany()
                        .HasForeignKey("ReviewById");

                    b.HasOne("ComplaintTracking.Models.Concern", "SecondaryConcern")
                        .WithMany()
                        .HasForeignKey("SecondaryConcernId");

                    b.HasOne("ComplaintTracking.Models.State", "SourceState")
                        .WithMany()
                        .HasForeignKey("SourceStateId");

                    b.Navigation("CallerState");

                    b.Navigation("ComplaintCounty");

                    b.Navigation("CurrentOffice");

                    b.Navigation("CurrentOwner");

                    b.Navigation("DeletedBy");

                    b.Navigation("EnteredBy");

                    b.Navigation("PrimaryConcern");

                    b.Navigation("ReceivedBy");

                    b.Navigation("ReviewBy");

                    b.Navigation("SecondaryConcern");

                    b.Navigation("SourceState");
                });

            modelBuilder.Entity("ComplaintTracking.Models.ComplaintAction", b =>
                {
                    b.HasOne("ComplaintTracking.Models.ActionType", "ActionType")
                        .WithMany()
                        .HasForeignKey("ActionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ComplaintTracking.Models.Complaint", "Complaint")
                        .WithMany("ComplaintActions")
                        .HasForeignKey("ComplaintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "EnteredBy")
                        .WithMany()
                        .HasForeignKey("EnteredById");

                    b.Navigation("ActionType");

                    b.Navigation("Complaint");

                    b.Navigation("DeletedBy");

                    b.Navigation("EnteredBy");
                });

            modelBuilder.Entity("ComplaintTracking.Models.ComplaintTransition", b =>
                {
                    b.HasOne("ComplaintTracking.Models.Complaint", "Complaint")
                        .WithMany("ComplaintTransitions")
                        .HasForeignKey("ComplaintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "TransferredByUser")
                        .WithMany()
                        .HasForeignKey("TransferredByUserId");

                    b.HasOne("ComplaintTracking.Models.Office", "TransferredFromOffice")
                        .WithMany()
                        .HasForeignKey("TransferredFromOfficeId");

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "TransferredFromUser")
                        .WithMany()
                        .HasForeignKey("TransferredFromUserId");

                    b.HasOne("ComplaintTracking.Models.Office", "TransferredToOffice")
                        .WithMany()
                        .HasForeignKey("TransferredToOfficeId");

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "TransferredToUser")
                        .WithMany()
                        .HasForeignKey("TransferredToUserId");

                    b.Navigation("Complaint");

                    b.Navigation("TransferredByUser");

                    b.Navigation("TransferredFromOffice");

                    b.Navigation("TransferredFromUser");

                    b.Navigation("TransferredToOffice");

                    b.Navigation("TransferredToUser");
                });

            modelBuilder.Entity("ComplaintTracking.Models.Office", b =>
                {
                    b.HasOne("ComplaintTracking.Models.ApplicationUser", "MasterUser")
                        .WithMany()
                        .HasForeignKey("MasterUserId");

                    b.Navigation("MasterUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ComplaintTracking.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ComplaintTracking.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ComplaintTracking.Models.ApplicationUser", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ComplaintTracking.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ComplaintTracking.Models.ApplicationUser", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("ComplaintTracking.Models.Complaint", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("ComplaintActions");

                    b.Navigation("ComplaintTransitions");
                });

            modelBuilder.Entity("ComplaintTracking.Models.Office", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}

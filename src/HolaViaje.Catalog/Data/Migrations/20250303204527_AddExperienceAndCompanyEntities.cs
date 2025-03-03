using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HolaViaje.Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExperienceAndCompanyEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LegalName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RegisteredIn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    VatId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BookInfo_Email = table.Column<string>(type: "text", nullable: true),
                    BookInfo_Phone = table.Column<string>(type: "text", nullable: true),
                    BookInfo_WhatsApp = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstantTicketDelivery = table.Column<bool>(type: "boolean", nullable: false),
                    MobileTicket = table.Column<bool>(type: "boolean", nullable: false),
                    WheelchairAccessible = table.Column<bool>(type: "boolean", nullable: false),
                    PetsFrendly = table.Column<bool>(type: "boolean", nullable: false),
                    MaxGuests = table.Column<int>(type: "integer", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    DaysToCancel = table.Column<int>(type: "integer", nullable: false),
                    CancellationPolicyType = table.Column<int>(type: "integer", nullable: false),
                    RefundPercentage = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duration_Days = table.Column<int>(type: "integer", nullable: true),
                    Duration_Hours = table.Column<int>(type: "integer", nullable: true),
                    Duration_Minutes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ManageAll = table.Column<bool>(type: "boolean", nullable: false),
                    ManageExperiences = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyManagers_Companies_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperiencePhotos",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperiencePhotos", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_ExperiencePhotos_Experiences_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Experiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExperienceId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    EndPoint_RecordId = table.Column<Guid>(type: "uuid", nullable: true),
                    EndPoint_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EndPoint_Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EndPoint_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EndPoint_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EndPoint_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EndPoint_ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EndPoint_Latitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EndPoint_Longitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EndPoint_Time = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EndPoint_Details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TicketRedemptionPoint_RecordId = table.Column<Guid>(type: "uuid", nullable: true),
                    TicketRedemptionPoint_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TicketRedemptionPoint_Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TicketRedemptionPoint_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TicketRedemptionPoint_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TicketRedemptionPoint_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TicketRedemptionPoint_ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TicketRedemptionPoint_Latitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TicketRedemptionPoint_Longitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TicketRedemptionPoint_Time = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TicketRedemptionPoint_Details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CancellationPolicyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CancellationPolicyDetails = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ImportantInformation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PetsPolicyDetails = table.Column<string>(type: "text", nullable: true),
                    WhatToExpect = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    LiveGuide = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Place_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Place_Latitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Place_Longitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienceTranslations_Experiences_ExperienceId",
                        column: x => x.ExperienceId,
                        principalTable: "Experiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceAdditionalInfos",
                columns: table => new
                {
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceAdditionalInfos", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_ExperienceAdditionalInfos_ExperienceTranslations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ExperienceTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceMeetingPoints",
                columns: table => new
                {
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Latitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Longitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Time = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceMeetingPoints", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_ExperienceMeetingPoints_ExperienceTranslations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ExperienceTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperiencePickupPoints",
                columns: table => new
                {
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Latitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Longitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Time = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperiencePickupPoints", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_ExperiencePickupPoints_ExperienceTranslations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ExperienceTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceServices",
                columns: table => new
                {
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Included = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceServices", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_ExperienceServices_ExperienceTranslations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ExperienceTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceStops",
                columns: table => new
                {
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    StopOrder = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AdmissionTicket = table.Column<int>(type: "integer", nullable: false),
                    Place_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Place_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Place_Latitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Place_Longitude = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Duration_Days = table.Column<int>(type: "integer", nullable: true),
                    Duration_Hours = table.Column<int>(type: "integer", nullable: true),
                    Duration_Minutes = table.Column<int>(type: "integer", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceStops", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_ExperienceStops_ExperienceTranslations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ExperienceTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyManagers_OwnerId",
                table: "CompanyManagers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceAdditionalInfos_OwnerId",
                table: "ExperienceAdditionalInfos",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceMeetingPoints_OwnerId",
                table: "ExperienceMeetingPoints",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperiencePhotos_OwnerId",
                table: "ExperiencePhotos",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperiencePickupPoints_OwnerId",
                table: "ExperiencePickupPoints",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceServices_OwnerId",
                table: "ExperienceServices",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceStops_OwnerId",
                table: "ExperienceStops",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceTranslations_ExperienceId_LanguageCode",
                table: "ExperienceTranslations",
                columns: new[] { "ExperienceId", "LanguageCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyManagers");

            migrationBuilder.DropTable(
                name: "ExperienceAdditionalInfos");

            migrationBuilder.DropTable(
                name: "ExperienceMeetingPoints");

            migrationBuilder.DropTable(
                name: "ExperiencePhotos");

            migrationBuilder.DropTable(
                name: "ExperiencePickupPoints");

            migrationBuilder.DropTable(
                name: "ExperienceServices");

            migrationBuilder.DropTable(
                name: "ExperienceStops");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "ExperienceTranslations");

            migrationBuilder.DropTable(
                name: "Experiences");
        }
    }
}

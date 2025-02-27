using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HolaViaje.Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExperienceAndTranslationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PageId = table.Column<Guid>(type: "uuid", nullable: false),
                    CancellationPolicyType = table.Column<int>(type: "integer", nullable: true),
                    DaysToCancel = table.Column<int>(type: "integer", nullable: true),
                    RefundPercentage = table.Column<int>(type: "integer", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    PickupAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    InstantTicketDelivery = table.Column<bool>(type: "boolean", nullable: false),
                    MobileTicket = table.Column<bool>(type: "boolean", nullable: false),
                    WheelchairAccessible = table.Column<bool>(type: "boolean", nullable: false),
                    MaxGuests = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    WhatsApp = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExperiencePhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperiencePhotos", x => x.Id);
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
                    Pickup_Address1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pickup_Address2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pickup_Place_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pickup_Place_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pickup_Place_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pickup_Place_Location_Latitude = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Pickup_Place_Location_Longitude = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Pickup_Details = table.Column<string>(type: "text", nullable: true),
                    CancellationPolicyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CancellationPolicyDetails = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ImportantInformation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    WhatToExpect = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    LiveGuide = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_Location_Latitude = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Place_Location_Longitude = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceAdditionalInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienceAdditionalInfos_ExperienceTranslations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ExperienceTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Included = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceServices", x => x.Id);
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Place_Location_Latitude = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Place_Location_Longitude = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienceStops_ExperienceTranslations_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ExperienceTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceAdditionalInfos_OwnerId",
                table: "ExperienceAdditionalInfos",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperiencePhotos_OwnerId",
                table: "ExperiencePhotos",
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
                name: "ExperienceAdditionalInfos");

            migrationBuilder.DropTable(
                name: "ExperiencePhotos");

            migrationBuilder.DropTable(
                name: "ExperienceServices");

            migrationBuilder.DropTable(
                name: "ExperienceStops");

            migrationBuilder.DropTable(
                name: "ExperienceTranslations");

            migrationBuilder.DropTable(
                name: "Experiences");
        }
    }
}

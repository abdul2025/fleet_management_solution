using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FleetManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistrationNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Manufacturer = table.Column<int>(type: "integer", nullable: false),
                    YearOfManufacture = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AircraftComponent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tag = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LifeLimitHours = table.Column<int>(type: "integer", nullable: true),
                    LifeLimitCycles = table.Column<int>(type: "integer", nullable: true),
                    InstalledOnAircraftId = table.Column<int>(type: "integer", nullable: false),
                    InstalledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AircraftComponent_Aircrafts_InstalledOnAircraftId",
                        column: x => x.InstalledOnAircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AircraftSpecification",
                columns: table => new
                {
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    BasedStation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SeatingCapacity = table.Column<int>(type: "integer", nullable: false),
                    MaxTakeoffWeight = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxLandingWeight = table.Column<decimal>(type: "numeric", nullable: false),
                    WeightUnit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftSpecification", x => x.AircraftId);
                    table.ForeignKey(
                        name: "FK_AircraftSpecification_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    ComponentId = table.Column<int>(type: "integer", nullable: true),
                    EventType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PerformedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ComponentsId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceEvent_AircraftComponent_ComponentsId",
                        column: x => x.ComponentsId,
                        principalTable: "AircraftComponent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceEvent_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AircraftComponent_InstalledOnAircraftId",
                table: "AircraftComponent",
                column: "InstalledOnAircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceEvent_AircraftId",
                table: "MaintenanceEvent",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceEvent_ComponentsId",
                table: "MaintenanceEvent",
                column: "ComponentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftSpecification");

            migrationBuilder.DropTable(
                name: "MaintenanceEvent");

            migrationBuilder.DropTable(
                name: "AircraftComponent");

            migrationBuilder.DropTable(
                name: "Aircrafts");
        }
    }
}

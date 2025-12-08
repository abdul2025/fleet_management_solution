using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addAircraftSpac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "aircrafts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "aircraft_specifications",
                columns: table => new
                {
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    based_station = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    seating_capacity = table.Column<int>(type: "integer", nullable: false),
                    max_takeoff_weight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    max_landing_weight = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraft_specifications", x => x.AircraftId);
                    table.ForeignKey(
                        name: "FK_aircraft_specifications_aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "aircrafts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aircraft_specifications");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "aircrafts");
        }
    }
}

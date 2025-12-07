using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FleetManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aircrafts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    registration_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    manufacturer = table.Column<int>(type: "integer", maxLength: 100, nullable: false),
                    year_of_manufacture = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircrafts", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_aircrafts_manufacturer",
                table: "aircrafts",
                column: "manufacturer");

            migrationBuilder.CreateIndex(
                name: "ix_aircrafts_model",
                table: "aircrafts",
                column: "model");

            migrationBuilder.CreateIndex(
                name: "ix_aircrafts_registration_number",
                table: "aircrafts",
                column: "registration_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_aircrafts_status",
                table: "aircrafts",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aircrafts");
        }
    }
}

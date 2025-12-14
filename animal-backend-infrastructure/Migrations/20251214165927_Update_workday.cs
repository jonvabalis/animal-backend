using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace animal_backend_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_workday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkHours",
                table: "WorkHours");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "WorkHours");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "WorkHours");

            migrationBuilder.AddColumn<int>(
                name: "Hour",
                table: "WorkHours",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Taken",
                table: "WorkHours",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkHours",
                table: "WorkHours",
                columns: new[] { "VeterinarianId", "Date", "Hour" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkHours",
                table: "WorkHours");

            migrationBuilder.DropColumn(
                name: "Hour",
                table: "WorkHours");

            migrationBuilder.DropColumn(
                name: "Taken",
                table: "WorkHours");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "WorkHours",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "WorkHours",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkHours",
                table: "WorkHours",
                columns: new[] { "VeterinarianId", "Date" });
        }
    }
}

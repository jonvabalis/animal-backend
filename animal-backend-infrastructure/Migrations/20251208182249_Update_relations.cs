using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace animal_backend_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkHours",
                table: "WorkHours");

            migrationBuilder.DropIndex(
                name: "IX_WorkHours_VeterinarianId",
                table: "WorkHours");

            migrationBuilder.DropColumn(
                name: "VeterinarianUuid",
                table: "WorkHours");

            migrationBuilder.DropColumn(
                name: "UserUuid",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "VeterinarianUuid",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "VeterinarianUuid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserUuid",
                table: "Animals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkHours",
                table: "WorkHours",
                columns: new[] { "VeterinarianId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkHours",
                table: "WorkHours");

            migrationBuilder.AddColumn<Guid>(
                name: "VeterinarianUuid",
                table: "WorkHours",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserUuid",
                table: "Visits",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VeterinarianUuid",
                table: "Visits",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VeterinarianUuid",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserUuid",
                table: "Animals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkHours",
                table: "WorkHours",
                columns: new[] { "VeterinarianUuid", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_VeterinarianId",
                table: "WorkHours",
                column: "VeterinarianId");
        }
    }
}

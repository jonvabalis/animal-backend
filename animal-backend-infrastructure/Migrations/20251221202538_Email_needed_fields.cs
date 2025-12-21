using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace animal_backend_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Email_needed_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReminderSent",
                table: "Visits",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderSent",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Users");
        }
    }
}

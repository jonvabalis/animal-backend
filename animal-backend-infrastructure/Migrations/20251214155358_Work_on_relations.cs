using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace animal_backend_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Work_on_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veterinarians_Users_Id",
                table: "Veterinarians");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Veterinarians_Users_Id",
                table: "Veterinarians",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

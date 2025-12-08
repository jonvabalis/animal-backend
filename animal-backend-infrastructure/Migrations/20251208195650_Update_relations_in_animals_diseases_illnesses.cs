using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace animal_backend_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_relations_in_animals_diseases_illnesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Veterinarians_VeterinarianId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "VeterinarianId",
                table: "Users",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "AnimalId",
                table: "Illnesses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DiseaseId",
                table: "Illnesses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Illnesses_AnimalId",
                table: "Illnesses",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Illnesses_DiseaseId",
                table: "Illnesses",
                column: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Illnesses_Animals_AnimalId",
                table: "Illnesses",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Illnesses_Diseases_DiseaseId",
                table: "Illnesses",
                column: "DiseaseId",
                principalTable: "Diseases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Veterinarians_VeterinarianId",
                table: "Users",
                column: "VeterinarianId",
                principalTable: "Veterinarians",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Illnesses_Animals_AnimalId",
                table: "Illnesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Illnesses_Diseases_DiseaseId",
                table: "Illnesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Veterinarians_VeterinarianId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Illnesses_AnimalId",
                table: "Illnesses");

            migrationBuilder.DropIndex(
                name: "IX_Illnesses_DiseaseId",
                table: "Illnesses");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Illnesses");

            migrationBuilder.DropColumn(
                name: "DiseaseId",
                table: "Illnesses");

            migrationBuilder.AlterColumn<Guid>(
                name: "VeterinarianId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Veterinarians_VeterinarianId",
                table: "Users",
                column: "VeterinarianId",
                principalTable: "Veterinarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

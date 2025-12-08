using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace animal_backend_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_vaccine_product_animal_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnimalId",
                table: "Vaccines",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AnimalId",
                table: "ProductsUsed",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductsUsed",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vaccines_AnimalId",
                table: "Vaccines",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsUsed_AnimalId",
                table: "ProductsUsed",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsUsed_ProductId",
                table: "ProductsUsed",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsUsed_Animals_AnimalId",
                table: "ProductsUsed",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsUsed_Products_ProductId",
                table: "ProductsUsed",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccines_Animals_AnimalId",
                table: "Vaccines",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsUsed_Animals_AnimalId",
                table: "ProductsUsed");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsUsed_Products_ProductId",
                table: "ProductsUsed");

            migrationBuilder.DropForeignKey(
                name: "FK_Vaccines_Animals_AnimalId",
                table: "Vaccines");

            migrationBuilder.DropIndex(
                name: "IX_Vaccines_AnimalId",
                table: "Vaccines");

            migrationBuilder.DropIndex(
                name: "IX_ProductsUsed_AnimalId",
                table: "ProductsUsed");

            migrationBuilder.DropIndex(
                name: "IX_ProductsUsed_ProductId",
                table: "ProductsUsed");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Vaccines");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "ProductsUsed");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductsUsed");
        }
    }
}

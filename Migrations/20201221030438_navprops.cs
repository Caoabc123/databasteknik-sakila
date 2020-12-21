using Microsoft.EntityFrameworkCore.Migrations;

namespace Sakila.Migrations
{
    public partial class navprops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rentals_InventoryId",
                table: "Rentals",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Inventory_InventoryId",
                table: "Rentals",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Inventory_InventoryId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_InventoryId",
                table: "Rentals");
        }
    }
}

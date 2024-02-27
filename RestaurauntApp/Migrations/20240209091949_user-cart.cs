using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurauntApp.Migrations
{
    /// <inheritdoc />
    public partial class usercart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CartItems_MenuItemId",
                table: "CartItems",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_MenuItems_MenuItemId",
                table: "CartItems",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_MenuItems_MenuItemId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_MenuItemId",
                table: "CartItems");
        }
    }
}

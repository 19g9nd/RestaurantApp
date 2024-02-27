using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurauntApp.Migrations
{
    /// <inheritdoc />
    public partial class addorderitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "OrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

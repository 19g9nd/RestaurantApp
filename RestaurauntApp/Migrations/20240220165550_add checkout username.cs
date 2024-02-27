using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurauntApp.Migrations
{
    /// <inheritdoc />
    public partial class addcheckoutusername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Checkouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Checkouts");
        }
    }
}

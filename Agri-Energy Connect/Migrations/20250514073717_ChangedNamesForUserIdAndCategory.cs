using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agri_Energy_Connect.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNamesForUserIdAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "farmerId",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Products",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "FarmerId",
                table: "Products",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "farmerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Products",
                newName: "FarmerId");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "Type");
        }
    }
}

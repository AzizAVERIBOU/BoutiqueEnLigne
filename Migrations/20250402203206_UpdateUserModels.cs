using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoutiqueEnLigne.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Addresses",
                newName: "Address1");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Banks");

            migrationBuilder.RenameColumn(
                name: "Address1",
                table: "Addresses",
                newName: "Street");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notebook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddressContactIdIsNotUniqueNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_ContactId",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ContactId",
                table: "Addresses",
                column: "ContactId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_ContactId",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ContactId",
                table: "Addresses",
                column: "ContactId",
                unique: true);
        }
    }
}

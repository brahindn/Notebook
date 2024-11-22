using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notebook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Contacts_PersonId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Addresses",
                newName: "ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses",
                newName: "IX_Addresses_ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Contacts_ContactId",
                table: "Addresses",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Contacts_ContactId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ContactId",
                table: "Addresses",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_ContactId",
                table: "Addresses",
                newName: "IX_Addresses_PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Contacts_PersonId",
                table: "Addresses",
                column: "PersonId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

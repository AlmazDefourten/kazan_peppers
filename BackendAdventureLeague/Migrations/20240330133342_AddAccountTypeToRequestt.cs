using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAdventureLeague.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountTypeToRequestt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Accounts_AccountId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Requests",
                newName: "AccountToId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_AccountId",
                table: "Requests",
                newName: "IX_Requests_AccountToId");

            migrationBuilder.AddColumn<long>(
                name: "AccountFromId",
                table: "Requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AccountFromId",
                table: "Requests",
                column: "AccountFromId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Accounts_AccountFromId",
                table: "Requests",
                column: "AccountFromId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Accounts_AccountToId",
                table: "Requests",
                column: "AccountToId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Accounts_AccountFromId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Accounts_AccountToId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_AccountFromId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "AccountFromId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "AccountToId",
                table: "Requests",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_AccountToId",
                table: "Requests",
                newName: "IX_Requests_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Accounts_AccountId",
                table: "Requests",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

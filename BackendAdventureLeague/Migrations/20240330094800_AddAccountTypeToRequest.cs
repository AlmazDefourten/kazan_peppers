using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAdventureLeague.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountTypeToRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "Requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AccountId",
                table: "Requests",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Accounts_AccountId",
                table: "Requests",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Accounts_AccountId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_AccountId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Requests");
        }
    }
}

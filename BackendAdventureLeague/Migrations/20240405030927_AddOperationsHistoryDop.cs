using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAdventureLeague.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationsHistoryDop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Operations",
                type: "text",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                table: "Operations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_RequestId",
                table: "Operations",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Requests_RequestId",
                table: "Operations",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Requests_RequestId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_RequestId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Operations");

            migrationBuilder.AlterColumn<long>(
                name: "Name",
                table: "Operations",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

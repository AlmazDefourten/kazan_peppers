using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAdventureLeague.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "Accounts");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAdventureLeague.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueToPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Phone",
                table: "AspNetUsers",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Phone",
                table: "AspNetUsers");
        }
    }
}

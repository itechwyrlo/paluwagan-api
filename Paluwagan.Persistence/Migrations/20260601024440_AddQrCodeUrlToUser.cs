using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paluwagan.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQrCodeUrlToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCodeUrl",
                table: "application_users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCodeUrl",
                table: "application_users");
        }
    }
}

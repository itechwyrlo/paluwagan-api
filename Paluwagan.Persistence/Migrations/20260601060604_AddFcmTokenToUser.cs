using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paluwagan.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFcmTokenToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FcmToken",
                table: "application_users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FcmToken",
                table: "application_users");
        }
    }
}

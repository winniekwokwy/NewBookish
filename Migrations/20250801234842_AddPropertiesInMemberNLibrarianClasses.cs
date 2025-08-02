using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewBookish.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesInMemberNLibrarianClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Members",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Librarians",
                newName: "LibrarianId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Members",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Members",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LibrarianId",
                table: "Librarians",
                newName: "Id");
        }
    }
}

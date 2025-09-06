using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewBookish.Migrations
{
    /// <inheritdoc />
    public partial class addLoanedBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Books_BookId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_BookId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Members");

            migrationBuilder.CreateTable(
                name: "BookMember",
                columns: table => new
                {
                    BorrowersMemberId = table.Column<int>(type: "integer", nullable: false),
                    LoanedBooksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookMember", x => new { x.BorrowersMemberId, x.LoanedBooksId });
                    table.ForeignKey(
                        name: "FK_BookMember_Books_LoanedBooksId",
                        column: x => x.LoanedBooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookMember_Members_BorrowersMemberId",
                        column: x => x.BorrowersMemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookMember_LoanedBooksId",
                table: "BookMember",
                column: "LoanedBooksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookMember");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Members",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_BookId",
                table: "Members",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Books_BookId",
                table: "Members",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Catcode",
                table: "transactions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_Catcode",
                table: "transactions",
                column: "Catcode");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions",
                column: "Catcode",
                principalTable: "categories",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "IX_transactions_Catcode",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Catcode",
                table: "transactions");
        }
    }
}

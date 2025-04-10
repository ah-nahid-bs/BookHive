using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHive.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalSoldToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalSold",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSold",
                table: "Books");
        }
    }
}

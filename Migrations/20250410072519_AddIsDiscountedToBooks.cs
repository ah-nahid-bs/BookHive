using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHive.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDiscountedToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDiscounted",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDiscounted",
                table: "Books");
        }
    }
}

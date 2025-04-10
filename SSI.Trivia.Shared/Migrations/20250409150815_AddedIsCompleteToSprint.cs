using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSI.Trivia.Shared.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsCompleteToSprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Games");
        }
    }
}

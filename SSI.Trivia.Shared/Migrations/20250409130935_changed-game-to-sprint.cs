using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSI.Trivia.Shared.Migrations
{
    /// <inheritdoc />
    public partial class changedgametosprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Games_GameId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Questions",
                newName: "SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_GameId",
                table: "Questions",
                newName: "IX_Questions_SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Games_SprintId",
                table: "Questions",
                column: "SprintId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Games_SprintId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "SprintId",
                table: "Questions",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_SprintId",
                table: "Questions",
                newName: "IX_Questions_GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Games_GameId",
                table: "Questions",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}

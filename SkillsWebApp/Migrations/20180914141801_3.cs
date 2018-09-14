using Microsoft.EntityFrameworkCore.Migrations;

namespace SkillsWebApp.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Player_RatingOfPlayer",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_RatingOfPlayer",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "RatingOfPlayer",
                table: "Rating");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Player",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_RatingId",
                table: "Player",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Rating_RatingId",
                table: "Player",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Rating_RatingId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_RatingId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "RatingOfPlayer",
                table: "Rating",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_RatingOfPlayer",
                table: "Rating",
                column: "RatingOfPlayer",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Player_RatingOfPlayer",
                table: "Rating",
                column: "RatingOfPlayer",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SkillsWebApp.Migrations
{
    public partial class uniquekey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PlayfabId",
                table: "Player",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_PlayfabId",
                table: "Player",
                column: "PlayfabId",
                unique: true,
                filter: "[PlayfabId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Player_PlayfabId",
                table: "Player");

            migrationBuilder.AlterColumn<string>(
                name: "PlayfabId",
                table: "Player",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SkillsWebApp.Migrations
{
    public partial class Playfab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayfabId",
                table: "Player",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayfabId",
                table: "Player");
        }
    }
}

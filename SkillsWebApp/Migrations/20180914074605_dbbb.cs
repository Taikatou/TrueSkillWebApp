using Microsoft.EntityFrameworkCore.Migrations;

namespace SkillsWebApp.Migrations
{
    public partial class dbbb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Rating");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Rating",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SkillsWebApp.Migrations
{
    public partial class dbbbd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Mean",
                table: "Rating",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StandardDeviation",
                table: "Rating",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mean",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "StandardDeviation",
                table: "Rating");
        }
    }
}

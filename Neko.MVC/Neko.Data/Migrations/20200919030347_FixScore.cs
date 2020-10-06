using Microsoft.EntityFrameworkCore.Migrations;

namespace Neko.Data.Migrations
{
    public partial class FixScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Socre",
                table: "tb_Solutions");

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "tb_Solutions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "tb_Questions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "tb_Solutions");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "tb_Questions");

            migrationBuilder.AddColumn<double>(
                name: "Socre",
                table: "tb_Solutions",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

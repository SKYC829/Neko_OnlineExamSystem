using Microsoft.EntityFrameworkCore.Migrations;

namespace Neko.Data.Migrations
{
    public partial class FixSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "tb_Solutions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "tb_Solutions");
        }
    }
}

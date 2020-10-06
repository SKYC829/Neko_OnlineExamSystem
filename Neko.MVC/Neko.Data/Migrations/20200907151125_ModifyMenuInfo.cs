using Microsoft.EntityFrameworkCore.Migrations;

namespace Neko.Data.Migrations
{
    public partial class ModifyMenuInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JumpUrl",
                table: "Tb_Menu");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Tb_Menu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Tb_Menu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "Tb_Menu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Tb_Menu");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Tb_Menu");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "Tb_Menu");

            migrationBuilder.AddColumn<string>(
                name: "JumpUrl",
                table: "Tb_Menu",
                type: "text",
                nullable: true);
        }
    }
}

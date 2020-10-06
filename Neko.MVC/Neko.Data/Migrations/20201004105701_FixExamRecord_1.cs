using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neko.Data.Migrations
{
    public partial class FixExamRecord_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BeginTime",
                table: "tb_ExamRecord",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LeftTime",
                table: "tb_ExamRecord",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginTime",
                table: "tb_ExamRecord");

            migrationBuilder.DropColumn(
                name: "LeftTime",
                table: "tb_ExamRecord");
        }
    }
}

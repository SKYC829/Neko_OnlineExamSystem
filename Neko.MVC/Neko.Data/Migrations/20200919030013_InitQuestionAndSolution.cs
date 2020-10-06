using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Neko.Data.Migrations
{
    public partial class InitQuestionAndSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    QuestionName = table.Column<string>(nullable: true),
                    QuestionType = table.Column<int>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_Questions_Tb_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Tb_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_Solutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Socre = table.Column<double>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_Solutions_Tb_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Tb_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_QuestionSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<int>(nullable: false),
                    SolutionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_QuestionSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_QuestionSolutions_tb_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "tb_Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_QuestionSolutions_tb_Solutions_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "tb_Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Questions_CreateUserId",
                table: "tb_Questions",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_QuestionSolutions_QuestionId",
                table: "tb_QuestionSolutions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_QuestionSolutions_SolutionId",
                table: "tb_QuestionSolutions",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Solutions_CreateUserId",
                table: "tb_Solutions",
                column: "CreateUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_QuestionSolutions");

            migrationBuilder.DropTable(
                name: "tb_Questions");

            migrationBuilder.DropTable(
                name: "tb_Solutions");
        }
    }
}

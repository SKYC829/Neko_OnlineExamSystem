using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Neko.Data.Migrations
{
    public partial class InitExamPaper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_ExamPapers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ExamName = table.Column<string>(nullable: true),
                    ExamMinute = table.Column<int>(nullable: false),
                    ExamScore = table.Column<double>(nullable: false),
                    QuestionNums = table.Column<int>(nullable: false),
                    ExamDateFrom = table.Column<DateTime>(nullable: false),
                    ExamDateTo = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    CreateUserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ExamPapers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_ExamPapers_Tb_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Tb_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_ExamQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ExamId = table.Column<int>(nullable: false),
                    RelationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ExamQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_ExamQuestions_tb_ExamPapers_ExamId",
                        column: x => x.ExamId,
                        principalTable: "tb_ExamPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_ExamQuestions_tb_Questions_RelationId",
                        column: x => x.RelationId,
                        principalTable: "tb_Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamPapers_CreateUserId",
                table: "tb_ExamPapers",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamQuestions_ExamId",
                table: "tb_ExamQuestions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamQuestions_RelationId",
                table: "tb_ExamQuestions",
                column: "RelationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_ExamQuestions");

            migrationBuilder.DropTable(
                name: "tb_ExamPapers");
        }
    }
}

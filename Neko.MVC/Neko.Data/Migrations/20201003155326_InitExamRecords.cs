using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Neko.Data.Migrations
{
    public partial class InitExamRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_ExamRecord",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ExamPaperId = table.Column<int>(nullable: false),
                    ExamScore = table.Column<double>(nullable: false),
                    IsPassed = table.Column<bool>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false,defaultValueSql:"current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ExamRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_ExamRecord_Tb_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Tb_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_ExamRecordQuestionDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RecordId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    QuestionScore = table.Column<double>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false,defaultValueSql:"current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ExamRecordQuestionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_ExamRecordQuestionDetail_Tb_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Tb_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_ExamRecordQuestionDetail_tb_ExamRecord_RecordId",
                        column: x => x.RecordId,
                        principalTable: "tb_ExamRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_ExamRecordSolutionDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DetailId = table.Column<int>(nullable: false),
                    SolutionId = table.Column<int>(nullable: false),
                    IsCorrect = table.Column<bool>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false,defaultValueSql:"current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ExamRecordSolutionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_ExamRecordSolutionDetail_Tb_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Tb_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_ExamRecordSolutionDetail_tb_ExamRecordQuestionDetail_Deta~",
                        column: x => x.DetailId,
                        principalTable: "tb_ExamRecordQuestionDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamRecord_CreateUserId",
                table: "tb_ExamRecord",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamRecordQuestionDetail_CreateUserId",
                table: "tb_ExamRecordQuestionDetail",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamRecordQuestionDetail_RecordId",
                table: "tb_ExamRecordQuestionDetail",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamRecordSolutionDetail_CreateUserId",
                table: "tb_ExamRecordSolutionDetail",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamRecordSolutionDetail_DetailId",
                table: "tb_ExamRecordSolutionDetail",
                column: "DetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_ExamRecordSolutionDetail");

            migrationBuilder.DropTable(
                name: "tb_ExamRecordQuestionDetail");

            migrationBuilder.DropTable(
                name: "tb_ExamRecord");
        }
    }
}

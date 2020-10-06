using Microsoft.EntityFrameworkCore.Migrations;

namespace Neko.Data.Migrations
{
    public partial class FixExamRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamRecordSolutionDetail_SolutionId",
                table: "tb_ExamRecordSolutionDetail",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamRecordQuestionDetail_QuestionId",
                table: "tb_ExamRecordQuestionDetail",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ExamRecord_ExamPaperId",
                table: "tb_ExamRecord",
                column: "ExamPaperId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_ExamRecord_tb_ExamPapers_ExamPaperId",
                table: "tb_ExamRecord",
                column: "ExamPaperId",
                principalTable: "tb_ExamPapers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_ExamRecordQuestionDetail_tb_Questions_QuestionId",
                table: "tb_ExamRecordQuestionDetail",
                column: "QuestionId",
                principalTable: "tb_Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_ExamRecordSolutionDetail_tb_Solutions_SolutionId",
                table: "tb_ExamRecordSolutionDetail",
                column: "SolutionId",
                principalTable: "tb_Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_ExamRecord_tb_ExamPapers_ExamPaperId",
                table: "tb_ExamRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_ExamRecordQuestionDetail_tb_Questions_QuestionId",
                table: "tb_ExamRecordQuestionDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_ExamRecordSolutionDetail_tb_Solutions_SolutionId",
                table: "tb_ExamRecordSolutionDetail");

            migrationBuilder.DropIndex(
                name: "IX_tb_ExamRecordSolutionDetail_SolutionId",
                table: "tb_ExamRecordSolutionDetail");

            migrationBuilder.DropIndex(
                name: "IX_tb_ExamRecordQuestionDetail_QuestionId",
                table: "tb_ExamRecordQuestionDetail");

            migrationBuilder.DropIndex(
                name: "IX_tb_ExamRecord_ExamPaperId",
                table: "tb_ExamRecord");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quasitekWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecordLogRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mode",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "Mode",
                table: "RecordLog",
                newName: "ModeId");

            migrationBuilder.RenameColumn(
                name: "WrongAnswer",
                table: "PaperType",
                newName: "WrongAnswers");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "PaperType",
                newName: "CorrectAnswers");

            migrationBuilder.AddColumn<long>(
                name: "ClassesId",
                table: "RecordLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RecordLog_ClassesId",
                table: "RecordLog",
                column: "ClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLog_CourseId",
                table: "RecordLog",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLog_ModeId",
                table: "RecordLog",
                column: "ModeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLog_StudentId",
                table: "RecordLog",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLog_Classes_ClassesId",
                table: "RecordLog",
                column: "ClassesId",
                principalTable: "Classes",
                principalColumn: "ClassesId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLog_Course_CourseId",
                table: "RecordLog",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLog_Mode_ModeId",
                table: "RecordLog",
                column: "ModeId",
                principalTable: "Mode",
                principalColumn: "ModeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLog_Student_StudentId",
                table: "RecordLog",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordLog_Classes_ClassesId",
                table: "RecordLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLog_Course_CourseId",
                table: "RecordLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLog_Mode_ModeId",
                table: "RecordLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLog_Student_StudentId",
                table: "RecordLog");

            migrationBuilder.DropIndex(
                name: "IX_RecordLog_ClassesId",
                table: "RecordLog");

            migrationBuilder.DropIndex(
                name: "IX_RecordLog_CourseId",
                table: "RecordLog");

            migrationBuilder.DropIndex(
                name: "IX_RecordLog_ModeId",
                table: "RecordLog");

            migrationBuilder.DropIndex(
                name: "IX_RecordLog_StudentId",
                table: "RecordLog");

            migrationBuilder.DropColumn(
                name: "ClassesId",
                table: "RecordLog");

            migrationBuilder.RenameColumn(
                name: "ModeId",
                table: "RecordLog",
                newName: "Mode");

            migrationBuilder.RenameColumn(
                name: "WrongAnswers",
                table: "PaperType",
                newName: "WrongAnswer");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswers",
                table: "PaperType",
                newName: "CorrectAnswer");

            migrationBuilder.AddColumn<string>(
                name: "Mode",
                table: "Course",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

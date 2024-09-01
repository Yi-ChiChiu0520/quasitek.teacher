using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quasitekWeb.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Record");

            migrationBuilder.DropColumn(
                name: "ClassesCode",
                table: "Classes");

            migrationBuilder.AlterColumn<long>(
                name: "TeacherId",
                table: "Teacher",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "StudentNumber",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Mode",
                columns: table => new
                {
                    ModeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mode", x => x.ModeId);
                });

            migrationBuilder.CreateTable(
                name: "PaperType",
                columns: table => new
                {
                    TypeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    CorrectNumber = table.Column<int>(type: "int", nullable: false),
                    WrongNumber = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WrongAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaperType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "RecordLog",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcedemicTypeId = table.Column<long>(type: "bigint", nullable: false),
                    AcedemicTypeScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TechTypeId = table.Column<long>(type: "bigint", nullable: false),
                    TechTypeScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    StudentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordLog", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_RecordLog_PaperType_AcedemicTypeId",
                        column: x => x.AcedemicTypeId,
                        principalTable: "PaperType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordLog_PaperType_TechTypeId",
                        column: x => x.TechTypeId,
                        principalTable: "PaperType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordLog_AcedemicTypeId",
                table: "RecordLog",
                column: "AcedemicTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLog_TechTypeId",
                table: "RecordLog",
                column: "TechTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mode");

            migrationBuilder.DropTable(
                name: "RecordLog");

            migrationBuilder.DropTable(
                name: "PaperType");

            migrationBuilder.DropColumn(
                name: "StudentNumber",
                table: "Student");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Teacher",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ClassesCode",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Record",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassesId = table.Column<long>(type: "bigint", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    Mode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionNum = table.Column<int>(type: "int", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestScore = table.Column<int>(type: "int", nullable: false),
                    TestTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Record", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_Record_Classes_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Classes",
                        principalColumn: "ClassesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Record_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Record_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Record_ClassesId",
                table: "Record",
                column: "ClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_CourseId",
                table: "Record",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_StudentId",
                table: "Record",
                column: "StudentId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quasitekWeb.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_Classes_ClassesId",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Student_StudentId1",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Classes_ClassesId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_ClassesId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Record_ClassesId",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "StudentCode",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "StudentId1",
                table: "Record",
                newName: "ClassesId1");

            migrationBuilder.RenameIndex(
                name: "IX_Record_StudentId1",
                table: "Record",
                newName: "IX_Record_ClassesId1");

            migrationBuilder.AlterColumn<long>(
                name: "StudentId",
                table: "Student",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ClassesId1",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ClassesId",
                table: "Classes",
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

            migrationBuilder.CreateIndex(
                name: "IX_Student_ClassesId1",
                table: "Student",
                column: "ClassesId1");

            migrationBuilder.CreateIndex(
                name: "IX_Record_StudentId",
                table: "Record",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Classes_ClassesId1",
                table: "Record",
                column: "ClassesId1",
                principalTable: "Classes",
                principalColumn: "ClassesId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Student_StudentId",
                table: "Record",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Classes_ClassesId1",
                table: "Student",
                column: "ClassesId1",
                principalTable: "Classes",
                principalColumn: "ClassesId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_Classes_ClassesId1",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Student_StudentId",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Classes_ClassesId1",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_ClassesId1",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Record_StudentId",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "ClassesId1",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ClassesCode",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "ClassesId1",
                table: "Record",
                newName: "StudentId1");

            migrationBuilder.RenameIndex(
                name: "IX_Record_ClassesId1",
                table: "Record",
                newName: "IX_Record_StudentId1");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Student",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "StudentCode",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "ClassesId",
                table: "Classes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ClassesId",
                table: "Student",
                column: "ClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_ClassesId",
                table: "Record",
                column: "ClassesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Classes_ClassesId",
                table: "Record",
                column: "ClassesId",
                principalTable: "Classes",
                principalColumn: "ClassesId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Student_StudentId1",
                table: "Record",
                column: "StudentId1",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Classes_ClassesId",
                table: "Student",
                column: "ClassesId",
                principalTable: "Classes",
                principalColumn: "ClassesId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

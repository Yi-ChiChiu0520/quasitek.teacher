using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quasitekWeb.Migrations
{
    /// <inheritdoc />
    public partial class fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_Classes_ClassesId1",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Classes_ClassesId1",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_ClassesId1",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Record_ClassesId1",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "ClassesId1",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ClassesId1",
                table: "Record");

            migrationBuilder.AlterColumn<long>(
                name: "TeacherId",
                table: "Classes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Student_Classes_ClassesId",
                table: "Student",
                column: "ClassesId",
                principalTable: "Classes",
                principalColumn: "ClassesId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_Classes_ClassesId",
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

            migrationBuilder.AddColumn<int>(
                name: "ClassesId1",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassesId1",
                table: "Record",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Classes",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "ClassesId",
                table: "Classes",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ClassesId1",
                table: "Student",
                column: "ClassesId1");

            migrationBuilder.CreateIndex(
                name: "IX_Record_ClassesId1",
                table: "Record",
                column: "ClassesId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Classes_ClassesId1",
                table: "Record",
                column: "ClassesId1",
                principalTable: "Classes",
                principalColumn: "ClassesId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Classes_ClassesId1",
                table: "Student",
                column: "ClassesId1",
                principalTable: "Classes",
                principalColumn: "ClassesId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

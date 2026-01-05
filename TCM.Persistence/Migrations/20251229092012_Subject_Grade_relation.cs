using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCM.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Subject_Grade_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeSubjects_Subject_SubjectId",
                table: "GradeSubjects");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "GradeSubjects");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "GradeSubjects");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subject",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SubjectId",
                table: "GradeSubjects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GradeId",
                table: "GradeSubjects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeSubjects_GradeId_SubjectId",
                table: "GradeSubjects",
                columns: new[] { "GradeId", "SubjectId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeSubjects_Grades_GradeId",
                table: "GradeSubjects",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeSubjects_Subject_SubjectId",
                table: "GradeSubjects",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeSubjects_Grades_GradeId",
                table: "GradeSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeSubjects_Subject_SubjectId",
                table: "GradeSubjects");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_GradeSubjects_GradeId_SubjectId",
                table: "GradeSubjects");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "GradeSubjects");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SubjectId",
                table: "GradeSubjects",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "GradeSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Subject",
                table: "GradeSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeSubjects_Subject_SubjectId",
                table: "GradeSubjects",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");
        }
    }
}

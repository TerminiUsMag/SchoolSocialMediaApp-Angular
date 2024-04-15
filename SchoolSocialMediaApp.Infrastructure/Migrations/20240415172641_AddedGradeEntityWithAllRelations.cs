using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedGradeEntityWithAllRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the grade."),
                    GradeValue = table.Column<int>(type: "int", nullable: false, comment: "The grade itself."),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time the grade was created."),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the subject"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the creator of the grade."),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the student the grade is assigned to.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Grades_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Grades_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                },
                comment: "Grade of a student");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CreatorId",
                table: "Grades",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SubjectId",
                table: "Grades",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grades");
        }
    }
}

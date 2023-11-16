using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class FixedSchoolClassesAndStudentsRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassesAndSubjects",
                columns: table => new
                {
                    SchoolClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "School Class id"),
                    SchoolSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "School Subject id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassesAndSubjects", x => new { x.SchoolClassId, x.SchoolSubjectId });
                    table.ForeignKey(
                        name: "FK_ClassesAndSubjects_Classes_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassesAndSubjects_Subjects_SchoolSubjectId",
                        column: x => x.SchoolSubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                },
                comment: "School Classes and their School Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_ClassesAndSubjects_SchoolSubjectId",
                table: "ClassesAndSubjects",
                column: "SchoolSubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassesAndSubjects");

            migrationBuilder.CreateTable(
                name: "SchoolClassSchoolSubject",
                columns: table => new
                {
                    SchoolClassesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClassSchoolSubject", x => new { x.SchoolClassesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_SchoolClassSchoolSubject_Classes_SchoolClassesId",
                        column: x => x.SchoolClassesId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolClassSchoolSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClassSchoolSubject_SubjectsId",
                table: "SchoolClassSchoolSubject",
                column: "SubjectsId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedPostsToClassesAndSubjectsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClassesAndSubjectsSchoolClassId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClassesAndSubjectsSchoolSubjectId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ClassesAndSubjectsSchoolClassId_ClassesAndSubjectsSchoolSubjectId",
                table: "Posts",
                columns: new[] { "ClassesAndSubjectsSchoolClassId", "ClassesAndSubjectsSchoolSubjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_ClassesAndSubjects_ClassesAndSubjectsSchoolClassId_ClassesAndSubjectsSchoolSubjectId",
                table: "Posts",
                columns: new[] { "ClassesAndSubjectsSchoolClassId", "ClassesAndSubjectsSchoolSubjectId" },
                principalTable: "ClassesAndSubjects",
                principalColumns: new[] { "SchoolClassId", "SchoolSubjectId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_ClassesAndSubjects_ClassesAndSubjectsSchoolClassId_ClassesAndSubjectsSchoolSubjectId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ClassesAndSubjectsSchoolClassId_ClassesAndSubjectsSchoolSubjectId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ClassesAndSubjectsSchoolClassId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ClassesAndSubjectsSchoolSubjectId",
                table: "Posts");
        }
    }
}

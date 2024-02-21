using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedOptionalRelationBetweenPostAndClassesAndSubjectsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolClassId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolSubjectId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SchoolClassId_SchoolSubjectId",
                table: "Posts",
                columns: new[] { "SchoolClassId", "SchoolSubjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_ClassesAndSubjects_SchoolClassId_SchoolSubjectId",
                table: "Posts",
                columns: new[] { "SchoolClassId", "SchoolSubjectId" },
                principalTable: "ClassesAndSubjects",
                principalColumns: new[] { "SchoolClassId", "SchoolSubjectId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_ClassesAndSubjects_SchoolClassId_SchoolSubjectId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SchoolClassId_SchoolSubjectId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SchoolClassId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SchoolSubjectId",
                table: "Posts");
        }
    }
}

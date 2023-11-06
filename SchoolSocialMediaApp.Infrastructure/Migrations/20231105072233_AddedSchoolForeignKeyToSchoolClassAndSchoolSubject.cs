using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedSchoolForeignKeyToSchoolClassAndSchoolSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "The school's id");

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "The school's id");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SchoolId",
                table: "Subjects",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Schools_SchoolId",
                table: "Classes",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Schools_SchoolId",
                table: "Subjects",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Schools_SchoolId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Schools_SchoolId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SchoolId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Classes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class FixedStudentsAndParentsRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Students_StudentId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Parents_StudentId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Parents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Parents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parents_StudentId",
                table: "Parents",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Students_StudentId",
                table: "Parents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}

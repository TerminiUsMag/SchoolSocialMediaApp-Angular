using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedStudentsParentsMappingTableForManyToManyRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Principals_PrincipalId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ParentId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Schools_PrincipalId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Students");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "Parents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "The id of the student that the parent is a parent of.");

            migrationBuilder.CreateTable(
                name: "StudentsParents",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the student."),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the parent.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsParents", x => new { x.StudentId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_StudentsParents_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentsParents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Parents to students mapping table");

            migrationBuilder.CreateIndex(
                name: "IX_Principals_SchoolId",
                table: "Principals",
                column: "SchoolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parents_StudentId",
                table: "Parents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsParents_ParentId",
                table: "StudentsParents",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Students_StudentId",
                table: "Parents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Principals_Schools_SchoolId",
                table: "Principals",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Students_StudentId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Principals_Schools_SchoolId",
                table: "Principals");

            migrationBuilder.DropTable(
                name: "StudentsParents");

            migrationBuilder.DropIndex(
                name: "IX_Principals_SchoolId",
                table: "Principals");

            migrationBuilder.DropIndex(
                name: "IX_Parents_StudentId",
                table: "Parents");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "The id of the parent of the student.");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "Parents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "The id of the student that the parent is a parent of.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ParentId",
                table: "Students",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_PrincipalId",
                table: "Schools",
                column: "PrincipalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Principals_PrincipalId",
                table: "Schools",
                column: "PrincipalId",
                principalTable: "Principals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }
    }
}

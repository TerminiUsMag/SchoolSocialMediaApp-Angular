using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedParentsAndStudentsAndAdministrators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the administrator."),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the user that is an administrator.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administrators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents an administrator of the application.");

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the parent."),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the user that is the parent."),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the student that the parent is a parent of."),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the school that the student is in.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parents_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "A parent that has a student in a school.");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the student."),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the user that is the student."),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the school the student is in."),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the parent of the student."),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                },
                comment: "A student that has a parent and is in a school.");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SchoolId",
                table: "Posts",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_UserId",
                table: "Administrators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_SchoolId",
                table: "Parents",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_UserId",
                table: "Parents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ParentId",
                table: "Students",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Schools_SchoolId",
                table: "Posts",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Schools_SchoolId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SchoolId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Posts");
        }
    }
}

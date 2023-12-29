using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class FixedUserSchoolRelationsParticipantsAndPrincipalRelationsAreSeparated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schools_SchoolId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "PrincipalId",
                table: "Schools",
                type: "uniqueidentifier",
                nullable: false,
                comment: "The id of the principal of the school.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "The id of the director of the school.");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                comment: "The id of the school the user is participant in.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "The id of the school the user is in.");

            migrationBuilder.AddColumn<Guid>(
                name: "PrincipledSchoolId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                comment: "The id of the school which the user is Principal of");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PrincipledSchoolId",
                table: "AspNetUsers",
                column: "PrincipledSchoolId",
                unique: true,
                filter: "[PrincipledSchoolId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schools_PrincipledSchoolId",
                table: "AspNetUsers",
                column: "PrincipledSchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schools_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schools_PrincipledSchoolId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schools_SchoolId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PrincipledSchoolId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrincipledSchoolId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "PrincipalId",
                table: "Schools",
                type: "uniqueidentifier",
                nullable: false,
                comment: "The id of the director of the school.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "The id of the principal of the school.");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                comment: "The id of the school the user is in.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "The id of the school the user is participant in.");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schools_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

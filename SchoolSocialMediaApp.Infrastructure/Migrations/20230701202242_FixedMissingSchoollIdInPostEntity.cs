using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class FixedMissingSchoollIdInPostEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Schools_SchoolId",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Schools",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                comment: "The description of the school.");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Schools",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "The image url of the school.");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Schools",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "The location of the school.");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "The id of the school the post is for.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "The image url of the user.");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Schools_SchoolId",
                table: "Posts",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Schools_SchoolId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "The id of the school the post is for.");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Schools_SchoolId",
                table: "Posts",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }
    }
}

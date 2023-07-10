using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedIsInvitedPropertyToApplicationUserAndRemovedAcceptedAndDeclinedPropertiesOfInvitationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedOn",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "DeclinedOn",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "IsDeclined",
                table: "Invitations");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Invitations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                comment: "Role is the role the invitation is for.",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Role is the role the invitation is for.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTeacher",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                comment: "Is teacher in a school",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsStudent",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                comment: "Is student in a school",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrincipal",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                comment: "Is principal of a school",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsParent",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                comment: "Is parent in a school",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Is admin of the app");

            migrationBuilder.AddColumn<bool>(
                name: "IsInvited",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Has a pending invitation for a school");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsInvited",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Invitations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Role is the role the invitation is for.",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldComment: "Role is the role the invitation is for.");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedOn",
                table: "Invitations",
                type: "datetime2",
                nullable: true,
                comment: "AcceptedOn is the date and time the invitation was accepted.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclinedOn",
                table: "Invitations",
                type: "datetime2",
                nullable: true,
                comment: "DeclinedOn is the date and time the invitation was declined.");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Invitations",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "IsAccepted is a boolean that determines if the invitation has been accepted.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeclined",
                table: "Invitations",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "IsDeclined is a boolean that determines if the invitation has been declined.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTeacher",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Is teacher in a school");

            migrationBuilder.AlterColumn<bool>(
                name: "IsStudent",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Is student in a school");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrincipal",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Is principal of a school");

            migrationBuilder.AlterColumn<bool>(
                name: "IsParent",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Is parent in a school");
        }
    }
}

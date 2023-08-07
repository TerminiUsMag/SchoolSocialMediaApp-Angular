using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("37453265-50a9-418b-1e11-08db7bfe5717"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("45c2c82d-a4ac-4da1-1e12-08db7bfe5717"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5181f14c-a6fc-464c-1e0f-08db7bfe5717"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5dbd2e13-b653-41b3-1e13-08db7bfe5717"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a40fc683-6f20-49f8-1e10-08db7bfe5717"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "ImageUrl", "IsAdmin", "IsInvited", "IsParent", "IsPrincipal", "IsStudent", "IsTeacher", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "ParentId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SchoolId", "SecurityStamp", "TeacherId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("37453265-50a9-418b-1e11-08db7bfe5717"), 0, "4cf98984-e485-438d-831b-687168281aac", new DateTime(2023, 8, 1, 16, 8, 7, 993, DateTimeKind.Local).AddTicks(22), "teacher@teachers.com", false, "Teacher", "/images/defaultProfile.png", false, false, false, false, false, false, "Teacher", true, null, "TEACHER@TEACHERS.COM", "TEACHER.TEACHER", null, "AQAAAAEAACcQAAAAEM24WYKmPOUA0utHBxc1PmRvmT9RySlJpXSFDFzkNuV75sJRDRKMApb4gEvj+qSqfA==", "2222222222", false, null, "BXEWJLAKEB6EO23MT2TY57ETV7XTQVZ6", null, false, "teacher.teacher" },
                    { new Guid("45c2c82d-a4ac-4da1-1e12-08db7bfe5717"), 0, "6a979441-e166-4d7c-bc4c-4b46e5cae55a", new DateTime(2023, 8, 1, 16, 8, 7, 994, DateTimeKind.Local).AddTicks(3254), "parent@parents.com", false, "Parent", "/images/defaultProfile.png", false, false, false, false, false, false, "Parent", true, null, "PARENT@PARENTS.COM", "PARENT.PARENT", null, "AQAAAAEAACcQAAAAEKjdCLR5p9fQ/DP+BbNxvKr8M6pBWfjnRnHf8uXxXEDqhUyedNX6D2GuwoPeNqdw5g==", "3333333333", false, null, "V3A2ZGPBDCFNLVW2EP5WEVPROE4265ZW", null, false, "parent.parent" },
                    { new Guid("5181f14c-a6fc-464c-1e0f-08db7bfe5717"), 0, "5c564bf9-a235-4ec4-8dc4-8bcf12ef4966", new DateTime(2023, 8, 1, 16, 8, 7, 990, DateTimeKind.Local).AddTicks(2203), "admin@admins.com", false, "Admin", "/images/defaultProfile.png", false, false, false, false, false, false, "Admin", true, null, "ADMIN@ADMINS.COM", "ADMIN.ADMIN", null, "AQAAAAEAACcQAAAAEIsL1v6/QL2rNMmR5zG1hms/H8TGNduUTRQomKn5wpXHsv9g6fzKaAywLywXYuTexw==", "0000000000", false, null, "BC7FH6NJQYGF3HPFVFUTZA3OOQPGHZHC", null, false, "admin.admin" },
                    { new Guid("5dbd2e13-b653-41b3-1e13-08db7bfe5717"), 0, "23370626-f28b-4650-8d07-30a2d6be75d3", new DateTime(2023, 8, 1, 16, 8, 7, 995, DateTimeKind.Local).AddTicks(6497), "student@students.com", false, "Student", "/images/defaultProfile.png", false, false, false, false, false, false, "Student", true, null, "STUDENT@STUDENTS.COM", "STUDENT.STUDENT", null, "AQAAAAEAACcQAAAAEH3YNItWCrE36aGh1u3OyaqbpUAK1dga9LEwyVM5RFD9cuXoiJo+nSa8eUFETE35lg==", "4444444444", false, null, "X64IPK5BPQMIAOA7VWMFQ3NRRAKFHGTD", null, false, "student.student" },
                    { new Guid("a40fc683-6f20-49f8-1e10-08db7bfe5717"), 0, "0f1bb1eb-c9b9-41cf-b37f-af1c03e4f280", new DateTime(2023, 8, 1, 16, 8, 7, 991, DateTimeKind.Local).AddTicks(6859), "principal@principals.com", false, "Principal", "/images/defaultProfile.png", false, false, false, false, false, false, "Principal", true, null, "PRINCIPAL@PRINCIPALS.COM", "PRINCIPAL.PRINCIPAL", null, "AQAAAAEAACcQAAAAEHTnpkr6gL/JNctl/jJR1Op1VQCHqUl+1tVqxzLd30urM9hGeDIRvpOGzY8Hq9bd9A==", "1111111111", false, null, "ZPXT765IMVDPWRJUCGNLC2RDDWZRP6VA", null, false, "principal.principal" }
                });
        }
    }
}

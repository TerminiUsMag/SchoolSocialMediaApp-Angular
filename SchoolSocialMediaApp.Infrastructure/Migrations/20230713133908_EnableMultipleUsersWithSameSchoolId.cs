using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class EnableMultipleUsersWithSameSchoolId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SchoolId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SchoolId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId",
                unique: true,
                filter: "[SchoolId] IS NOT NULL");
        }
    }
}

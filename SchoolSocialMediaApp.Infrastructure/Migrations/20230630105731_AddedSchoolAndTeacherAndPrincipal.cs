using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedSchoolAndTeacherAndPrincipal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_CreatorId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentsDislikes_Comment_CommentId",
                table: "CommentsDislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentsLikes_Comment_CommentId",
                table: "CommentsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_CreatorId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsDislikes_Post_PostId",
                table: "PostsDislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsLikes_Post_PostId",
                table: "PostsLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Post_CreatorId",
                table: "Posts",
                newName: "IX_Posts_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PostId",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_CreatorId",
                table: "Comments",
                newName: "IX_Comments_CreatorId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "The last name of the user.",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "The last name of the user.");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "The first name of the user.",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "The first name of the user.");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Principals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the director."),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the school the director is in charge of."),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the user that is a director.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Principals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Principals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "A director of a school.");

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the school."),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The name of the school."),
                    PrincipalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the director of the school.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_Principals_PrincipalId",
                        column: x => x.PrincipalId,
                        principalTable: "Principals",
                        principalColumn: "Id");
                },
                comment: "A school that has a director and students.");

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the teacher."),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the user that is a teacher."),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The id of the school the teacher is in."),
                    SubjectOfTeaching = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The subject the teacher teaches.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "A teacher of a school.");

            migrationBuilder.CreateIndex(
                name: "IX_Principals_UserId",
                table: "Principals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_PrincipalId",
                table: "Schools",
                column: "PrincipalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolId",
                table: "Teachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CreatorId",
                table: "Comments",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsDislikes_Comments_CommentId",
                table: "CommentsDislikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsLikes_Comments_CommentId",
                table: "CommentsLikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_CreatorId",
                table: "Posts",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsDislikes_Posts_PostId",
                table: "PostsDislikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostsLikes_Posts_PostId",
                table: "PostsLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CreatorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentsDislikes_Comments_CommentId",
                table: "CommentsDislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentsLikes_Comments_CommentId",
                table: "CommentsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_CreatorId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsDislikes_Posts_PostId",
                table: "PostsDislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsLikes_Posts_PostId",
                table: "PostsLikes");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "Principals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CreatorId",
                table: "Post",
                newName: "IX_Post_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "Comment",
                newName: "IX_Comment_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CreatorId",
                table: "Comment",
                newName: "IX_Comment_CreatorId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                comment: "The last name of the user.",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "The last name of the user.");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                comment: "The first name of the user.",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "The first name of the user.");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_CreatorId",
                table: "Comment",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsDislikes_Comment_CommentId",
                table: "CommentsDislikes",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsLikes_Comment_CommentId",
                table: "CommentsLikes",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_CreatorId",
                table: "Post",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsDislikes_Post_PostId",
                table: "PostsDislikes",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostsLikes_Post_PostId",
                table: "PostsLikes",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }
    }
}

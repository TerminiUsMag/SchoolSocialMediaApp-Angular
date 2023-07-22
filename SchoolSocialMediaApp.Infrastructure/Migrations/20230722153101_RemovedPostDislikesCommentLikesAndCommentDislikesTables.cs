using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class RemovedPostDislikesCommentLikesAndCommentDislikesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentsDislikes");

            migrationBuilder.DropTable(
                name: "CommentsLikes");

            migrationBuilder.DropTable(
                name: "PostsDislikes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentsDislikes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the user who disliked the comment."),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the comment which is disliked.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsDislikes", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentsDislikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentsDislikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Comments which are disliked by users");

            migrationBuilder.CreateTable(
                name: "CommentsLikes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the user who liked the comment."),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the comment which is liked.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsLikes", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentsLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentsLikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Comments liked by user");

            migrationBuilder.CreateTable(
                name: "PostsDislikes",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the post which is disliked."),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the user who disliked the post.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsDislikes", x => new { x.PostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PostsDislikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostsDislikes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                },
                comment: "Users who have disliked a post");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsDislikes_CommentId",
                table: "CommentsDislikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsLikes_CommentId",
                table: "CommentsLikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostsDislikes_UserId",
                table: "PostsDislikes",
                column: "UserId");
        }
    }
}

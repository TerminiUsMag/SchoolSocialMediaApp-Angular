using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class RemovedPostsCommentsUnnecessaryMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostsComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostsComments",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the post which is commented on."),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The unique identifier for the comment.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsComments", x => new { x.PostId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_PostsComments_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostsComments_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Comments on a post");

            migrationBuilder.CreateIndex(
                name: "IX_PostsComments_CommentId",
                table: "PostsComments",
                column: "CommentId");
        }
    }
}

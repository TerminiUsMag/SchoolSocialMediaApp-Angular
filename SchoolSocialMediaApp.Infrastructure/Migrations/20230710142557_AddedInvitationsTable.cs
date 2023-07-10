using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSocialMediaApp.Infrastructure.Migrations
{
    public partial class AddedInvitationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id is the primary key of the invitation table."),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "SenderId is the foreign key of the sender of the invitation."),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ReceiverId is the foreign key of the receiver of the invitation."),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "SchoolId is the foreign key of the school the invitation is for."),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Role is the role the invitation is for."),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false, comment: "IsAccepted is a boolean that determines if the invitation has been accepted."),
                    IsPending = table.Column<bool>(type: "bit", nullable: false, comment: "IsPending is a boolean that determines if the invitation is pending."),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false, comment: "IsDeclined is a boolean that determines if the invitation has been declined."),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "CreatedOn is the date and time the invitation was created."),
                    AcceptedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "AcceptedOn is the date and time the invitation was accepted."),
                    DeclinedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "DeclinedOn is the date and time the invitation was declined.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invitations_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                },
                comment: "Invitation table holds all the invitations sent to users to join a school and a role in that school. It holds the sender, receiver, school, role, and status of the invitation.");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ReceiverId",
                table: "Invitations",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_SchoolId",
                table: "Invitations",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_SenderId",
                table: "Invitations",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");
        }
    }
}

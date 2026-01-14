using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_AspNetUsers_ReceiverId",
                table: "ChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_AspNetUsers_SenderId",
                table: "ChatMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessage_ReceiverId",
                table: "ChatMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "ChatMessage");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Match",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "ChatMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ChatMessage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChatMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastMessageAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversation_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Match_ApplicationUserId",
                table: "Match",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ConversationId",
                table: "ChatMessage",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_User1Id_User2Id",
                table: "Conversation",
                columns: new[] { "User1Id", "User2Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_User2Id",
                table: "Conversation",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_Conversation_ConversationId",
                table: "ChatMessage",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_AspNetUsers_ApplicationUserId",
                table: "Match",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_Conversation_ConversationId",
                table: "ChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_AspNetUsers_ApplicationUserId",
                table: "Match");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Match_ApplicationUserId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessage_ConversationId",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatMessage");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "ChatMessage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "ChatMessage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ReceiverId",
                table: "ChatMessage",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessage",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_AspNetUsers_ReceiverId",
                table: "ChatMessage",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_AspNetUsers_SenderId",
                table: "ChatMessage",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

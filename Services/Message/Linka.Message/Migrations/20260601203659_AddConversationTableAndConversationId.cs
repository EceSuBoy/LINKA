using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Linka.Message.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationTableAndConversationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "UserMessages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    ConversationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstUserId = table.Column<string>(type: "text", nullable: false),
                    SecondUserId = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    SourceCommentId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.ConversationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_ConversationId",
                table: "UserMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_FirstUserId_SecondUserId_ProductId",
                table: "Conversations",
                columns: new[] { "FirstUserId", "SecondUserId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_Conversations_ConversationId",
                table: "UserMessages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_Conversations_ConversationId",
                table: "UserMessages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_UserMessages_ConversationId",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "UserMessages");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Infrastructure.AppContext.Migrations
{
    public partial class AddTitleAndPublicToChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeletedForAll",
                table: "Messages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeletedForSender",
                table: "Messages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Chats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Chats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedForAll",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DeletedForSender",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Chats");
        }
    }
}

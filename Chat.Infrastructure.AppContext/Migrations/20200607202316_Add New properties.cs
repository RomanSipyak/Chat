using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Infrastructure.AppContext.Migrations
{
    public partial class AddNewproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DefaultChat",
                table: "Chats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSend",
                table: "ChatMessages",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                column: "ConcurrencyStamp",
                value: "3c2696b2-de23-45de-a1ca-8ddf0049bce5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultChat",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "DateSend",
                table: "ChatMessages");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                column: "ConcurrencyStamp",
                value: "fedc1c84-51cf-4579-9933-5ead0e6f8d9d");
        }
    }
}

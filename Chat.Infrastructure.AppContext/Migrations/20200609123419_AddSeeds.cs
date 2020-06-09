using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Infrastructure.AppContext.Migrations
{
    public partial class AddSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                column: "ConcurrencyStamp",
                value: "553d49f4-2061-41b1-abeb-4420699d7696");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "360829bd-6e48-412c-8d27-8c60bea7cca6", "user1@gmail.fake", false, false, null, "USER1@GMAIL.FAKE", "USER1", "AQAAAAEAACcQAAAAEBUqt+LdVK2AjnKZJadd43pqLSM41OHxC55iVntVgCNkftdJm4bL41xeaJ9Vghck6Q==", null, false, "", false, "User1" },
                    { "2", 0, "7b6d9120-da9f-4d59-9785-4c3fad5906e5", "user2@gmail.fake", false, false, null, "USER2@GMAIL.FAKE", "USER2", "AQAAAAEAACcQAAAAELR+Uv3leJorsbmw+JYsvF6z4DaX+/r8cFlxgldkvVR8dpsb1RWb05f/ks4cKUpxdQ==", null, false, "", false, "User2" },
                    { "3", 0, "258286ee-a951-4e17-8b56-6ee38f6c50de", "user3@gmail.fake", false, false, null, "USER3@GMAIL.FAKE", "USER3", "AQAAAAEAACcQAAAAEMCeldQQQn+clWh4fAEzSEgH7Bg9lAOxes5EQ7QEp7/XOMh4+2YfpRMUUGAf02qIRQ==", null, false, "", false, "User3" },
                    { "4", 0, "ba91eb43-0099-47fd-87aa-f8a41eb85a1c", "user4@gmail.fake", false, false, null, "USER4@GMAIL.FAKE", "USER4", "AQAAAAEAACcQAAAAEAEFbkTH5NqyTCHRYYZ7MHJIT44CqL6f9q6kWBL+WVxa2Ild6UwQlBSw3jnjUC+vUw==", null, false, "", false, "User4" }
                });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "Id", "DefaultChat", "Public", "Title" },
                values: new object[,]
                {
                    { 1, false, true, "Chat1" },
                    { 2, true, false, "Chat2" },
                    { 3, false, true, "Chat3" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "1", "2c087181-7837-4f6e-b233-7d0bb9a517fa" },
                    { "2", "2c087181-7837-4f6e-b233-7d0bb9a517fa" },
                    { "3", "2c087181-7837-4f6e-b233-7d0bb9a517fa" },
                    { "4", "2c087181-7837-4f6e-b233-7d0bb9a517fa" }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "DeletedForAll", "DeletedForSender", "MessageData", "ParrentMessageId", "SenderId" },
                values: new object[,]
                {
                    { 1, false, false, "For chat1 user1", null, "1" },
                    { 4, false, false, "For chat2 user1", null, "1" },
                    { 2, false, false, "For chat1 user2", null, "2" },
                    { 5, false, false, "For chat2 user2", null, "2" }
                });

            migrationBuilder.InsertData(
                table: "UserChats",
                columns: new[] { "Id", "ChatId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "1" },
                    { 4, 2, "1" },
                    { 2, 1, "2" },
                    { 5, 2, "2" },
                    { 3, 1, "3" }
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "ChatId", "DateSend", "MessageId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2020, 6, 9, 12, 34, 14, 304, DateTimeKind.Utc).AddTicks(6547), 1 },
                    { 4, 2, new DateTime(2020, 6, 9, 12, 44, 14, 305, DateTimeKind.Utc).AddTicks(14), 4 },
                    { 2, 1, new DateTime(2020, 6, 10, 12, 34, 14, 304, DateTimeKind.Utc).AddTicks(9539), 2 },
                    { 5, 2, new DateTime(2020, 6, 9, 12, 46, 14, 305, DateTimeKind.Utc).AddTicks(23), 5 }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "DeletedForAll", "DeletedForSender", "MessageData", "ParrentMessageId", "SenderId" },
                values: new object[,]
                {
                    { 6, false, false, "For chat2 user1", 4, "2" },
                    { 3, false, true, "For chat1 user3", 2, "3" }
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "ChatId", "DateSend", "MessageId" },
                values: new object[] { 6, 2, new DateTime(2020, 6, 9, 12, 49, 14, 305, DateTimeKind.Utc).AddTicks(30), 6 });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "ChatId", "DateSend", "MessageId" },
                values: new object[] { 3, 1, new DateTime(2020, 6, 9, 12, 44, 14, 304, DateTimeKind.Utc).AddTicks(9946), 3 });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "DeletedForAll", "DeletedForSender", "MessageData", "ParrentMessageId", "SenderId" },
                values: new object[] { 7, false, false, "For chat2 user1", 6, "1" });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "ChatId", "DateSend", "MessageId" },
                values: new object[] { 7, 2, new DateTime(2020, 6, 9, 12, 54, 14, 305, DateTimeKind.Utc).AddTicks(36), 7 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "1", "2c087181-7837-4f6e-b233-7d0bb9a517fa" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "2", "2c087181-7837-4f6e-b233-7d0bb9a517fa" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "3", "2c087181-7837-4f6e-b233-7d0bb9a517fa" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "4", "2c087181-7837-4f6e-b233-7d0bb9a517fa" });

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserChats",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserChats",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserChats",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserChats",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UserChats",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                column: "ConcurrencyStamp",
                value: "3c2696b2-de23-45de-a1ca-8ddf0049bce5");
        }
    }
}

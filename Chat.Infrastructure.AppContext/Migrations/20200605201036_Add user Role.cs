using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Infrastructure.AppContext.Migrations
{
    public partial class AdduserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2c087181-7837-4f6e-b233-7d0bb9a517fa", "fedc1c84-51cf-4579-9933-5ead0e6f8d9d", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c087181-7837-4f6e-b233-7d0bb9a517fa");
        }
    }
}

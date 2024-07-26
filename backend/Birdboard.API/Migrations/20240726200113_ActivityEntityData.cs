using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birdboard.API.Migrations
{
    public partial class ActivityEntityData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "468463e4-2cec-4d04-a6c0-f8454c8d206f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc4571da-83ff-49a3-82aa-904478053559");

            migrationBuilder.AddColumn<string>(
                name: "EntityData",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "41d6e83e-0b38-41d1-9cf0-a02f2eba6eb5", "009c1f50-0591-4a74-b6ef-4fcd3e040d8c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "90603965-abf6-41c3-91cd-26208a34ba1f", "087c540e-5426-46c8-8a60-99e3b5cc25ea", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41d6e83e-0b38-41d1-9cf0-a02f2eba6eb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90603965-abf6-41c3-91cd-26208a34ba1f");

            migrationBuilder.DropColumn(
                name: "EntityData",
                table: "Activities");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "468463e4-2cec-4d04-a6c0-f8454c8d206f", "eb897712-a09f-4d3c-a56e-f90c4e3f67c8", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cc4571da-83ff-49a3-82aa-904478053559", "10668ba4-2efd-4c61-9dd8-d10a13a9af48", "Admin", "ADMIN" });
        }
    }
}

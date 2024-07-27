using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birdboard.API.Migrations
{
    public partial class ActivityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0f34308-366b-4d58-9c1c-6b860766c4df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6d9e944-246a-42ed-ab9f-1f7e8ab0b6a7");

            migrationBuilder.AddColumn<string>(
                name: "Changes",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "64be66f7-41e4-4785-8884-b4e4ad6d930e", "18703dbf-9854-4ed9-a891-2a5768e33291", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c769f20d-c10c-4bef-bdb2-7a24329e2091", "18b6bd55-6f93-4eb3-a0f9-5f6d65f31b80", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64be66f7-41e4-4785-8884-b4e4ad6d930e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c769f20d-c10c-4bef-bdb2-7a24329e2091");

            migrationBuilder.DropColumn(
                name: "Changes",
                table: "Activities");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a0f34308-366b-4d58-9c1c-6b860766c4df", "65ed3d2e-ac49-41f7-9d6a-0ed27fe99c46", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f6d9e944-246a-42ed-ab9f-1f7e8ab0b6a7", "07837045-bea7-4f36-9d4c-c79237156283", "Admin", "ADMIN" });
        }
    }
}

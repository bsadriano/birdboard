using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birdboard.API.Migrations
{
    public partial class AddActivityProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41d6e83e-0b38-41d1-9cf0-a02f2eba6eb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90603965-abf6-41c3-91cd-26208a34ba1f");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a0f34308-366b-4d58-9c1c-6b860766c4df", "65ed3d2e-ac49-41f7-9d6a-0ed27fe99c46", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f6d9e944-246a-42ed-ab9f-1f7e8ab0b6a7", "07837045-bea7-4f36-9d4c-c79237156283", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ProjectId",
                table: "Activities",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ProjectId",
                table: "Activities");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0f34308-366b-4d58-9c1c-6b860766c4df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6d9e944-246a-42ed-ab9f-1f7e8ab0b6a7");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Activities");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "41d6e83e-0b38-41d1-9cf0-a02f2eba6eb5", "009c1f50-0591-4a74-b6ef-4fcd3e040d8c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "90603965-abf6-41c3-91cd-26208a34ba1f", "087c540e-5426-46c8-8a60-99e3b5cc25ea", "User", "USER" });
        }
    }
}

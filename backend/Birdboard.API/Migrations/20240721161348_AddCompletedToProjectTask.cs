using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birdboard.API.Migrations
{
    public partial class AddCompletedToProjectTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3f26c70-14c8-411f-a479-62ad88bed57c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f81de6d9-1c05-4da0-8a60-91d9baf96ba8");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "ProjectTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6812b448-818f-44c2-8403-496c63e1ccf2", "83f46b94-9da1-4256-8602-864564634149", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7aff409c-f415-4df3-adc3-2259a0ddd191", "98b4ddf7-3810-474d-977f-77f2e1532ab8", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6812b448-818f-44c2-8403-496c63e1ccf2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7aff409c-f415-4df3-adc3-2259a0ddd191");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "ProjectTasks");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d3f26c70-14c8-411f-a479-62ad88bed57c", "d806bf95-ffc8-4d56-bfae-0b2bda8fdca1", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f81de6d9-1c05-4da0-8a60-91d9baf96ba8", "71924fcb-ff34-4708-aa20-5753366218a5", "User", "USER" });
        }
    }
}

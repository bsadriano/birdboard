using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birdboard.API.Migrations
{
    public partial class ReplaceActivityProjectWithSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: "9c5f340e-7097-469a-b006-04190cd25131");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e98a6e1f-a109-46b6-8afa-4d35a410339a");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Activities",
                newName: "SubjectId");

            migrationBuilder.AddColumn<string>(
                name: "SubjectType",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "468463e4-2cec-4d04-a6c0-f8454c8d206f", "eb897712-a09f-4d3c-a56e-f90c4e3f67c8", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cc4571da-83ff-49a3-82aa-904478053559", "10668ba4-2efd-4c61-9dd8-d10a13a9af48", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "468463e4-2cec-4d04-a6c0-f8454c8d206f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc4571da-83ff-49a3-82aa-904478053559");

            migrationBuilder.DropColumn(
                name: "SubjectType",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Activities",
                newName: "ProjectId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9c5f340e-7097-469a-b006-04190cd25131", "f21b67be-827f-4a80-aaab-ed053d42f99c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e98a6e1f-a109-46b6-8afa-4d35a410339a", "1d58aa0d-91bf-4fae-97fd-bd1ef92f7af9", "User", "USER" });

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
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class DataModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BTUserProject_AspNetUsers_MemebersId",
                table: "BTUserProject");

            migrationBuilder.RenameColumn(
                name: "MemebersId",
                table: "BTUserProject",
                newName: "MembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_BTUserProject_AspNetUsers_MembersId",
                table: "BTUserProject",
                column: "MembersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BTUserProject_AspNetUsers_MembersId",
                table: "BTUserProject");

            migrationBuilder.RenameColumn(
                name: "MembersId",
                table: "BTUserProject",
                newName: "MemebersId");

            migrationBuilder.AddForeignKey(
                name: "FK_BTUserProject_AspNetUsers_MemebersId",
                table: "BTUserProject",
                column: "MemebersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

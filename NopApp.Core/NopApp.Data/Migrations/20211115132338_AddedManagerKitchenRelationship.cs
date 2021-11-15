using Microsoft.EntityFrameworkCore.Migrations;

namespace NopApp.DAL.Migrations
{
    public partial class AddedManagerKitchenRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Kitchens",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KitchenId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kitchens_ManagerId",
                table: "Kitchens",
                column: "ManagerId",
                unique: true,
                filter: "[ManagerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Kitchens_AspNetUsers_ManagerId",
                table: "Kitchens",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kitchens_AspNetUsers_ManagerId",
                table: "Kitchens");

            migrationBuilder.DropIndex(
                name: "IX_Kitchens_ManagerId",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "KitchenId",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace NopApp.DAL.Migrations
{
    public partial class UpdatedKitchenAndUserFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactInfo",
                table: "Kitchens",
                newName: "ContactPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Kitchens",
                newName: "AdditionalInformation");

            migrationBuilder.RenameColumn(
                name: "ContactPhoneNumber",
                table: "AspNetUsers",
                newName: "status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactPhoneNumber",
                table: "Kitchens",
                newName: "ContactInfo");

            migrationBuilder.RenameColumn(
                name: "AdditionalInformation",
                table: "Kitchens",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "AspNetUsers",
                newName: "ContactPhoneNumber");
        }
    }
}

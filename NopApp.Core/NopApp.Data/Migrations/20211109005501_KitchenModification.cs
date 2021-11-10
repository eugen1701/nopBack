using Microsoft.EntityFrameworkCore.Migrations;

namespace NopApp.DAL.Migrations
{
    public partial class KitchenModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Kitchens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo",
                table: "Kitchens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryInterval",
                table: "Kitchens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Kitchens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpeningHours",
                table: "Kitchens",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "ContactInfo",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "DeliveryInterval",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Kitchens");

            migrationBuilder.DropColumn(
                name: "OpeningHours",
                table: "Kitchens");
        }
    }
}

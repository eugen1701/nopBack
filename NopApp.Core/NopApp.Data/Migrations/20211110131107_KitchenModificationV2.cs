using Microsoft.EntityFrameworkCore.Migrations;

namespace NopApp.DAL.Migrations
{
    public partial class KitchenModificationV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpeningHours",
                table: "Kitchens",
                newName: "DeliveryOpenHour");

            migrationBuilder.RenameColumn(
                name: "DeliveryInterval",
                table: "Kitchens",
                newName: "DeliveryCloseHour");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryOpenHour",
                table: "Kitchens",
                newName: "OpeningHours");

            migrationBuilder.RenameColumn(
                name: "DeliveryCloseHour",
                table: "Kitchens",
                newName: "DeliveryInterval");
        }
    }
}

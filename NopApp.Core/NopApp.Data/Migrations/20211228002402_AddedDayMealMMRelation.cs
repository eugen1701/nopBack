using Microsoft.EntityFrameworkCore.Migrations;

namespace NopApp.DAL.Migrations
{
    public partial class AddedDayMealMMRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayMeal",
                columns: table => new
                {
                    DaysId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MealsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayMeal", x => new { x.DaysId, x.MealsId });
                    table.ForeignKey(
                        name: "FK_DayMeal_Days_DaysId",
                        column: x => x.DaysId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DayMeal_Meals_MealsId",
                        column: x => x.MealsId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayMeal_MealsId",
                table: "DayMeal",
                column: "MealsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayMeal");
        }
    }
}

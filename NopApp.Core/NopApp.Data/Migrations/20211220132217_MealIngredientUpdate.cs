using Microsoft.EntityFrameworkCore.Migrations;

namespace NopApp.DAL.Migrations
{
    public partial class MealIngredientUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Meals_MealId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_MealId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "MealId",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Ingredients",
                newName: "Unit");

            migrationBuilder.AddColumn<string>(
                name: "KitchenId",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MealIngredient",
                columns: table => new
                {
                    MealId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IngredientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealIngredient", x => new { x.MealId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_MealIngredient_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealIngredient_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealIngredient_IngredientId",
                table: "MealIngredient",
                column: "IngredientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealIngredient");

            migrationBuilder.DropColumn(
                name: "KitchenId",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "Ingredients",
                newName: "quantity");

            migrationBuilder.AddColumn<string>(
                name: "MealId",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_MealId",
                table: "Ingredients",
                column: "MealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Meals_MealId",
                table: "Ingredients",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

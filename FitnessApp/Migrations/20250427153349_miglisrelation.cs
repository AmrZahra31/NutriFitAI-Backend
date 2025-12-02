using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Migrations
{
    /// <inheritdoc />
    public partial class miglisrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_diet_plan_meals_meals_TbMealMealId",
                table: "diet_plan_meals");

            migrationBuilder.DropIndex(
                name: "IX_diet_plan_meals_TbMealMealId",
                table: "diet_plan_meals");

            migrationBuilder.DropColumn(
                name: "TbMealMealId",
                table: "diet_plan_meals");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUsersId",
                table: "diet_plans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_diet_plans_ApplicationUsersId",
                table: "diet_plans",
                column: "ApplicationUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_diet_plans_AspNetUsers_ApplicationUsersId",
                table: "diet_plans",
                column: "ApplicationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_diet_plans_AspNetUsers_ApplicationUsersId",
                table: "diet_plans");

            migrationBuilder.DropIndex(
                name: "IX_diet_plans_ApplicationUsersId",
                table: "diet_plans");

            migrationBuilder.DropColumn(
                name: "ApplicationUsersId",
                table: "diet_plans");

            migrationBuilder.AddColumn<int>(
                name: "TbMealMealId",
                table: "diet_plan_meals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_diet_plan_meals_TbMealMealId",
                table: "diet_plan_meals",
                column: "TbMealMealId");

            migrationBuilder.AddForeignKey(
                name: "FK_diet_plan_meals_meals_TbMealMealId",
                table: "diet_plan_meals",
                column: "TbMealMealId",
                principalTable: "meals",
                principalColumn: "meal_id");
        }
    }
}

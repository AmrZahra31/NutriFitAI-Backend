using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workouts_AspNetUsers_UserId",
                table: "workouts");

            migrationBuilder.DropIndex(
                name: "IX_workouts_UserId",
                table: "workouts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "workouts");

            migrationBuilder.AddColumn<string>(
                name: "NextWorkout",
                table: "workouts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextWorkout",
                table: "workouts");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "workouts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_workouts_UserId",
                table: "workouts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_workouts_AspNetUsers_UserId",
                table: "workouts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

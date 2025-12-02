using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Migrations
{
    /// <inheritdoc />
    public partial class real3Mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GeneralNotification",
                table: "user_settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NextMealTime",
                table: "user_settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NextWorkoutTime",
                table: "user_settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReminderToDrnkWater",
                table: "user_settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sound",
                table: "user_settings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralNotification",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "NextMealTime",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "NextWorkoutTime",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "ReminderToDrnkWater",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "Sound",
                table: "user_settings");
        }
    }
}

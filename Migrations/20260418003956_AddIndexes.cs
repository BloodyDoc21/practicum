using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanLife.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Habits_Status",
                table: "Habits",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_HabitProgresses_Date",
                table: "HabitProgresses",
                column: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Habits_Status",
                table: "Habits");

            migrationBuilder.DropIndex(
                name: "IX_HabitProgresses_Date",
                table: "HabitProgresses");
        }
    }
}

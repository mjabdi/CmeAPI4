using Microsoft.EntityFrameworkCore.Migrations;

namespace CallMeAPI.Migrations
{
    public partial class migrationsver10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WeekDays",
                table: "Widgets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekDays",
                table: "Widgets");
        }
    }
}

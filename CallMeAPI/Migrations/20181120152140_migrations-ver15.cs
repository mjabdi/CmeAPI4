using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallMeAPI.Migrations
{
    public partial class migrationsver15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerID",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CurrentPeriodStart = table.Column<DateTime>(nullable: true),
                    CurrentPeriodEnd = table.Column<DateTime>(nullable: true),
                    CustomerEmail = table.Column<string>(nullable: true),
                    TrialingUntil = table.Column<DateTime>(nullable: true),
                    BillingCycleAnchor = table.Column<DateTime>(nullable: true),
                    CanceledAtDate = table.Column<DateTime>(nullable: true),
                    PlanID = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Users");
        }
    }
}

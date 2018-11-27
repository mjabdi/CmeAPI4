using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallMeAPI.Migrations
{
    public partial class migrationsver19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceID = table.Column<string>(nullable: false),
                    AmountPaid = table.Column<decimal>(nullable: false),
                    CustomerID = table.Column<string>(nullable: true),
                    InvoiceDateTime = table.Column<DateTime>(nullable: false),
                    InvoicePdf = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PlanName = table.Column<string>(nullable: true),
                    SubscriptionID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}

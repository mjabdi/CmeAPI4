using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallMeAPI.Migrations
{
    public partial class migrationsver9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthKey",
                table: "Widgets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Widgets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CallInfos",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CallType = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    Seconds = table.Column<int>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    CallDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallInfos", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallInfos");

            migrationBuilder.DropColumn(
                name: "AuthKey",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Widgets");
        }
    }
}

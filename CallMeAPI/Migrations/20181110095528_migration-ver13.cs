using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallMeAPI.Migrations
{
    public partial class migrationver13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CallbackSchedules_Widgets_widgetID",
                table: "CallbackSchedules");

            migrationBuilder.AlterColumn<Guid>(
                name: "widgetID",
                table: "CallbackSchedules",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CallLogs",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Request = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    ReqDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallLogs", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CallbackSchedules_Widgets_widgetID",
                table: "CallbackSchedules",
                column: "widgetID",
                principalTable: "Widgets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CallbackSchedules_Widgets_widgetID",
                table: "CallbackSchedules");

            migrationBuilder.DropTable(
                name: "CallLogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "widgetID",
                table: "CallbackSchedules",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_CallbackSchedules_Widgets_widgetID",
                table: "CallbackSchedules",
                column: "widgetID",
                principalTable: "Widgets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

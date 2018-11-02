using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallMeAPI.Migrations
{
    public partial class migrationsver11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallbackSchedules",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    widgetID = table.Column<Guid>(nullable: true),
                    ScheduledDateTime = table.Column<DateTime>(nullable: false),
                    LeadName = table.Column<string>(nullable: true),
                    LeadEmail = table.Column<string>(nullable: true),
                    LeadPhoneNumber = table.Column<string>(nullable: true),
                    LeadMessage = table.Column<string>(nullable: true),
                    CallDone = table.Column<bool>(nullable: false),
                    EmailNotificationDone = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallbackSchedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CallbackSchedules_Widgets_widgetID",
                        column: x => x.widgetID,
                        principalTable: "Widgets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallbackSchedules_widgetID",
                table: "CallbackSchedules",
                column: "widgetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallbackSchedules");
        }
    }
}

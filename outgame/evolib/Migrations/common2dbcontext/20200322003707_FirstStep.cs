using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common2dbcontext
{
    public partial class FirstStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    playerIdL = table.Column<long>(nullable: false),
                    playerIdR = table.Column<long>(nullable: false),
                    timeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.playerIdL, x.playerIdR });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_playerIdR",
                table: "Friends",
                column: "playerIdR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");
        }
    }
}

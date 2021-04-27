using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class ReDefineReplayTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReplayUserHistory",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    matchId = table.Column<string>(maxLength: 64, nullable: true),
                    matchType = table.Column<int>(nullable: false),
                    resultType = table.Column<int>(nullable: false),
                    packageVersion = table.Column<ulong>(nullable: false),
                    masterDataVersion = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayUserHistory", x => new { x.playerId, x.date });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplayUserHistory");
        }
    }
}

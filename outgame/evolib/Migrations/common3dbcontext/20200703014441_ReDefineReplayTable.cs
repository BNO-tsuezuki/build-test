using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common3dbcontext
{
    public partial class ReDefineReplayTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReplayInfoAllMatch",
                columns: table => new
                {
                    date = table.Column<DateTime>(nullable: false),
                    matchId = table.Column<string>(maxLength: 64, nullable: false),
                    matchType = table.Column<int>(nullable: false),
                    mapId = table.Column<string>(maxLength: 16, nullable: true),
                    gameMode = table.Column<int>(nullable: false),
                    totalTime = table.Column<int>(nullable: false),
                    result = table.Column<string>(maxLength: 16, nullable: true),
                    mvpUserName = table.Column<string>(maxLength: 32, nullable: true),
                    mvpUnitId = table.Column<int>(nullable: false),
                    packageVersion = table.Column<ulong>(nullable: false),
                    masterDataVersion = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayInfoAllMatch", x => new { x.date, x.matchId });
                });

            migrationBuilder.CreateTable(
                name: "ReplayInfoRankMatch",
                columns: table => new
                {
                    date = table.Column<DateTime>(nullable: false),
                    matchId = table.Column<string>(maxLength: 64, nullable: false),
                    isFeatured = table.Column<bool>(nullable: false),
                    ratingAverage = table.Column<int>(nullable: false),
                    mapId = table.Column<string>(maxLength: 16, nullable: true),
                    gameMode = table.Column<int>(nullable: false),
                    mvpUnitId = table.Column<int>(nullable: false),
                    spawnUnits = table.Column<ulong>(nullable: false),
                    awardUnits = table.Column<ulong>(nullable: false),
                    packageVersion = table.Column<ulong>(nullable: false),
                    masterDataVersion = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayInfoRankMatch", x => new { x.date, x.matchId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplayInfoAllMatch");

            migrationBuilder.DropTable(
                name: "ReplayInfoRankMatch");
        }
    }
}

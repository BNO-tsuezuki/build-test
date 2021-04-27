using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common3dbcontext
{
    public partial class FirstStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchMember",
                columns: table => new
                {
                    matchId = table.Column<string>(maxLength: 64, nullable: false),
                    playersInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchMember", x => x.matchId);
                });

            migrationBuilder.CreateTable(
                name: "ReplayInfoAllMatch",
                columns: table => new
                {
                    replayVersion = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    matchId = table.Column<string>(maxLength: 64, nullable: false),
                    matchType = table.Column<int>(nullable: false),
                    mapId = table.Column<string>(maxLength: 16, nullable: true),
                    gameMode = table.Column<int>(nullable: false),
                    totalTime = table.Column<int>(nullable: false),
                    result = table.Column<string>(maxLength: 16, nullable: true),
                    mvpUserName = table.Column<string>(maxLength: 32, nullable: true),
                    mvpUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayInfoAllMatch", x => new { x.replayVersion, x.date, x.matchId });
                });

            migrationBuilder.CreateTable(
                name: "ReplayInfoRankMatch",
                columns: table => new
                {
                    replayVersion = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    matchId = table.Column<string>(maxLength: 64, nullable: false),
                    isFeatured = table.Column<bool>(nullable: false),
                    ratingAverage = table.Column<int>(nullable: false),
                    mapId = table.Column<string>(maxLength: 16, nullable: true),
                    gameMode = table.Column<int>(nullable: false),
                    mvpUnitId = table.Column<int>(nullable: false),
                    spawnUnits = table.Column<byte[]>(maxLength: 64, nullable: true),
                    awardUnits = table.Column<byte[]>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayInfoRankMatch", x => new { x.replayVersion, x.date, x.matchId });
                });

            migrationBuilder.CreateTable(
                name: "ReplayViewNum",
                columns: table => new
                {
                    matchId = table.Column<string>(maxLength: 64, nullable: false),
                    ViewNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayViewNum", x => x.matchId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchMember");

            migrationBuilder.DropTable(
                name: "ReplayInfoAllMatch");

            migrationBuilder.DropTable(
                name: "ReplayInfoRankMatch");

            migrationBuilder.DropTable(
                name: "ReplayViewNum");
        }
    }
}

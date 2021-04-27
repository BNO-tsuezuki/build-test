using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common3dbcontext
{
    public partial class DeleteReplayTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplayInfoAllMatch");

            migrationBuilder.DropTable(
                name: "ReplayInfoRankMatch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReplayInfoAllMatch",
                columns: table => new
                {
                    replayVersion = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    matchId = table.Column<string>(maxLength: 64, nullable: false),
                    gameMode = table.Column<int>(nullable: false),
                    mapId = table.Column<string>(maxLength: 16, nullable: true),
                    matchType = table.Column<int>(nullable: false),
                    mvpUnitId = table.Column<int>(nullable: false),
                    mvpUserName = table.Column<string>(maxLength: 32, nullable: true),
                    result = table.Column<string>(maxLength: 16, nullable: true),
                    totalTime = table.Column<int>(nullable: false)
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
                    awardUnits = table.Column<byte[]>(maxLength: 64, nullable: true),
                    gameMode = table.Column<int>(nullable: false),
                    isFeatured = table.Column<bool>(nullable: false),
                    mapId = table.Column<string>(maxLength: 16, nullable: true),
                    mvpUnitId = table.Column<int>(nullable: false),
                    ratingAverage = table.Column<int>(nullable: false),
                    spawnUnits = table.Column<byte[]>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayInfoRankMatch", x => new { x.replayVersion, x.date, x.matchId });
                });
        }
    }
}

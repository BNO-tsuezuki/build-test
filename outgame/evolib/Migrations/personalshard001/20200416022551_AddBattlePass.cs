using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class AddBattlePass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BattlePasses",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    passId = table.Column<int>(nullable: false),
                    totalExp = table.Column<int>(nullable: false),
                    isPremium = table.Column<bool>(nullable: false),
                    level = table.Column<int>(nullable: false),
                    rewardLevel = table.Column<int>(nullable: false),
                    createdDate = table.Column<DateTime>(nullable: false),
                    updatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattlePasses", x => new { x.playerId, x.passId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattlePasses");
        }
    }
}

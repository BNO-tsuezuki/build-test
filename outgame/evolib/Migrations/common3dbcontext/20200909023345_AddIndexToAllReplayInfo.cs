using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common3dbcontext
{
    public partial class AddIndexToAllReplayInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReplayInfoAllMatch_matchId",
                table: "ReplayInfoAllMatch",
                column: "matchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReplayInfoAllMatch_matchId",
                table: "ReplayInfoAllMatch");
        }
    }
}

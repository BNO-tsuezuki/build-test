using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class PlayerBasicInformationRmPlayerNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayerBasicInformations_playerNo_playerName",
                table: "PlayerBasicInformations");

            migrationBuilder.DropColumn(
                name: "playerNo",
                table: "PlayerBasicInformations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "playerNo",
                table: "PlayerBasicInformations",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBasicInformations_playerNo_playerName",
                table: "PlayerBasicInformations",
                columns: new[] { "playerNo", "playerName" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class AddColumnBattlePass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "levelExp",
                table: "BattlePasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nextExp",
                table: "BattlePasses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "levelExp",
                table: "BattlePasses");

            migrationBuilder.DropColumn(
                name: "nextExp",
                table: "BattlePasses");
        }
    }
}

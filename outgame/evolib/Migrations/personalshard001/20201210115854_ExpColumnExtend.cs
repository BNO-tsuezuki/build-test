using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class ExpColumnExtend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "totalExp",
                table: "BattlePasses",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<ulong>(
                name: "nextExp",
                table: "BattlePasses",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<ulong>(
                name: "levelExp",
                table: "BattlePasses",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "totalExp",
                table: "BattlePasses",
                nullable: false,
                oldClrType: typeof(ulong));

            migrationBuilder.AlterColumn<int>(
                name: "nextExp",
                table: "BattlePasses",
                nullable: false,
                oldClrType: typeof(ulong));

            migrationBuilder.AlterColumn<int>(
                name: "levelExp",
                table: "BattlePasses",
                nullable: false,
                oldClrType: typeof(ulong));
        }
    }
}

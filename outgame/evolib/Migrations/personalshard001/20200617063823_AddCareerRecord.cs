using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class AddCareerRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CareerRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    playerId = table.Column<long>(nullable: false),
                    matchType = table.Column<int>(nullable: false),
                    seasonNo = table.Column<int>(nullable: false),
                    recordItemId = table.Column<string>(maxLength: 6, nullable: false),
                    mobileSuitId = table.Column<string>(maxLength: 32, nullable: false),
                    intValue = table.Column<int>(nullable: false),
                    decimalValue = table.Column<double>(nullable: false),
                    numForAverage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareerRecords_playerId_matchType_seasonNo_recordItemId_mobil~",
                table: "CareerRecords",
                columns: new[] { "playerId", "matchType", "seasonNo", "recordItemId", "mobileSuitId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareerRecords");
        }
    }
}

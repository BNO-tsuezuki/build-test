using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class AddAssetsInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetsInventories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    playerId = table.Column<long>(nullable: false),
                    assetsId = table.Column<string>(nullable: false),
                    amount = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsInventories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetsInventories_playerId_assetsId",
                table: "AssetsInventories",
                columns: new[] { "playerId", "assetsId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetsInventories");
        }
    }
}

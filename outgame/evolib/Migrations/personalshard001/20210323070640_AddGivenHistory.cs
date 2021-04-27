using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class AddGivenHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GivenHistorys",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    playerId = table.Column<long>(nullable: false),
                    obtainedDate = table.Column<DateTime>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    presentId = table.Column<string>(maxLength: 32, nullable: true),
                    amount = table.Column<long>(nullable: false),
                    giveType = table.Column<int>(nullable: false),
                    text = table.Column<string>(maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GivenHistorys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GivenHistorys_playerId_obtainedDate",
                table: "GivenHistorys",
                columns: new[] { "playerId", "obtainedDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GivenHistorys");
        }
    }
}

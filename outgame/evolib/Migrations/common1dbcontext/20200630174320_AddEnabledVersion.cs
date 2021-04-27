using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common1dbcontext
{
    public partial class AddEnabledVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnabledVersions",
                columns: table => new
                {
                    checkTarget = table.Column<int>(nullable: false),
                    referenceSrc = table.Column<int>(nullable: false),
                    major = table.Column<int>(nullable: false),
                    minor = table.Column<int>(nullable: false),
                    patch = table.Column<int>(nullable: false),
                    build = table.Column<int>(nullable: false),
                    updateDate = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnabledVersions", x => new { x.checkTarget, x.referenceSrc });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnabledVersions");
        }
    }
}

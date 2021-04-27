using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common1dbcontext
{
    public partial class AddClientPackageVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientPackageVersions",
                columns: table => new
                {
                    version = table.Column<string>(nullable: false),
                    comment = table.Column<string>(nullable: false),
                    enabled = table.Column<int>(nullable: false),
                    inserted = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPackageVersions", x => x.version);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientPackageVersions");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LogProcess.Migrations
{
    public partial class AddSessionCountHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "session_count_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    area_name = table.Column<string>(type: "varchar(200)", nullable: false),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_count_history", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_session_count_history_area_name_datetime",
                table: "session_count_history",
                columns: new[] { "area_name", "datetime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "session_count_history");
        }
    }
}

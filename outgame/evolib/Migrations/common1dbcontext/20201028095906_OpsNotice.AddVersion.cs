using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common1dbcontext
{
    public partial class OpsNoticeAddVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "OpsNotices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "version",
                table: "OpsNotices");
        }
    }
}

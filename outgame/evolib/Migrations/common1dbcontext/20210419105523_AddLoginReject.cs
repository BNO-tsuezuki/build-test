using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common1dbcontext
{
    public partial class AddLoginReject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginRejects",
                columns: table => new
                {
                    target = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginRejects", x => new { x.target, x.value });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginRejects");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationServer.Migrations
{
    public partial class FirstStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    account = table.Column<string>(nullable: false),
                    hashedPassword = table.Column<string>(nullable: false),
                    salt = table.Column<string>(nullable: false),
                    permission = table.Column<int>(nullable: false),
                    nickname = table.Column<string>(nullable: false),
                    hostType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.account);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}

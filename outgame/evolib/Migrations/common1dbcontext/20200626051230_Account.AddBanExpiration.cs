using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common1dbcontext
{
    public partial class AccountAddBanExpiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "banExpiration",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banExpiration",
                table: "Accounts");
        }
    }
}

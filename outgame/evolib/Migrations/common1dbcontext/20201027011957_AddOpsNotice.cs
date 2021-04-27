using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common1dbcontext
{
    public partial class AddOpsNotice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpsNotices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updateDate = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    release = table.Column<bool>(nullable: false),
                    target = table.Column<ulong>(nullable: false),
                    memo = table.Column<string>(nullable: true),
                    beginDate = table.Column<DateTime>(nullable: false),
                    endDate = table.Column<DateTime>(nullable: false),
                    enabledEnglish = table.Column<bool>(nullable: false),
                    msgEnglish = table.Column<string>(nullable: true),
                    enabledFrench = table.Column<bool>(nullable: false),
                    msgFrench = table.Column<string>(nullable: true),
                    enabledGerman = table.Column<bool>(nullable: false),
                    msgGerman = table.Column<string>(nullable: true),
                    enabledJapanese = table.Column<bool>(nullable: false),
                    msgJapanese = table.Column<string>(nullable: true),
                    optNoticeType = table.Column<int>(nullable: false),
                    times = table.Column<int>(nullable: false),
                    repeateIntervalMinutes = table.Column<int>(nullable: false),
                    titleEnglish = table.Column<string>(nullable: true),
                    titleFrench = table.Column<string>(nullable: true),
                    titleGerman = table.Column<string>(nullable: true),
                    titleJapanese = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpsNotices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpsNotices");
        }
    }
}

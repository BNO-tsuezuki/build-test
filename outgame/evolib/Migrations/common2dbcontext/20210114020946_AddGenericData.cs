using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.common2dbcontext
{
    public partial class AddGenericData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenericDatas",
                columns: table => new
                {
                    type = table.Column<int>(nullable: false),
                    data1 = table.Column<string>(nullable: true),
                    data2 = table.Column<string>(nullable: true),
                    data3 = table.Column<string>(nullable: true),
                    data4 = table.Column<string>(nullable: true),
                    data5 = table.Column<string>(nullable: true),
                    data6 = table.Column<string>(nullable: true),
                    data7 = table.Column<string>(nullable: true),
                    data8 = table.Column<string>(nullable: true),
                    timeStamp = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericDatas", x => x.type);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenericDatas");
        }
    }
}

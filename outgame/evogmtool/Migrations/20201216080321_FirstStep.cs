using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evogmtool.Migrations
{
    public partial class FirstStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthLogs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    account = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: false),
                    result = table.Column<sbyte>(type: "tinyint(4)", nullable: false),
                    ipAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthLogs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    domainId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    domainName = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.domainId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    languageCode = table.Column<string>(type: "char(2)", nullable: false),
                    languageName = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.languageCode);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    publisherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    publisherName = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.publisherId);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    regionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    regionCode = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.regionId);
                });

            migrationBuilder.CreateTable(
                name: "Timezones",
                columns: table => new
                {
                    timezoneCode = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timezones", x => x.timezoneCode);
                });

            migrationBuilder.CreateTable(
                name: "DomainRegions",
                columns: table => new
                {
                    domainId = table.Column<int>(type: "int", nullable: false),
                    regionId = table.Column<int>(type: "int", nullable: false),
                    publisherId = table.Column<int>(type: "int", nullable: false),
                    target = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainRegions", x => new { x.domainId, x.regionId });
                    table.ForeignKey(
                        name: "FK_DomainRegions_Domains_domainId",
                        column: x => x.domainId,
                        principalTable: "Domains",
                        principalColumn: "domainId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DomainRegions_Publishers_publisherId",
                        column: x => x.publisherId,
                        principalTable: "Publishers",
                        principalColumn: "publisherId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DomainRegions_Regions_regionId",
                        column: x => x.regionId,
                        principalTable: "Regions",
                        principalColumn: "regionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    account = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: false),
                    salt = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    passwordHash = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    publisherId = table.Column<int>(type: "int", nullable: false),
                    timezoneCode = table.Column<string>(type: "varchar(100)", nullable: false),
                    languageCode = table.Column<string>(type: "char(2)", nullable: false),
                    isAvailable = table.Column<bool>(type: "bit(1)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Users_Languages_languageCode",
                        column: x => x.languageCode,
                        principalTable: "Languages",
                        principalColumn: "languageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Publishers_publisherId",
                        column: x => x.publisherId,
                        principalTable: "Publishers",
                        principalColumn: "publisherId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Timezones_timezoneCode",
                        column: x => x.timezoneCode,
                        principalTable: "Timezones",
                        principalColumn: "timezoneCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DomainRegionLanguages",
                columns: table => new
                {
                    domainId = table.Column<int>(type: "int", nullable: false),
                    regionId = table.Column<int>(type: "int", nullable: false),
                    languageCode = table.Column<string>(type: "char(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainRegionLanguages", x => new { x.domainId, x.regionId, x.languageCode });
                    table.ForeignKey(
                        name: "FK_DomainRegionLanguages_Languages_languageCode",
                        column: x => x.languageCode,
                        principalTable: "Languages",
                        principalColumn: "languageCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DomainRegionLanguages_DomainRegions_domainId_regionId",
                        columns: x => new { x.domainId, x.regionId },
                        principalTable: "DomainRegions",
                        principalColumns: new[] { "domainId", "regionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationLogs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(nullable: true),
                    statusCode = table.Column<short>(type: "smallint(6)", nullable: false),
                    method = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    url = table.Column<string>(type: "tinytext", nullable: false),
                    queryString = table.Column<string>(type: "text", nullable: true),
                    requestBody = table.Column<string>(type: "mediumtext", nullable: true),
                    responseBody = table.Column<string>(type: "mediumtext", nullable: true),
                    exception = table.Column<string>(type: "mediumtext", nullable: true),
                    ipAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_OperationLogs_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthLogs_createdAt",
                table: "AuthLogs",
                column: "createdAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuthLogs_account_createdAt",
                table: "AuthLogs",
                columns: new[] { "account", "createdAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AuthLogs_ipAddress_createdAt",
                table: "AuthLogs",
                columns: new[] { "ipAddress", "createdAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DomainRegionLanguages_languageCode",
                table: "DomainRegionLanguages",
                column: "languageCode");

            migrationBuilder.CreateIndex(
                name: "IX_DomainRegions_publisherId",
                table: "DomainRegions",
                column: "publisherId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainRegions_regionId",
                table: "DomainRegions",
                column: "regionId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainRegions_target",
                table: "DomainRegions",
                column: "target",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationLogs_createdAt",
                table: "OperationLogs",
                column: "createdAt");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLogs_userId_createdAt",
                table: "OperationLogs",
                columns: new[] { "userId", "createdAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_account",
                table: "Users",
                column: "account",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_languageCode",
                table: "Users",
                column: "languageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_publisherId",
                table: "Users",
                column: "publisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_timezoneCode",
                table: "Users",
                column: "timezoneCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthLogs");

            migrationBuilder.DropTable(
                name: "DomainRegionLanguages");

            migrationBuilder.DropTable(
                name: "OperationLogs");

            migrationBuilder.DropTable(
                name: "DomainRegions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Timezones");
        }
    }
}

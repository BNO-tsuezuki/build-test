using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class FirstStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppOptions",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    category = table.Column<int>(nullable: false),
                    value0 = table.Column<int>(nullable: false),
                    value1 = table.Column<int>(nullable: false),
                    value2 = table.Column<int>(nullable: false),
                    value3 = table.Column<int>(nullable: false),
                    value4 = table.Column<int>(nullable: false),
                    value5 = table.Column<int>(nullable: false),
                    value6 = table.Column<int>(nullable: false),
                    value7 = table.Column<int>(nullable: false),
                    value8 = table.Column<int>(nullable: false),
                    value9 = table.Column<int>(nullable: false),
                    value10 = table.Column<int>(nullable: false),
                    value11 = table.Column<int>(nullable: false),
                    value12 = table.Column<int>(nullable: false),
                    value13 = table.Column<int>(nullable: false),
                    value14 = table.Column<int>(nullable: false),
                    value15 = table.Column<int>(nullable: false),
                    value16 = table.Column<int>(nullable: false),
                    value17 = table.Column<int>(nullable: false),
                    value18 = table.Column<int>(nullable: false),
                    value19 = table.Column<int>(nullable: false),
                    value20 = table.Column<int>(nullable: false),
                    value21 = table.Column<int>(nullable: false),
                    value22 = table.Column<int>(nullable: false),
                    value23 = table.Column<int>(nullable: false),
                    value24 = table.Column<int>(nullable: false),
                    value25 = table.Column<int>(nullable: false),
                    value26 = table.Column<int>(nullable: false),
                    value27 = table.Column<int>(nullable: false),
                    value28 = table.Column<int>(nullable: false),
                    value29 = table.Column<int>(nullable: false),
                    value30 = table.Column<int>(nullable: false),
                    value31 = table.Column<int>(nullable: false),
                    value32 = table.Column<int>(nullable: false),
                    value33 = table.Column<int>(nullable: false),
                    value34 = table.Column<int>(nullable: false),
                    value35 = table.Column<int>(nullable: false),
                    value36 = table.Column<int>(nullable: false),
                    value37 = table.Column<int>(nullable: false),
                    value38 = table.Column<int>(nullable: false),
                    value39 = table.Column<int>(nullable: false),
                    value40 = table.Column<int>(nullable: false),
                    value41 = table.Column<int>(nullable: false),
                    value42 = table.Column<int>(nullable: false),
                    value43 = table.Column<int>(nullable: false),
                    value44 = table.Column<int>(nullable: false),
                    value45 = table.Column<int>(nullable: false),
                    value46 = table.Column<int>(nullable: false),
                    value47 = table.Column<int>(nullable: false),
                    value48 = table.Column<int>(nullable: false),
                    value49 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOptions", x => new { x.playerId, x.category });
                });

            migrationBuilder.CreateTable(
                name: "DateLogs",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    OnlineStamp = table.Column<DateTime>(nullable: false),
                    FriendRequestPageLastView = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateLogs", x => x.playerId);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteFriends",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    favoritePlayerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteFriends", x => new { x.playerId, x.favoritePlayerId });
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    playerIdDst = table.Column<long>(nullable: false),
                    playerIdSrc = table.Column<long>(nullable: false),
                    timeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => new { x.playerIdDst, x.playerIdSrc });
                });

            migrationBuilder.CreateTable(
                name: "ItemInventories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    playerId = table.Column<long>(nullable: false),
                    itemType = table.Column<int>(nullable: false),
                    itemId = table.Column<string>(maxLength: 32, nullable: false),
                    obtainedDate = table.Column<DateTime>(nullable: false),
                    obtainedWay = table.Column<int>(nullable: false),
                    isNew = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileSuitOptions",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    mobileSuitId = table.Column<string>(maxLength: 32, nullable: false),
                    value0 = table.Column<int>(nullable: false),
                    value1 = table.Column<int>(nullable: false),
                    value2 = table.Column<int>(nullable: false),
                    value3 = table.Column<int>(nullable: false),
                    value4 = table.Column<int>(nullable: false),
                    value5 = table.Column<int>(nullable: false),
                    value6 = table.Column<int>(nullable: false),
                    value7 = table.Column<int>(nullable: false),
                    value8 = table.Column<int>(nullable: false),
                    value9 = table.Column<int>(nullable: false),
                    value10 = table.Column<int>(nullable: false),
                    value11 = table.Column<int>(nullable: false),
                    value12 = table.Column<int>(nullable: false),
                    value13 = table.Column<int>(nullable: false),
                    value14 = table.Column<int>(nullable: false),
                    value15 = table.Column<int>(nullable: false),
                    value16 = table.Column<int>(nullable: false),
                    value17 = table.Column<int>(nullable: false),
                    value18 = table.Column<int>(nullable: false),
                    value19 = table.Column<int>(nullable: false),
                    value20 = table.Column<int>(nullable: false),
                    value21 = table.Column<int>(nullable: false),
                    value22 = table.Column<int>(nullable: false),
                    value23 = table.Column<int>(nullable: false),
                    value24 = table.Column<int>(nullable: false),
                    value25 = table.Column<int>(nullable: false),
                    value26 = table.Column<int>(nullable: false),
                    value27 = table.Column<int>(nullable: false),
                    value28 = table.Column<int>(nullable: false),
                    value29 = table.Column<int>(nullable: false),
                    value30 = table.Column<int>(nullable: false),
                    value31 = table.Column<int>(nullable: false),
                    value32 = table.Column<int>(nullable: false),
                    value33 = table.Column<int>(nullable: false),
                    value34 = table.Column<int>(nullable: false),
                    value35 = table.Column<int>(nullable: false),
                    value36 = table.Column<int>(nullable: false),
                    value37 = table.Column<int>(nullable: false),
                    value38 = table.Column<int>(nullable: false),
                    value39 = table.Column<int>(nullable: false),
                    value40 = table.Column<int>(nullable: false),
                    value41 = table.Column<int>(nullable: false),
                    value42 = table.Column<int>(nullable: false),
                    value43 = table.Column<int>(nullable: false),
                    value44 = table.Column<int>(nullable: false),
                    value45 = table.Column<int>(nullable: false),
                    value46 = table.Column<int>(nullable: false),
                    value47 = table.Column<int>(nullable: false),
                    value48 = table.Column<int>(nullable: false),
                    value49 = table.Column<int>(nullable: false),
                    value50 = table.Column<int>(nullable: false),
                    value51 = table.Column<int>(nullable: false),
                    value52 = table.Column<int>(nullable: false),
                    value53 = table.Column<int>(nullable: false),
                    value54 = table.Column<int>(nullable: false),
                    value55 = table.Column<int>(nullable: false),
                    value56 = table.Column<int>(nullable: false),
                    value57 = table.Column<int>(nullable: false),
                    value58 = table.Column<int>(nullable: false),
                    value59 = table.Column<int>(nullable: false),
                    value60 = table.Column<int>(nullable: false),
                    value61 = table.Column<int>(nullable: false),
                    value62 = table.Column<int>(nullable: false),
                    value63 = table.Column<int>(nullable: false),
                    value64 = table.Column<int>(nullable: false),
                    value65 = table.Column<int>(nullable: false),
                    value66 = table.Column<int>(nullable: false),
                    value67 = table.Column<int>(nullable: false),
                    value68 = table.Column<int>(nullable: false),
                    value69 = table.Column<int>(nullable: false),
                    value70 = table.Column<int>(nullable: false),
                    value71 = table.Column<int>(nullable: false),
                    value72 = table.Column<int>(nullable: false),
                    value73 = table.Column<int>(nullable: false),
                    value74 = table.Column<int>(nullable: false),
                    value75 = table.Column<int>(nullable: false),
                    value76 = table.Column<int>(nullable: false),
                    value77 = table.Column<int>(nullable: false),
                    value78 = table.Column<int>(nullable: false),
                    value79 = table.Column<int>(nullable: false),
                    value80 = table.Column<int>(nullable: false),
                    value81 = table.Column<int>(nullable: false),
                    value82 = table.Column<int>(nullable: false),
                    value83 = table.Column<int>(nullable: false),
                    value84 = table.Column<int>(nullable: false),
                    value85 = table.Column<int>(nullable: false),
                    value86 = table.Column<int>(nullable: false),
                    value87 = table.Column<int>(nullable: false),
                    value88 = table.Column<int>(nullable: false),
                    value89 = table.Column<int>(nullable: false),
                    value90 = table.Column<int>(nullable: false),
                    value91 = table.Column<int>(nullable: false),
                    value92 = table.Column<int>(nullable: false),
                    value93 = table.Column<int>(nullable: false),
                    value94 = table.Column<int>(nullable: false),
                    value95 = table.Column<int>(nullable: false),
                    value96 = table.Column<int>(nullable: false),
                    value97 = table.Column<int>(nullable: false),
                    value98 = table.Column<int>(nullable: false),
                    value99 = table.Column<int>(nullable: false),
                    value100 = table.Column<int>(nullable: false),
                    value101 = table.Column<int>(nullable: false),
                    value102 = table.Column<int>(nullable: false),
                    value103 = table.Column<int>(nullable: false),
                    value104 = table.Column<int>(nullable: false),
                    value105 = table.Column<int>(nullable: false),
                    value106 = table.Column<int>(nullable: false),
                    value107 = table.Column<int>(nullable: false),
                    value108 = table.Column<int>(nullable: false),
                    value109 = table.Column<int>(nullable: false),
                    value110 = table.Column<int>(nullable: false),
                    value111 = table.Column<int>(nullable: false),
                    value112 = table.Column<int>(nullable: false),
                    value113 = table.Column<int>(nullable: false),
                    value114 = table.Column<int>(nullable: false),
                    value115 = table.Column<int>(nullable: false),
                    value116 = table.Column<int>(nullable: false),
                    value117 = table.Column<int>(nullable: false),
                    value118 = table.Column<int>(nullable: false),
                    value119 = table.Column<int>(nullable: false),
                    value120 = table.Column<int>(nullable: false),
                    value121 = table.Column<int>(nullable: false),
                    value122 = table.Column<int>(nullable: false),
                    value123 = table.Column<int>(nullable: false),
                    value124 = table.Column<int>(nullable: false),
                    value125 = table.Column<int>(nullable: false),
                    value126 = table.Column<int>(nullable: false),
                    value127 = table.Column<int>(nullable: false),
                    value128 = table.Column<int>(nullable: false),
                    value129 = table.Column<int>(nullable: false),
                    value130 = table.Column<int>(nullable: false),
                    value131 = table.Column<int>(nullable: false),
                    value132 = table.Column<int>(nullable: false),
                    value133 = table.Column<int>(nullable: false),
                    value134 = table.Column<int>(nullable: false),
                    value135 = table.Column<int>(nullable: false),
                    value136 = table.Column<int>(nullable: false),
                    value137 = table.Column<int>(nullable: false),
                    value138 = table.Column<int>(nullable: false),
                    value139 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileSuitOptions", x => new { x.playerId, x.mobileSuitId });
                });

            migrationBuilder.CreateTable(
                name: "MutePlayers",
                columns: table => new
                {
                    playerIdSrc = table.Column<long>(nullable: false),
                    playerIdDst = table.Column<long>(nullable: false),
                    text = table.Column<bool>(nullable: false),
                    voice = table.Column<bool>(nullable: false),
                    timeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MutePlayers", x => new { x.playerIdSrc, x.playerIdDst });
                });

            migrationBuilder.CreateTable(
                name: "OwnMobileSuitSettings",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    mobileSuitId = table.Column<string>(maxLength: 32, nullable: false),
                    voicePackItemId = table.Column<string>(maxLength: 32, nullable: false),
                    ornamentItemId = table.Column<string>(maxLength: 32, nullable: false),
                    bodySkinItemId = table.Column<string>(maxLength: 32, nullable: false),
                    weaponSkinItemId = table.Column<string>(maxLength: 32, nullable: false),
                    mvpCelebrationItemId = table.Column<string>(maxLength: 32, nullable: false),
                    stampSlotItemId1 = table.Column<string>(maxLength: 32, nullable: false),
                    stampSlotItemId2 = table.Column<string>(maxLength: 32, nullable: false),
                    stampSlotItemId3 = table.Column<string>(maxLength: 32, nullable: false),
                    stampSlotItemId4 = table.Column<string>(maxLength: 32, nullable: false),
                    emotionSlotItemId1 = table.Column<string>(maxLength: 32, nullable: false),
                    emotionSlotItemId2 = table.Column<string>(maxLength: 32, nullable: false),
                    emotionSlotItemId3 = table.Column<string>(maxLength: 32, nullable: false),
                    emotionSlotItemId4 = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnMobileSuitSettings", x => new { x.playerId, x.mobileSuitId });
                });

            migrationBuilder.CreateTable(
                name: "OwnVoicePackSettings",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    mobileSuitId = table.Column<string>(maxLength: 32, nullable: false),
                    voicePackItemId = table.Column<string>(maxLength: 32, nullable: false),
                    voiceId1 = table.Column<string>(maxLength: 32, nullable: false),
                    voiceId2 = table.Column<string>(maxLength: 32, nullable: false),
                    voiceId3 = table.Column<string>(maxLength: 32, nullable: false),
                    voiceId4 = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnVoicePackSettings", x => new { x.playerId, x.mobileSuitId, x.voicePackItemId });
                });

            migrationBuilder.CreateTable(
                name: "PlayerBasicInformations",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    firstLogin = table.Column<DateTime>(nullable: false),
                    playerNo = table.Column<uint>(nullable: false),
                    playerName = table.Column<string>(maxLength: 32, nullable: false),
                    initialLevel = table.Column<int>(nullable: false),
                    playerIconItemId = table.Column<string>(maxLength: 32, nullable: false),
                    pretendOffline = table.Column<bool>(nullable: false),
                    openType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBasicInformations", x => x.playerId);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBattleInformations",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    rating = table.Column<float>(nullable: false),
                    victory = table.Column<int>(nullable: false),
                    defeat = table.Column<int>(nullable: false),
                    draw = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBattleInformations", x => x.playerId);
                });

            migrationBuilder.CreateTable(
                name: "ReplayUserHistory",
                columns: table => new
                {
                    playerId = table.Column<long>(nullable: false),
                    replayVersion = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    matchId = table.Column<string>(maxLength: 64, nullable: true),
                    matchType = table.Column<int>(nullable: false),
                    resultType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayUserHistory", x => new { x.playerId, x.replayVersion, x.date });
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemInventories_playerId_itemId",
                table: "ItemInventories",
                columns: new[] { "playerId", "itemId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemInventories_playerId_itemType",
                table: "ItemInventories",
                columns: new[] { "playerId", "itemType" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBasicInformations_playerNo_playerName",
                table: "PlayerBasicInformations",
                columns: new[] { "playerNo", "playerName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppOptions");

            migrationBuilder.DropTable(
                name: "DateLogs");

            migrationBuilder.DropTable(
                name: "FavoriteFriends");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "ItemInventories");

            migrationBuilder.DropTable(
                name: "MobileSuitOptions");

            migrationBuilder.DropTable(
                name: "MutePlayers");

            migrationBuilder.DropTable(
                name: "OwnMobileSuitSettings");

            migrationBuilder.DropTable(
                name: "OwnVoicePackSettings");

            migrationBuilder.DropTable(
                name: "PlayerBasicInformations");

            migrationBuilder.DropTable(
                name: "PlayerBattleInformations");

            migrationBuilder.DropTable(
                name: "ReplayUserHistory");
        }
    }
}

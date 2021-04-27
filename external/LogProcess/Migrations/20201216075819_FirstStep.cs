using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LogProcess.Migrations
{
    public partial class FirstStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chat_direct_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "nvarchar(96)", nullable: false),
                    target_player_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_direct_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat_say_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    chat_type = table.Column<int>(type: "int", nullable: false),
                    group_id = table.Column<string>(type: "varchar(44)", nullable: false),
                    match_id = table.Column<string>(type: "varchar(43)", nullable: false),
                    side = table.Column<int>(type: "int", nullable: false),
                    text = table.Column<string>(type: "nvarchar(96)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_say_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "login_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    remote_ip = table.Column<string>(type: "varchar(45)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logout_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    remote_ip = table.Column<string>(type: "varchar(45)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logout_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "match_cue_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    group_id = table.Column<string>(type: "varchar(44)", nullable: false),
                    match_format = table.Column<int>(type: "int", nullable: false),
                    player_id_1 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_2 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_3 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_4 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_5 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_6 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_match_cue_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "match_entry_player_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    match_format = table.Column<int>(type: "int", nullable: false),
                    match_id = table.Column<string>(type: "varchar(43)", nullable: false),
                    match_entry_reason = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_match_entry_player_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "match_execution_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    match_format = table.Column<int>(type: "int", nullable: false),
                    match_id = table.Column<string>(type: "varchar(43)", nullable: false),
                    rule_format = table.Column<int>(type: "int", nullable: false),
                    match_win_team = table.Column<int>(type: "int", nullable: false),
                    match_lose_team = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_match_execution_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "match_exit_player_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    match_format = table.Column<int>(type: "int", nullable: false),
                    match_id = table.Column<string>(type: "varchar(43)", nullable: false),
                    match_exit_reason = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_match_exit_player_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "match_start_player_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    match_format = table.Column<int>(type: "int", nullable: false),
                    match_id = table.Column<string>(type: "varchar(43)", nullable: false),
                    rule_format = table.Column<int>(type: "int", nullable: false),
                    player_id_a_1 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_a_2 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_a_3 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_a_4 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_a_5 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_a_6 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_b_1 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_b_2 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_b_3 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_b_4 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_b_5 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_b_6 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_match_start_player_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "party_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    group_id = table.Column<string>(type: "varchar(44)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    player_id_1 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_2 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_3 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_4 = table.Column<long>(type: "bigint", nullable: true),
                    player_id_5 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_party_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "player_account_create_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    player_name = table.Column<string>(type: "varchar(32)", nullable: false),
                    account_type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_account_create_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "player_exp_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    exp = table.Column<int>(type: "int", nullable: false),
                    total_exp = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_exp_history", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_direct_history_datetime",
                table: "chat_direct_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_chat_direct_history_player_id_datetime",
                table: "chat_direct_history",
                columns: new[] { "player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_chat_direct_history_target_player_id_datetime",
                table: "chat_direct_history",
                columns: new[] { "target_player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_chat_say_history_datetime",
                table: "chat_say_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_chat_say_history_group_id_datetime",
                table: "chat_say_history",
                columns: new[] { "group_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_chat_say_history_match_id_datetime",
                table: "chat_say_history",
                columns: new[] { "match_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_chat_say_history_player_id_datetime",
                table: "chat_say_history",
                columns: new[] { "player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_login_history_datetime",
                table: "login_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_login_history_player_id_datetime",
                table: "login_history",
                columns: new[] { "player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_login_history_remote_ip_datetime",
                table: "login_history",
                columns: new[] { "remote_ip", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_logout_history_datetime",
                table: "logout_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_logout_history_player_id_datetime",
                table: "logout_history",
                columns: new[] { "player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_logout_history_remote_ip_datetime",
                table: "logout_history",
                columns: new[] { "remote_ip", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_cue_history_datetime",
                table: "match_cue_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_match_cue_history_player_id_1_datetime",
                table: "match_cue_history",
                columns: new[] { "player_id_1", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_cue_history_player_id_2_datetime",
                table: "match_cue_history",
                columns: new[] { "player_id_2", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_cue_history_player_id_3_datetime",
                table: "match_cue_history",
                columns: new[] { "player_id_3", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_cue_history_player_id_4_datetime",
                table: "match_cue_history",
                columns: new[] { "player_id_4", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_cue_history_player_id_5_datetime",
                table: "match_cue_history",
                columns: new[] { "player_id_5", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_cue_history_player_id_6_datetime",
                table: "match_cue_history",
                columns: new[] { "player_id_6", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_entry_player_history_datetime",
                table: "match_entry_player_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_match_entry_player_history_match_id",
                table: "match_entry_player_history",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_entry_player_history_player_id_datetime",
                table: "match_entry_player_history",
                columns: new[] { "player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_execution_history_datetime",
                table: "match_execution_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_match_execution_history_match_id",
                table: "match_execution_history",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_exit_player_history_datetime",
                table: "match_exit_player_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_match_exit_player_history_match_id",
                table: "match_exit_player_history",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_exit_player_history_player_id_datetime",
                table: "match_exit_player_history",
                columns: new[] { "player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_datetime",
                table: "match_start_player_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_match_id",
                table: "match_start_player_history",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_a_1_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_a_1", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_a_2_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_a_2", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_a_3_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_a_3", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_a_4_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_a_4", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_a_5_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_a_5", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_a_6_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_a_6", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_b_1_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_b_1", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_b_2_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_b_2", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_b_3_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_b_3", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_b_4_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_b_4", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_b_5_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_b_5", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_match_start_player_history_player_id_b_6_datetime",
                table: "match_start_player_history",
                columns: new[] { "player_id_b_6", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_party_history_datetime",
                table: "party_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_party_history_group_id",
                table: "party_history",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_party_history_player_id_datetime",
                table: "party_history",
                columns: new[] { "player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_player_account_create_history_datetime",
                table: "player_account_create_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_player_account_create_history_player_id_datetime",
                table: "player_account_create_history",
                columns: new[] { "player_id", "datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_player_exp_history_datetime",
                table: "player_exp_history",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_player_exp_history_player_id_datetime",
                table: "player_exp_history",
                columns: new[] { "player_id", "datetime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_direct_history");

            migrationBuilder.DropTable(
                name: "chat_say_history");

            migrationBuilder.DropTable(
                name: "login_history");

            migrationBuilder.DropTable(
                name: "logout_history");

            migrationBuilder.DropTable(
                name: "match_cue_history");

            migrationBuilder.DropTable(
                name: "match_entry_player_history");

            migrationBuilder.DropTable(
                name: "match_execution_history");

            migrationBuilder.DropTable(
                name: "match_exit_player_history");

            migrationBuilder.DropTable(
                name: "match_start_player_history");

            migrationBuilder.DropTable(
                name: "party_history");

            migrationBuilder.DropTable(
                name: "player_account_create_history");

            migrationBuilder.DropTable(
                name: "player_exp_history");
        }
    }
}

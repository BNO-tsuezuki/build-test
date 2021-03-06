// <auto-generated />
using System;
using LogProcess.Databases.EvoGameLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LogProcess.Migrations
{
    [DbContext(typeof(EvoGameLogDbContext))]
    [Migration("20210128100009_AddSessionCountHistory")]
    partial class AddSessionCountHistory
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.ChatDirectHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.Property<long>("TargetPlayerId")
                        .HasColumnName("target_player_id")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text")
                        .HasColumnType("nvarchar(96)");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("PlayerId", "Datetime");

                    b.HasIndex("TargetPlayerId", "Datetime");

                    b.ToTable("chat_direct_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.ChatSayHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<int>("ChatType")
                        .HasColumnName("chat_type")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnName("group_id")
                        .HasColumnType("varchar(44)");

                    b.Property<string>("MatchId")
                        .IsRequired()
                        .HasColumnName("match_id")
                        .HasColumnType("varchar(43)");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.Property<int>("Side")
                        .HasColumnName("side")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text")
                        .HasColumnType("nvarchar(96)");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("GroupId", "Datetime");

                    b.HasIndex("MatchId", "Datetime");

                    b.HasIndex("PlayerId", "Datetime");

                    b.ToTable("chat_say_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.LoginHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.Property<string>("RemoteIp")
                        .IsRequired()
                        .HasColumnName("remote_ip")
                        .HasColumnType("varchar(45)");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("PlayerId", "Datetime");

                    b.HasIndex("RemoteIp", "Datetime");

                    b.ToTable("login_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.LogoutHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.Property<string>("RemoteIp")
                        .IsRequired()
                        .HasColumnName("remote_ip")
                        .HasColumnType("varchar(45)");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("PlayerId", "Datetime");

                    b.HasIndex("RemoteIp", "Datetime");

                    b.ToTable("logout_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.MatchCueHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnName("group_id")
                        .HasColumnType("varchar(44)");

                    b.Property<int>("MatchFormat")
                        .HasColumnName("match_format")
                        .HasColumnType("int");

                    b.Property<long?>("PlayerId1")
                        .HasColumnName("player_id_1")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId2")
                        .HasColumnName("player_id_2")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId3")
                        .HasColumnName("player_id_3")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId4")
                        .HasColumnName("player_id_4")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId5")
                        .HasColumnName("player_id_5")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId6")
                        .HasColumnName("player_id_6")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("PlayerId1", "Datetime");

                    b.HasIndex("PlayerId2", "Datetime");

                    b.HasIndex("PlayerId3", "Datetime");

                    b.HasIndex("PlayerId4", "Datetime");

                    b.HasIndex("PlayerId5", "Datetime");

                    b.HasIndex("PlayerId6", "Datetime");

                    b.ToTable("match_cue_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.MatchEntryPlayerHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MatchEntryReason")
                        .HasColumnName("match_entry_reason")
                        .HasColumnType("int");

                    b.Property<int>("MatchFormat")
                        .HasColumnName("match_format")
                        .HasColumnType("int");

                    b.Property<string>("MatchId")
                        .IsRequired()
                        .HasColumnName("match_id")
                        .HasColumnType("varchar(43)");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId", "Datetime");

                    b.ToTable("match_entry_player_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.MatchExecutionHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MatchFormat")
                        .HasColumnName("match_format")
                        .HasColumnType("int");

                    b.Property<string>("MatchId")
                        .IsRequired()
                        .HasColumnName("match_id")
                        .HasColumnType("varchar(43)");

                    b.Property<int>("MatchLoseTeam")
                        .HasColumnName("match_lose_team")
                        .HasColumnType("int");

                    b.Property<int>("MatchWinTeam")
                        .HasColumnName("match_win_team")
                        .HasColumnType("int");

                    b.Property<int>("RuleFormat")
                        .HasColumnName("rule_format")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("MatchId");

                    b.ToTable("match_execution_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.MatchExitPlayerHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MatchExitReason")
                        .HasColumnName("match_exit_reason")
                        .HasColumnType("int");

                    b.Property<int>("MatchFormat")
                        .HasColumnName("match_format")
                        .HasColumnType("int");

                    b.Property<string>("MatchId")
                        .IsRequired()
                        .HasColumnName("match_id")
                        .HasColumnType("varchar(43)");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId", "Datetime");

                    b.ToTable("match_exit_player_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.MatchStartPlayerHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MatchFormat")
                        .HasColumnName("match_format")
                        .HasColumnType("int");

                    b.Property<string>("MatchId")
                        .IsRequired()
                        .HasColumnName("match_id")
                        .HasColumnType("varchar(43)");

                    b.Property<long?>("PlayerIdA1")
                        .HasColumnName("player_id_a_1")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdA2")
                        .HasColumnName("player_id_a_2")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdA3")
                        .HasColumnName("player_id_a_3")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdA4")
                        .HasColumnName("player_id_a_4")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdA5")
                        .HasColumnName("player_id_a_5")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdA6")
                        .HasColumnName("player_id_a_6")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdB1")
                        .HasColumnName("player_id_b_1")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdB2")
                        .HasColumnName("player_id_b_2")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdB3")
                        .HasColumnName("player_id_b_3")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdB4")
                        .HasColumnName("player_id_b_4")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdB5")
                        .HasColumnName("player_id_b_5")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerIdB6")
                        .HasColumnName("player_id_b_6")
                        .HasColumnType("bigint");

                    b.Property<int>("RuleFormat")
                        .HasColumnName("rule_format")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerIdA1", "Datetime");

                    b.HasIndex("PlayerIdA2", "Datetime");

                    b.HasIndex("PlayerIdA3", "Datetime");

                    b.HasIndex("PlayerIdA4", "Datetime");

                    b.HasIndex("PlayerIdA5", "Datetime");

                    b.HasIndex("PlayerIdA6", "Datetime");

                    b.HasIndex("PlayerIdB1", "Datetime");

                    b.HasIndex("PlayerIdB2", "Datetime");

                    b.HasIndex("PlayerIdB3", "Datetime");

                    b.HasIndex("PlayerIdB4", "Datetime");

                    b.HasIndex("PlayerIdB5", "Datetime");

                    b.HasIndex("PlayerIdB6", "Datetime");

                    b.ToTable("match_start_player_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.PartyHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnName("group_id")
                        .HasColumnType("varchar(44)");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId1")
                        .HasColumnName("player_id_1")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId2")
                        .HasColumnName("player_id_2")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId3")
                        .HasColumnName("player_id_3")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId4")
                        .HasColumnName("player_id_4")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId5")
                        .HasColumnName("player_id_5")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnName("type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("GroupId");

                    b.HasIndex("PlayerId", "Datetime");

                    b.ToTable("party_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.PlayerAccountCreateHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<int>("AccountType")
                        .HasColumnName("account_type")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnName("player_name")
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("PlayerId", "Datetime");

                    b.ToTable("player_account_create_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.PlayerExpHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Exp")
                        .HasColumnName("exp")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnName("level")
                        .HasColumnType("int");

                    b.Property<long>("PlayerId")
                        .HasColumnName("player_id")
                        .HasColumnType("bigint");

                    b.Property<int>("TotalExp")
                        .HasColumnName("total_exp")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Datetime");

                    b.HasIndex("PlayerId", "Datetime");

                    b.ToTable("player_exp_history");
                });

            modelBuilder.Entity("LogProcess.Databases.EvoGameLog.SessionCountHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("AreaName")
                        .IsRequired()
                        .HasColumnName("area_name")
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Count")
                        .HasColumnName("count")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datetime")
                        .HasColumnName("datetime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AreaName", "Datetime");

                    b.ToTable("session_count_history");
                });
#pragma warning restore 612, 618
        }
    }
}

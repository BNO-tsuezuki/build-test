﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using evolib.Databases.personal;

namespace evolib.Migrations.personalshard001
{
    [DbContext(typeof(PersonalDBShardManager.PersonalShard001))]
    [Migration("20210419063112_AddDiscipline")]
    partial class AddDiscipline
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("evolib.Databases.personal.Achievement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("achievementId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<bool>("notified");

                    b.Property<bool>("obtained");

                    b.Property<DateTime>("obtainedDate");

                    b.Property<long>("playerId");

                    b.Property<int>("value");

                    b.HasKey("Id");

                    b.HasIndex("playerId", "achievementId");

                    b.ToTable("Achievements");
                });

            modelBuilder.Entity("evolib.Databases.personal.AppOption", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<int>("category");

                    b.Property<int>("value0");

                    b.Property<int>("value1");

                    b.Property<int>("value10");

                    b.Property<int>("value11");

                    b.Property<int>("value12");

                    b.Property<int>("value13");

                    b.Property<int>("value14");

                    b.Property<int>("value15");

                    b.Property<int>("value16");

                    b.Property<int>("value17");

                    b.Property<int>("value18");

                    b.Property<int>("value19");

                    b.Property<int>("value2");

                    b.Property<int>("value20");

                    b.Property<int>("value21");

                    b.Property<int>("value22");

                    b.Property<int>("value23");

                    b.Property<int>("value24");

                    b.Property<int>("value25");

                    b.Property<int>("value26");

                    b.Property<int>("value27");

                    b.Property<int>("value28");

                    b.Property<int>("value29");

                    b.Property<int>("value3");

                    b.Property<int>("value30");

                    b.Property<int>("value31");

                    b.Property<int>("value32");

                    b.Property<int>("value33");

                    b.Property<int>("value34");

                    b.Property<int>("value35");

                    b.Property<int>("value36");

                    b.Property<int>("value37");

                    b.Property<int>("value38");

                    b.Property<int>("value39");

                    b.Property<int>("value4");

                    b.Property<int>("value40");

                    b.Property<int>("value41");

                    b.Property<int>("value42");

                    b.Property<int>("value43");

                    b.Property<int>("value44");

                    b.Property<int>("value45");

                    b.Property<int>("value46");

                    b.Property<int>("value47");

                    b.Property<int>("value48");

                    b.Property<int>("value49");

                    b.Property<int>("value5");

                    b.Property<int>("value6");

                    b.Property<int>("value7");

                    b.Property<int>("value8");

                    b.Property<int>("value9");

                    b.HasKey("playerId", "category");

                    b.ToTable("AppOptions");
                });

            modelBuilder.Entity("evolib.Databases.personal.AssetsInventory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("amount");

                    b.Property<string>("assetsId")
                        .IsRequired();

                    b.Property<long>("playerId");

                    b.HasKey("Id");

                    b.HasIndex("playerId", "assetsId");

                    b.ToTable("AssetsInventories");
                });

            modelBuilder.Entity("evolib.Databases.personal.BattlePass", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<int>("passId");

                    b.Property<DateTime>("createdDate");

                    b.Property<bool>("isPremium");

                    b.Property<int>("level");

                    b.Property<ulong>("levelExp");

                    b.Property<ulong>("nextExp");

                    b.Property<int>("rewardLevel");

                    b.Property<ulong>("totalExp");

                    b.Property<DateTime>("updatedDate");

                    b.HasKey("playerId", "passId");

                    b.ToTable("BattlePasses");
                });

            modelBuilder.Entity("evolib.Databases.personal.CareerRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("matchType");

                    b.Property<string>("mobileSuitId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<int>("numForAverage");

                    b.Property<long>("playerId");

                    b.Property<string>("recordItemId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<int>("seasonNo");

                    b.Property<double>("value");

                    b.HasKey("Id");

                    b.HasIndex("playerId", "matchType", "seasonNo", "recordItemId", "mobileSuitId");

                    b.ToTable("CareerRecords");
                });

            modelBuilder.Entity("evolib.Databases.personal.DateLog", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<DateTime>("FriendRequestPageLastView");

                    b.Property<DateTime>("OnlineStamp");

                    b.HasKey("playerId");

                    b.ToTable("DateLogs");
                });

            modelBuilder.Entity("evolib.Databases.personal.Discipline", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<DateTime>("expirationDate");

                    b.Property<int>("level");

                    b.Property<string>("text")
                        .IsRequired();

                    b.Property<string>("title")
                        .IsRequired();

                    b.HasKey("playerId");

                    b.ToTable("Disciplines");
                });

            modelBuilder.Entity("evolib.Databases.personal.FavoriteFriend", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<long>("favoritePlayerId");

                    b.HasKey("playerId", "favoritePlayerId");

                    b.ToTable("FavoriteFriends");
                });

            modelBuilder.Entity("evolib.Databases.personal.FriendRequest", b =>
                {
                    b.Property<long>("playerIdDst");

                    b.Property<long>("playerIdSrc");

                    b.Property<DateTime>("timeStamp");

                    b.HasKey("playerIdDst", "playerIdSrc");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("evolib.Databases.personal.GivenHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("amount");

                    b.Property<int>("giveType");

                    b.Property<DateTime>("obtainedDate");

                    b.Property<long>("playerId");

                    b.Property<string>("presentId")
                        .HasMaxLength(32);

                    b.Property<string>("text")
                        .HasMaxLength(120);

                    b.Property<int>("type");

                    b.HasKey("Id");

                    b.HasIndex("playerId", "obtainedDate");

                    b.ToTable("GivenHistorys");
                });

            modelBuilder.Entity("evolib.Databases.personal.ItemInventory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("isNew");

                    b.Property<string>("itemId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<int>("itemType");

                    b.Property<DateTime>("obtainedDate");

                    b.Property<int>("obtainedWay");

                    b.Property<long>("playerId");

                    b.HasKey("Id");

                    b.HasIndex("playerId", "itemId");

                    b.HasIndex("playerId", "itemType");

                    b.ToTable("ItemInventories");
                });

            modelBuilder.Entity("evolib.Databases.personal.MobileSuitOption", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<string>("mobileSuitId")
                        .HasMaxLength(32);

                    b.Property<int>("value0");

                    b.Property<int>("value1");

                    b.Property<int>("value10");

                    b.Property<int>("value100");

                    b.Property<int>("value101");

                    b.Property<int>("value102");

                    b.Property<int>("value103");

                    b.Property<int>("value104");

                    b.Property<int>("value105");

                    b.Property<int>("value106");

                    b.Property<int>("value107");

                    b.Property<int>("value108");

                    b.Property<int>("value109");

                    b.Property<int>("value11");

                    b.Property<int>("value110");

                    b.Property<int>("value111");

                    b.Property<int>("value112");

                    b.Property<int>("value113");

                    b.Property<int>("value114");

                    b.Property<int>("value115");

                    b.Property<int>("value116");

                    b.Property<int>("value117");

                    b.Property<int>("value118");

                    b.Property<int>("value119");

                    b.Property<int>("value12");

                    b.Property<int>("value120");

                    b.Property<int>("value121");

                    b.Property<int>("value122");

                    b.Property<int>("value123");

                    b.Property<int>("value124");

                    b.Property<int>("value125");

                    b.Property<int>("value126");

                    b.Property<int>("value127");

                    b.Property<int>("value128");

                    b.Property<int>("value129");

                    b.Property<int>("value13");

                    b.Property<int>("value130");

                    b.Property<int>("value131");

                    b.Property<int>("value132");

                    b.Property<int>("value133");

                    b.Property<int>("value134");

                    b.Property<int>("value135");

                    b.Property<int>("value136");

                    b.Property<int>("value137");

                    b.Property<int>("value138");

                    b.Property<int>("value139");

                    b.Property<int>("value14");

                    b.Property<int>("value15");

                    b.Property<int>("value16");

                    b.Property<int>("value17");

                    b.Property<int>("value18");

                    b.Property<int>("value19");

                    b.Property<int>("value2");

                    b.Property<int>("value20");

                    b.Property<int>("value21");

                    b.Property<int>("value22");

                    b.Property<int>("value23");

                    b.Property<int>("value24");

                    b.Property<int>("value25");

                    b.Property<int>("value26");

                    b.Property<int>("value27");

                    b.Property<int>("value28");

                    b.Property<int>("value29");

                    b.Property<int>("value3");

                    b.Property<int>("value30");

                    b.Property<int>("value31");

                    b.Property<int>("value32");

                    b.Property<int>("value33");

                    b.Property<int>("value34");

                    b.Property<int>("value35");

                    b.Property<int>("value36");

                    b.Property<int>("value37");

                    b.Property<int>("value38");

                    b.Property<int>("value39");

                    b.Property<int>("value4");

                    b.Property<int>("value40");

                    b.Property<int>("value41");

                    b.Property<int>("value42");

                    b.Property<int>("value43");

                    b.Property<int>("value44");

                    b.Property<int>("value45");

                    b.Property<int>("value46");

                    b.Property<int>("value47");

                    b.Property<int>("value48");

                    b.Property<int>("value49");

                    b.Property<int>("value5");

                    b.Property<int>("value50");

                    b.Property<int>("value51");

                    b.Property<int>("value52");

                    b.Property<int>("value53");

                    b.Property<int>("value54");

                    b.Property<int>("value55");

                    b.Property<int>("value56");

                    b.Property<int>("value57");

                    b.Property<int>("value58");

                    b.Property<int>("value59");

                    b.Property<int>("value6");

                    b.Property<int>("value60");

                    b.Property<int>("value61");

                    b.Property<int>("value62");

                    b.Property<int>("value63");

                    b.Property<int>("value64");

                    b.Property<int>("value65");

                    b.Property<int>("value66");

                    b.Property<int>("value67");

                    b.Property<int>("value68");

                    b.Property<int>("value69");

                    b.Property<int>("value7");

                    b.Property<int>("value70");

                    b.Property<int>("value71");

                    b.Property<int>("value72");

                    b.Property<int>("value73");

                    b.Property<int>("value74");

                    b.Property<int>("value75");

                    b.Property<int>("value76");

                    b.Property<int>("value77");

                    b.Property<int>("value78");

                    b.Property<int>("value79");

                    b.Property<int>("value8");

                    b.Property<int>("value80");

                    b.Property<int>("value81");

                    b.Property<int>("value82");

                    b.Property<int>("value83");

                    b.Property<int>("value84");

                    b.Property<int>("value85");

                    b.Property<int>("value86");

                    b.Property<int>("value87");

                    b.Property<int>("value88");

                    b.Property<int>("value89");

                    b.Property<int>("value9");

                    b.Property<int>("value90");

                    b.Property<int>("value91");

                    b.Property<int>("value92");

                    b.Property<int>("value93");

                    b.Property<int>("value94");

                    b.Property<int>("value95");

                    b.Property<int>("value96");

                    b.Property<int>("value97");

                    b.Property<int>("value98");

                    b.Property<int>("value99");

                    b.HasKey("playerId", "mobileSuitId");

                    b.ToTable("MobileSuitOptions");
                });

            modelBuilder.Entity("evolib.Databases.personal.MutePlayer", b =>
                {
                    b.Property<long>("playerIdSrc");

                    b.Property<long>("playerIdDst");

                    b.Property<bool>("text");

                    b.Property<DateTime>("timeStamp");

                    b.Property<bool>("voice");

                    b.HasKey("playerIdSrc", "playerIdDst");

                    b.ToTable("MutePlayers");
                });

            modelBuilder.Entity("evolib.Databases.personal.OwnMobileSuitSetting", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<string>("mobileSuitId")
                        .HasMaxLength(32);

                    b.Property<string>("bodySkinItemId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("emotionSlotItemId1")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("emotionSlotItemId2")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("emotionSlotItemId3")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("emotionSlotItemId4")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("mvpCelebrationItemId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("ornamentItemId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("stampSlotItemId1")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("stampSlotItemId2")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("stampSlotItemId3")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("stampSlotItemId4")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("voicePackItemId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("weaponSkinItemId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("playerId", "mobileSuitId");

                    b.ToTable("OwnMobileSuitSettings");
                });

            modelBuilder.Entity("evolib.Databases.personal.OwnVoicePackSetting", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<string>("mobileSuitId")
                        .HasMaxLength(32);

                    b.Property<string>("voicePackItemId")
                        .HasMaxLength(32);

                    b.Property<string>("voiceId1")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("voiceId2")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("voiceId3")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("voiceId4")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("playerId", "mobileSuitId", "voicePackItemId");

                    b.ToTable("OwnVoicePackSettings");
                });

            modelBuilder.Entity("evolib.Databases.personal.PlayerBasicInformation", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<DateTime>("firstLogin");

                    b.Property<int>("initialLevel");

                    b.Property<int>("openType");

                    b.Property<string>("playerIconItemId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("playerName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<bool>("pretendOffline");

                    b.Property<int>("tutorialProgress");

                    b.HasKey("playerId");

                    b.ToTable("PlayerBasicInformations");
                });

            modelBuilder.Entity("evolib.Databases.personal.PlayerBattleInformatin", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<int>("defeat");

                    b.Property<int>("draw");

                    b.Property<float>("rating");

                    b.Property<int>("victory");

                    b.HasKey("playerId");

                    b.ToTable("PlayerBattleInformations");
                });

            modelBuilder.Entity("evolib.Databases.personal.PresentBox", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("amount");

                    b.Property<DateTime>("beginDate");

                    b.Property<DateTime>("endDate");

                    b.Property<int>("giveType");

                    b.Property<long>("playerId");

                    b.Property<string>("presentId")
                        .HasMaxLength(32);

                    b.Property<string>("text")
                        .HasMaxLength(120);

                    b.Property<int>("type");

                    b.HasKey("Id");

                    b.HasIndex("playerId", "beginDate", "endDate");

                    b.ToTable("PresentBoxs");
                });

            modelBuilder.Entity("evolib.Databases.personal.ReplayUserHistory", b =>
                {
                    b.Property<long>("playerId");

                    b.Property<DateTime>("date");

                    b.Property<ulong>("masterDataVersion");

                    b.Property<string>("matchId")
                        .HasMaxLength(64);

                    b.Property<int>("matchType");

                    b.Property<ulong>("packageVersion");

                    b.Property<int>("resultType");

                    b.HasKey("playerId", "date");

                    b.ToTable("ReplayUserHistory");
                });
#pragma warning restore 612, 618
        }
    }
}

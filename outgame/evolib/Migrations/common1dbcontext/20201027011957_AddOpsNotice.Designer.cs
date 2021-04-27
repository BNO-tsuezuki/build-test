﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using evolib.Databases.common1;

namespace evolib.Migrations.common1dbcontext
{
    [DbContext(typeof(Common1DBContext))]
    [Migration("20201027011957_AddOpsNotice")]
    partial class AddOpsNotice
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("evolib.Databases.common1.Account", b =>
                {
                    b.Property<string>("account");

                    b.Property<int>("type");

                    b.Property<DateTime>("banExpiration");

                    b.Property<DateTime>("inserted")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("playerId");

                    b.Property<int>("privilegeLevel");

                    b.HasKey("account", "type");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("evolib.Databases.common1.EnabledVersion", b =>
                {
                    b.Property<int>("checkTarget");

                    b.Property<int>("referenceSrc");

                    b.Property<int>("build");

                    b.Property<int>("major");

                    b.Property<int>("minor");

                    b.Property<int>("patch");

                    b.Property<DateTime>("updateDate")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("checkTarget", "referenceSrc");

                    b.ToTable("EnabledVersions");
                });

            modelBuilder.Entity("evolib.Databases.common1.OpsNotice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("beginDate");

                    b.Property<bool>("enabledEnglish");

                    b.Property<bool>("enabledFrench");

                    b.Property<bool>("enabledGerman");

                    b.Property<bool>("enabledJapanese");

                    b.Property<DateTime>("endDate");

                    b.Property<string>("memo");

                    b.Property<string>("msgEnglish");

                    b.Property<string>("msgFrench");

                    b.Property<string>("msgGerman");

                    b.Property<string>("msgJapanese");

                    b.Property<int>("optNoticeType");

                    b.Property<bool>("release");

                    b.Property<int>("repeateIntervalMinutes");

                    b.Property<ulong>("target");

                    b.Property<int>("times");

                    b.Property<string>("titleEnglish");

                    b.Property<string>("titleFrench");

                    b.Property<string>("titleGerman");

                    b.Property<string>("titleJapanese");

                    b.Property<DateTime>("updateDate")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("OpsNotices");
                });

            modelBuilder.Entity("evolib.Databases.common1.PlayerId", b =>
                {
                    b.Property<long>("playerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("account")
                        .IsRequired();

                    b.Property<int>("accountType");

                    b.Property<DateTime>("inserted")
                        .ValueGeneratedOnAdd();

                    b.HasKey("playerId");

                    b.ToTable("PlayerIds");
                });
#pragma warning restore 612, 618
        }
    }
}

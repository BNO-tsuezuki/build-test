﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using evogmtool.Models;

namespace evogmtool.Migrations
{
    [DbContext(typeof(GmToolDbContext))]
    partial class GmToolDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("evogmtool.Models.AuthLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnName("account")
                        .HasColumnType("varchar(254)")
                        .HasMaxLength(254);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdAt")
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnName("ipAddress")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<sbyte>("Result")
                        .HasColumnName("result")
                        .HasColumnType("tinyint(4)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Account", "CreatedAt");

                    b.HasIndex("IpAddress", "CreatedAt");

                    b.ToTable("AuthLogs");
                });

            modelBuilder.Entity("evogmtool.Models.Domain", b =>
                {
                    b.Property<int>("DomainId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("domainId")
                        .HasColumnType("int");

                    b.Property<string>("DomainName")
                        .IsRequired()
                        .HasColumnName("domainName")
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("DomainId");

                    b.ToTable("Domains");
                });

            modelBuilder.Entity("evogmtool.Models.DomainRegion", b =>
                {
                    b.Property<int>("DomainId")
                        .HasColumnName("domainId")
                        .HasColumnType("int");

                    b.Property<int>("RegionId")
                        .HasColumnName("regionId")
                        .HasColumnType("int");

                    b.Property<int>("PublisherId")
                        .HasColumnName("publisherId")
                        .HasColumnType("int");

                    b.Property<ulong>("Target")
                        .HasColumnName("target")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("DomainId", "RegionId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("RegionId");

                    b.HasIndex("Target")
                        .IsUnique();

                    b.ToTable("DomainRegions");
                });

            modelBuilder.Entity("evogmtool.Models.DomainRegionLanguage", b =>
                {
                    b.Property<int>("DomainId")
                        .HasColumnName("domainId")
                        .HasColumnType("int");

                    b.Property<int>("RegionId")
                        .HasColumnName("regionId")
                        .HasColumnType("int");

                    b.Property<string>("LanguageCode")
                        .HasColumnName("languageCode")
                        .HasColumnType("char(2)");

                    b.HasKey("DomainId", "RegionId", "LanguageCode");

                    b.HasIndex("LanguageCode");

                    b.ToTable("DomainRegionLanguages");
                });

            modelBuilder.Entity("evogmtool.Models.Language", b =>
                {
                    b.Property<string>("LanguageCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("languageCode")
                        .HasColumnType("char(2)");

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasColumnName("languageName")
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("LanguageCode");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("evogmtool.Models.OperationLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdAt")
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<string>("Exception")
                        .HasColumnName("exception")
                        .HasColumnType("mediumtext");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnName("ipAddress")
                        .HasColumnType("varchar(45)")
                        .HasMaxLength(45);

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnName("method")
                        .HasColumnType("varchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("QueryString")
                        .HasColumnName("queryString")
                        .HasColumnType("text");

                    b.Property<string>("RequestBody")
                        .HasColumnName("requestBody")
                        .HasColumnType("mediumtext");

                    b.Property<string>("ResponseBody")
                        .HasColumnName("responseBody")
                        .HasColumnType("mediumtext");

                    b.Property<short>("StatusCode")
                        .HasColumnName("statusCode")
                        .HasColumnType("smallint(6)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnName("url")
                        .HasColumnType("tinytext");

                    b.Property<int?>("UserId")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("UserId", "CreatedAt");

                    b.ToTable("OperationLogs");
                });

            modelBuilder.Entity("evogmtool.Models.Publisher", b =>
                {
                    b.Property<int>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("publisherId")
                        .HasColumnType("int");

                    b.Property<string>("PublisherName")
                        .IsRequired()
                        .HasColumnName("publisherName")
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("PublisherId");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("evogmtool.Models.Region", b =>
                {
                    b.Property<int>("RegionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("regionId")
                        .HasColumnType("int");

                    b.Property<string>("RegionCode")
                        .IsRequired()
                        .HasColumnName("regionCode")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("RegionId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("evogmtool.Models.Timezone", b =>
                {
                    b.Property<string>("TimezoneCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("timezoneCode")
                        .HasColumnType("varchar(100)");

                    b.HasKey("TimezoneCode");

                    b.ToTable("Timezones");
                });

            modelBuilder.Entity("evogmtool.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("userId");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnName("account")
                        .HasColumnType("varchar(254)")
                        .HasMaxLength(254);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("createdAt")
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnName("isAvailable")
                        .HasColumnType("bit(1)");

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasColumnName("languageCode")
                        .HasColumnType("char(2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("passwordHash")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<int>("PublisherId")
                        .HasColumnName("publisherId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnName("role")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnName("salt")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("TimezoneCode")
                        .IsRequired()
                        .HasColumnName("timezoneCode")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("updatedAt")
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

                    b.HasKey("UserId");

                    b.HasIndex("Account")
                        .IsUnique();

                    b.HasIndex("LanguageCode");

                    b.HasIndex("PublisherId");

                    b.HasIndex("TimezoneCode");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("evogmtool.Models.DomainRegion", b =>
                {
                    b.HasOne("evogmtool.Models.Domain", "Domain")
                        .WithMany("DomainRegions")
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("evogmtool.Models.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("evogmtool.Models.Region", "Region")
                        .WithMany("DomainRegions")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("evogmtool.Models.DomainRegionLanguage", b =>
                {
                    b.HasOne("evogmtool.Models.Language", "Language")
                        .WithMany("DomainRegionLanguages")
                        .HasForeignKey("LanguageCode")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("evogmtool.Models.DomainRegion", "DomainRegion")
                        .WithMany("DomainRegionLanguages")
                        .HasForeignKey("DomainId", "RegionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("evogmtool.Models.OperationLog", b =>
                {
                    b.HasOne("evogmtool.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("evogmtool.Models.User", b =>
                {
                    b.HasOne("evogmtool.Models.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageCode")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("evogmtool.Models.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("evogmtool.Models.Timezone", "Timezone")
                        .WithMany()
                        .HasForeignKey("TimezoneCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

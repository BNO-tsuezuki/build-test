using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace evogmtool.Models
{
    public class GmToolDbContext : DbContext
    {
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<DomainRegion> DomainRegions { get; set; }
        public DbSet<DomainRegionLanguage> DomainRegionLanguages { get; set; }
        public DbSet<Timezone> Timezones { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<AuthLog> AuthLogs { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }

        public GmToolDbContext(DbContextOptions<GmToolDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildDomainModel(modelBuilder);
            BuildRegionModel(modelBuilder);
            BuildPublisherModel(modelBuilder);
            BuildLanguageModel(modelBuilder);
            BuildDomainRegionModel(modelBuilder);
            BuildDomainRegionLanguageModel(modelBuilder);
            BuildTimezoneModel(modelBuilder);

            BuildUserModel(modelBuilder);

            BuildAuthLogModel(modelBuilder);
            BuildOperationLogModel(modelBuilder);

            ConvertAllDateTimeValueFromDatabaseAsUtc(modelBuilder);
        }

        private static void BuildDomainModel(ModelBuilder modelBuilder)
        {
            // 開発用
            //modelBuilder.Entity<Domain>().HasData(new Domain { DomainId = 1, DomainName = "BNID", });
            //modelBuilder.Entity<Domain>().HasData(new Domain { DomainId = 2, DomainName = "BNA", });
        }

        private static void BuildRegionModel(ModelBuilder modelBuilder)
        {
            // 開発用
            //modelBuilder.Entity<Region>().HasData(new Region { RegionId = 1, RegionCode = "JP", });
            //modelBuilder.Entity<Region>().HasData(new Region { RegionId = 2, RegionCode = "NA", });
            //modelBuilder.Entity<Region>().HasData(new Region { RegionId = 3, RegionCode = "EU", });
        }

        private static void BuildPublisherModel(ModelBuilder modelBuilder)
        {
            // 開発用
            //modelBuilder.Entity<Publisher>().HasData(new Publisher { PublisherId = 1, PublisherName = "BNO", });
            //modelBuilder.Entity<Publisher>().HasData(new Publisher { PublisherId = 2, PublisherName = "BNEA", });
            //modelBuilder.Entity<Publisher>().HasData(new Publisher { PublisherId = 3, PublisherName = "BNEI", });
        }

        private static void BuildLanguageModel(ModelBuilder modelBuilder)
        {
            // 開発用
        //    modelBuilder.Entity<Language>().HasData(new Language { LanguageCode = "ja", LanguageName = "Japanese", });
        //    modelBuilder.Entity<Language>().HasData(new Language { LanguageCode = "en", LanguageName = "English", });
        //    modelBuilder.Entity<Language>().HasData(new Language { LanguageCode = "de", LanguageName = "German", });
        //    modelBuilder.Entity<Language>().HasData(new Language { LanguageCode = "fr", LanguageName = "French", });
        }

        private static void BuildDomainRegionModel(ModelBuilder modelBuilder)
        {
            // relation
            modelBuilder.Entity<DomainRegion>()
                .HasOne(domainRegion => domainRegion.Domain)
                .WithMany(domain => domain.DomainRegions)
                .HasForeignKey(domainRegion => domainRegion.DomainId);

            modelBuilder.Entity<DomainRegion>()
                .HasOne(domainRegion => domainRegion.Region)
                .WithMany(region => region.DomainRegions)
                .HasForeignKey(domainRegion => domainRegion.RegionId);

            modelBuilder.Entity<DomainRegion>()
                .HasIndex(domainRegion => domainRegion.Target)
                .IsUnique();

            // index
            modelBuilder.Entity<DomainRegion>()
                .HasKey(domainRegion => new
                {
                    domainRegion.DomainId,
                    domainRegion.RegionId,
                });

            // 開発用
            //modelBuilder.Entity<DomainRegion>().HasData(new DomainRegion { DomainId = 1, RegionId = 1, PublisherId = 1, Target = 1 });
            //modelBuilder.Entity<DomainRegion>().HasData(new DomainRegion { DomainId = 2, RegionId = 2, PublisherId = 2, Target = 2 });
            //modelBuilder.Entity<DomainRegion>().HasData(new DomainRegion { DomainId = 2, RegionId = 3, PublisherId = 2, Target = 3 });
        }

        private static void BuildDomainRegionLanguageModel(ModelBuilder modelBuilder)
        {
            // relation
            modelBuilder.Entity<DomainRegionLanguage>()
                .HasOne(domainRegionLanguage => domainRegionLanguage.DomainRegion)
                .WithMany(domainRegion => domainRegion.DomainRegionLanguages)
                .HasForeignKey(domainRegionLanguage => new { domainRegionLanguage.DomainId, domainRegionLanguage.RegionId });

            modelBuilder.Entity<DomainRegionLanguage>()
                .HasOne(domainRegionLanguage => domainRegionLanguage.Language)
                .WithMany(language => language.DomainRegionLanguages)
                .HasForeignKey(domainRegionLanguage => domainRegionLanguage.LanguageCode);

            // index
            modelBuilder.Entity<DomainRegionLanguage>()
                .HasKey(domainRegionLanguage => new
                {
                    domainRegionLanguage.DomainId,
                    domainRegionLanguage.RegionId,
                    domainRegionLanguage.LanguageCode,
                });

            // 開発用
            //modelBuilder.Entity<DomainRegionLanguage>().HasData(new DomainRegionLanguage { DomainId = 1, RegionId = 1, LanguageCode = "ja" });
            //modelBuilder.Entity<DomainRegionLanguage>().HasData(new DomainRegionLanguage { DomainId = 2, RegionId = 2, LanguageCode = "en" });
            //modelBuilder.Entity<DomainRegionLanguage>().HasData(new DomainRegionLanguage { DomainId = 2, RegionId = 2, LanguageCode = "de" });
            //modelBuilder.Entity<DomainRegionLanguage>().HasData(new DomainRegionLanguage { DomainId = 2, RegionId = 2, LanguageCode = "fr" });
            //modelBuilder.Entity<DomainRegionLanguage>().HasData(new DomainRegionLanguage { DomainId = 2, RegionId = 3, LanguageCode = "en" });
            //modelBuilder.Entity<DomainRegionLanguage>().HasData(new DomainRegionLanguage { DomainId = 2, RegionId = 3, LanguageCode = "de" });
            //modelBuilder.Entity<DomainRegionLanguage>().HasData(new DomainRegionLanguage { DomainId = 2, RegionId = 3, LanguageCode = "fr" });
        }

        private static void BuildTimezoneModel(ModelBuilder modelBuilder)
        {
            // 開発用
            //modelBuilder.Entity<Timezone>().HasData(new Timezone { TimezoneCode = "Japan" });
            //modelBuilder.Entity<Timezone>().HasData(new Timezone { TimezoneCode = "US/Pacific" });
            //modelBuilder.Entity<Timezone>().HasData(new Timezone { TimezoneCode = "US/Eastern" });
            //modelBuilder.Entity<Timezone>().HasData(new Timezone { TimezoneCode = "US/Central" });
            //modelBuilder.Entity<Timezone>().HasData(new Timezone { TimezoneCode = "US/Mountain" });
            //modelBuilder.Entity<Timezone>().HasData(new Timezone { TimezoneCode = "US/Alaska" });
            //modelBuilder.Entity<Timezone>().HasData(new Timezone { TimezoneCode = "US/Hawaii" });
        }

        private static void BuildUserModel(ModelBuilder modelBuilder)
        {
            // index
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Account)
                .IsUnique();

            // relation
            modelBuilder.Entity<User>()
                .HasOne(user => user.Publisher)
                .WithMany()
                .HasForeignKey(user => user.PublisherId);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Language)
                .WithMany()
                .HasForeignKey(user => user.LanguageCode);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Timezone)
                .WithMany()
                .HasForeignKey(user => user.TimezoneCode);

            // default
            modelBuilder.Entity<User>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            modelBuilder.Entity<User>()
                .Property(c => c.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");
        }

        private static void BuildAuthLogModel(ModelBuilder modelBuilder)
        {
            // index
            modelBuilder.Entity<AuthLog>()
                .HasIndex(c => c.CreatedAt);

            modelBuilder.Entity<AuthLog>()
                .HasIndex(c => new { c.Account, c.CreatedAt });

            modelBuilder.Entity<AuthLog>()
                .HasIndex(c => new { c.IpAddress, c.CreatedAt });

            // default
            modelBuilder.Entity<AuthLog>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        }

        private static void BuildOperationLogModel(ModelBuilder modelBuilder)
        {
            // index
            modelBuilder.Entity<OperationLog>()
                .HasIndex(c => c.CreatedAt);

            modelBuilder.Entity<OperationLog>()
                .HasIndex(c => new { c.UserId, c.CreatedAt });

            // default
            modelBuilder.Entity<OperationLog>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        }

        private static void ConvertAllDateTimeValueFromDatabaseAsUtc(ModelBuilder modelBuilder)
        {
            // todo: 場所
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }
        }
    }
}

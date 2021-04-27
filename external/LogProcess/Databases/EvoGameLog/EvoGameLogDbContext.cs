using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LogProcess.Databases.EvoGameLog
{
    public class EvoGameLogDbContext : DbContext
    {
        public EvoGameLogDbContext(DbContextOptions<EvoGameLogDbContext> options)
            : base(options)
        {
        }

        public DbSet<PlayerAccountCreateHistory> PlayerAccountCreateHistories { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<LogoutHistory> LogoutHistories { get; set; }
        public DbSet<PlayerExpHistory> PlayerExpHistories { get; set; }
        public DbSet<ChatSayHistory> ChatSayHistories { get; set; }
        public DbSet<ChatDirectHistory> ChatDirectHistories { get; set; }
        public DbSet<PartyHistory> PartyHistories { get; set; }
        public DbSet<MatchCueHistory> MatchCueHistories { get; set; }
        public DbSet<MatchStartPlayerHistory> MatchStartPlayerHistories { get; set; }
        public DbSet<MatchExecutionHistory> MatchExecutionHistories { get; set; }
        public DbSet<MatchExitPlayerHistory> MatchExitPlayerHistories { get; set; }
        public DbSet<MatchEntryPlayerHistory> MatchEntryPlayerHistories { get; set; }
        public DbSet<SessionCountHistory> SessionCountHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildPlayerAccountCreateHistoryModel(modelBuilder);
            BuildLoginHistoryModel(modelBuilder);
            BuildLogoutHistoryModel(modelBuilder);
            BuildPlayerExpHistoryModel(modelBuilder);
            BuildChatSayHistoryModel(modelBuilder);
            BuildChatDirectHistoryModel(modelBuilder);
            BuildPartyHistoryModel(modelBuilder);
            BuildMatchCueHistoryModel(modelBuilder);
            BuildMatchStartPlayerHistoryModel(modelBuilder);
            BuildMatchExecutionHistoryModel(modelBuilder);
            BuildMatchExitPlayerHistoryModel(modelBuilder);
            BuildMatchEntryPlayerHistoryModel(modelBuilder);
            BuildSessionCountHistoryModel(modelBuilder);

            ConvertAllDateTimeValueFromDatabaseAsUtc(modelBuilder);
        }

        private static void BuildPlayerAccountCreateHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerAccountCreateHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<PlayerAccountCreateHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });
        }

        private static void BuildLoginHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<LoginHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });

            modelBuilder.Entity<LoginHistory>()
                .HasIndex(c => new { c.RemoteIp, c.Datetime });
        }

        private static void BuildLogoutHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogoutHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<LogoutHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });

            modelBuilder.Entity<LogoutHistory>()
                .HasIndex(c => new { c.RemoteIp, c.Datetime });
        }

        private static void BuildPlayerExpHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerExpHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<PlayerExpHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });
        }

        private static void BuildChatSayHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatSayHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<ChatSayHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });

            modelBuilder.Entity<ChatSayHistory>()
                .HasIndex(c => new { c.GroupId, c.Datetime });

            modelBuilder.Entity<ChatSayHistory>()
                .HasIndex(c => new { c.MatchId, c.Datetime });
        }

        private static void BuildChatDirectHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatDirectHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<ChatDirectHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });

            modelBuilder.Entity<ChatDirectHistory>()
                .HasIndex(c => new { c.TargetPlayerId, c.Datetime });
        }

        private static void BuildPartyHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartyHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<PartyHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });

            modelBuilder.Entity<PartyHistory>()
                .HasIndex(c => c.GroupId);

            // todo: index 要検討
        }

        private static void BuildMatchCueHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchCueHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<MatchCueHistory>()
                .HasIndex(c => new { c.PlayerId1, c.Datetime });

            modelBuilder.Entity<MatchCueHistory>()
                .HasIndex(c => new { c.PlayerId2, c.Datetime });

            modelBuilder.Entity<MatchCueHistory>()
                .HasIndex(c => new { c.PlayerId3, c.Datetime });

            modelBuilder.Entity<MatchCueHistory>()
                .HasIndex(c => new { c.PlayerId4, c.Datetime });

            modelBuilder.Entity<MatchCueHistory>()
                .HasIndex(c => new { c.PlayerId5, c.Datetime });

            modelBuilder.Entity<MatchCueHistory>()
                .HasIndex(c => new { c.PlayerId6, c.Datetime });

            // todo: index 要検討
        }

        private static void BuildMatchStartPlayerHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdA1, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdA2, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdA3, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdA4, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdA5, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdA6, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdB1, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdB2, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdB3, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdB4, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdB5, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => new { c.PlayerIdB6, c.Datetime });

            modelBuilder.Entity<MatchStartPlayerHistory>()
                .HasIndex(c => c.MatchId);

            // todo: index 要検討
        }

        private static void BuildMatchExecutionHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchExecutionHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<MatchExecutionHistory>()
                .HasIndex(c => c.MatchId);
        }

        private static void BuildMatchExitPlayerHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchExitPlayerHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<MatchExitPlayerHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });

            modelBuilder.Entity<MatchExitPlayerHistory>()
                .HasIndex(c => c.MatchId);
        }

        private static void BuildMatchEntryPlayerHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchEntryPlayerHistory>()
                .HasIndex(c => c.Datetime);

            modelBuilder.Entity<MatchEntryPlayerHistory>()
                .HasIndex(c => new { c.PlayerId, c.Datetime });

            modelBuilder.Entity<MatchEntryPlayerHistory>()
                .HasIndex(c => c.MatchId);
        }

        private static void BuildSessionCountHistoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionCountHistory>()
                .HasIndex(c => new { c.AreaName, c.Datetime });
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

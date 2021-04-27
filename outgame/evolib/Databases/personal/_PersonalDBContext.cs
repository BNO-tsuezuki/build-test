using Microsoft.EntityFrameworkCore;

namespace evolib.Databases.personal
{
    public class PersonalDBContext : DbContext
    {
		public PersonalDBContext(DbContextOptions options)
		   : base(options)
		{
		}

        public DbSet<PlayerBasicInformation> PlayerBasicInformations { get; set; }
		public DbSet<PlayerBattleInformatin> PlayerBattleInformations { get; set; }
		public DbSet<OwnMobileSuitSetting> OwnMobileSuitSettings { get; set; }
		public DbSet<OwnVoicePackSetting> OwnVoicePackSettings { get; set; }
		public DbSet<ItemInventory> ItemInventories { get; set; }
		public DbSet<AssetsInventory> AssetsInventories { get; set; }
		public DbSet<FavoriteFriend> FavoriteFriends { get; set; }
		public DbSet<FriendRequest> FriendRequests { get; set; }
		public DbSet<MutePlayer> MutePlayers { get; set; }
		public DbSet<AppOption> AppOptions { get; set; }
		public DbSet<MobileSuitOption> MobileSuitOptions { get; set; }
		public DbSet<DateLog> DateLogs { get; set; }
		public DbSet<ReplayUserHistory> ReplayUserHistory { get; set; }
        public DbSet<BattlePass> BattlePasses { get; set; }
        public DbSet<CareerRecord> CareerRecords { get; set; }
		public DbSet<Achievement> Achievements { get; set; }
        public DbSet<PresentBox> PresentBoxs { get; set; }
        public DbSet<GivenHistory> GivenHistorys { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
		public DbSet<Discipline> Disciplines { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//いまのところ attribute annotationで複合キー貼れないのでこういう風にする
			//modelBuilder.Entity<FooModel>()
			//	.HasKey(e => new { e.Hage, e.Foo });

			//いまのところ attribute annotationでIndex貼れないのでこういう風にする
			//modelBuilder.Entity<FooModel>()
			//	.HasIndex(e => e.abcd);


			// *sequence 試したけど失敗
			// チャレンジするなら playerIdに[DatabaseGenerated(DatabaseGeneratedOption.None)]するのも忘れずに。
			//var seqPlayerId = "seqPlayerId";
			//modelBuilder.HasSequence<long>(seqPlayerId)
			//	.StartsAt(68000)
			//	.IsCyclic(true);
			//modelBuilder.Entity<PlayerBasicInformation>()
			//	.Property(e => e.playerId)
			//	.HasDefaultValueSql("NEXT VALUE FOR " + seqPlayerId);




			modelBuilder.Entity<ItemInventory>()
				.HasIndex(e => new { e.playerId, e.itemId });
			modelBuilder.Entity<ItemInventory>()
				.HasIndex(e => new { e.playerId, e.itemType });


			modelBuilder.Entity<AssetsInventory>()
				.HasIndex(e => new { e.playerId, e.assetsId });



			modelBuilder.Entity<OwnMobileSuitSetting>()
				.HasKey(e => new { e.playerId, e.mobileSuitId });
			modelBuilder.Entity<OwnVoicePackSetting>()
				.HasKey(e => new { e.playerId, e.mobileSuitId, e.voicePackItemId });



			modelBuilder.Entity<FavoriteFriend>()
				.HasKey(e => new { e.playerId, e.favoritePlayerId });

			modelBuilder.Entity<FriendRequest>()
				.HasKey(e => new { e.playerIdDst, e.playerIdSrc });

			modelBuilder.Entity<MutePlayer>()
				.HasKey(e => new { e.playerIdSrc, e.playerIdDst });



			modelBuilder.Entity<AppOption>()
				.HasKey(e => new { e.playerId, e.category });
			modelBuilder.Entity<MobileSuitOption>()
				.HasKey(e => new { e.playerId, e.mobileSuitId });


            modelBuilder.Entity<ReplayUserHistory>()
                .HasKey(e => new { e.playerId, e.date });


            modelBuilder.Entity<BattlePass>()
                .HasKey(e => new { e.playerId, e.passId });


            modelBuilder.Entity<CareerRecord>()
                .HasIndex(e => new { e.playerId, e.matchType, e.seasonNo, e.recordItemId, e.mobileSuitId });


            modelBuilder.Entity<Achievement>()
                .HasIndex(e => new { e.playerId, e.achievementId });


            modelBuilder.Entity<PresentBox>()
                .HasIndex(e => new { e.playerId, e.beginDate, e.endDate });


            modelBuilder.Entity<GivenHistory>()
                .HasIndex(e => new { e.playerId, e.obtainedDate });


            modelBuilder.Entity<Challenge>()
               .HasIndex(e => new { e.playerId });

		}
	}
}

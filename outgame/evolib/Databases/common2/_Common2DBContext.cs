using Microsoft.EntityFrameworkCore;


namespace evolib.Databases.common2
{
    public class Common2DBContext : DbContext
    {
		public Common2DBContext(DbContextOptions<Common2DBContext> options)
		   : base(options)
		{
		}
		public DbSet<Friend> Friends { get; set; }

		public DbSet<PlayerName> PlayerNames { get; set; }

		public DbSet<DisabledMobileSuit> DisabledMobileSuits { get; set; }

		public DbSet<GenericData> GenericDatas { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Friend>()
				.HasKey(e => new { e.playerIdL, e.playerIdR });
			modelBuilder.Entity<Friend>()
				.HasIndex(e => e.playerIdR);
		}
	}
}
